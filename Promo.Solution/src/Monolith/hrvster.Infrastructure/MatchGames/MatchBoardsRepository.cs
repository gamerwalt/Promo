using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.MatchGames
{
	public interface IMatchBoardsRepository
	{
		Task BulkInsertBoardsAndNoTilesAsync(MatchGame matchGame, List<MatchBoard> boards, CancellationToken cancellationToken);
		Task BulkInsertBoardsAndTiles(MatchGame matchGame, List<MatchBoard> currentBatch, CancellationToken cancellationToken);
		Task CreateAsync(MatchBoard board, CancellationToken cancellationToken);
		Task<IEnumerable<MatchBoard>> GetForMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken);
		Task<IEnumerable<Guid>> GetForMatchGameByIdAsync(int gameId, CancellationToken token);
		Task<Guid> GetGameUidAsync(int gameId, CancellationToken token);
		Task<MatchBoard> GetGameBoardAsync(Guid gameboardUid, CancellationToken token = default);
		/// <summary>
		/// Get a list of all the Board Uids for a specific game
		/// </summary>
		/// <param name="gameUid">The game unique identifier</param>
		/// <param name="cancellationToken">Cancellation Token</param>
		/// <returns>List<Guid></Guid></returns>
		Task<IEnumerable<Guid>> GetGameBoardUidsAsync(Guid gameUid, CancellationToken cancellationToken);
		Task<Dictionary<Guid, int>> GetInsertedBoardIds(List<MatchBoard> boards, CancellationToken cancellationToken);
		Task<Guid> GetRandomBoardAsync(Guid gameUid, CancellationToken cancellationToken);
		Task ReserveBoardAsync(Guid uid, CancellationToken cancellationToken);
		Task SaveAllTilesAsync(List<MatchBoard> boardList, CancellationToken token);
		Task UpdateAsPlayedAsync(Guid boardUid, CancellationToken cancellationToken);
	}

	public class MatchBoardsRepository : IMatchBoardsRepository
	{
		private readonly IDatabase _database;
		private readonly ILogger<MatchBoardsRepository> _logger;
		private readonly IConfiguration _config;

		public MatchBoardsRepository(IDatabase database, ILoggerFactory loggerFactory, IConfiguration config)
		{
			_database = database;
			_logger = loggerFactory.CreateLogger<MatchBoardsRepository>();
			_config = config;
		}

		public async Task BulkInsertBoardsAndNoTilesAsync(
			MatchGame matchGame,
			List<MatchBoard> boards,
			CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			await conn.OpenAsync(cancellationToken);

			try
			{
				var boardValues = new ConcurrentBag<string>();
				await Parallel.ForEachAsync(boards, new ParallelOptions { CancellationToken = cancellationToken }, async (board, token) =>
				{
					var prizeId = board.IsWinnable ? $"{board.Prize.Id}" : "NULL";
					boardValues.Add($"('{board.Uid}', {board.MatchGame.Id}, {prizeId}, {Convert.ToInt32(board.IsWinnable)})");

					await Task.CompletedTask; // Placeholder for potential asynchronous work
				});

				var boardInsertQuery = $@"INSERT INTO match_boards (board_uid, match_game_id, prize_id, is_winnable) VALUES {string.Join(", ", boardValues)};";
				await ExecuteNonQuery(conn, boardInsertQuery, cancellationToken);
			}
			finally
			{
				if (conn.State == ConnectionState.Closed)
				{
					await conn.OpenAsync(cancellationToken);
				}
			}
		}

		public async Task SaveAllTilesAsync(List<MatchBoard> boardList, CancellationToken token)
		{
			if (token.IsCancellationRequested)
			{
				token.ThrowIfCancellationRequested();
			}
			var numberOfLoops = 0;
			var tileCount = 0;
			int.TryParse(_config["TileBatchToSave"], out var batchSize);
			int.TryParse(_config["TileTaskToAwait"], out var maxConcurrentTasks);

			var boardBatches = boardList.Batch(batchSize);
			var totalBoardBatches = boardBatches.ToList().Count;

			_logger.LogInformation("Total batches: {Total}", totalBoardBatches);


			foreach (var boardBatch in boardBatches)
			{
				Stopwatch clock = Stopwatch.StartNew();
				clock.Start();
				var total = boardBatch.Count();
				_logger.LogInformation("Working on batch of {Total} boards", total);

				var taskList = new List<Task>();

				foreach (var board in boardBatch)
				{
					var tileValues = new List<string>();

					foreach (var tile in board.Tiles)
					{
						tileCount++;
						var prizeId = tile.WinIndicator ? tile.Prize.Id.ToString() : "NULL";

						tileValues.Add(
							$"('{tile.Uid}', {board.Id}, {tile.Image.Id}, {prizeId}, {tile.WinIndicator}, {tile.WinIndicator})");
					}

					var query = $@"INSERT INTO match_tiles 
                           (tile_uid, board_id, image_id, prize_id, win_indicator, is_winnable) 
                           VALUES {string.Join(", ", tileValues)};";

					taskList.Add(Task.Run(async () =>
					{
						using var conn = new MySqlConnection(_database.GetConnectionString());
						await conn.OpenAsync(token);

						try
						{
							var cmd = new MySqlCommand(query, conn);
							await cmd.ExecuteNonQueryAsync(token);
						}
						finally
						{
							if (conn.State == ConnectionState.Open)
							{
								await conn.CloseAsync();
							}
						}
					}, token));

					if (taskList.Count >= maxConcurrentTasks)
					{
						await Task.WhenAll(taskList);

						taskList.Clear();
					}
				}

				// Await any remaining tasks in the list
				if (taskList.Any())
				{
					await Task.WhenAll(taskList);
					taskList.Clear();
				}

				numberOfLoops++;
				clock.Stop();
				_logger.LogInformation("***********************************************************");
				_logger.LogWarning("We have now looped a total of {NumberOfLoops}", numberOfLoops);
				_logger.LogWarning("Finished awaiting... total of {TileCount} saved successfully.", tileCount);
				_logger.LogWarning("Total elapsed time for {TileCount} is {ElapsedTime} minutes", tileCount, clock.Elapsed.TotalMinutes);
				_logger.LogInformation("***********************************************************");
			}

			_logger.LogInformation("All tiles have been processed and saved.");
		}

		public async Task<Guid> GetRandomBoardAsync(Guid gameUid, CancellationToken cancellationToken)
		{
			// Step 1: Query to get the total number of rows
			const string countQuery = @"
				SELECT COUNT(1) 
				FROM match_boards AS mb FORCE INDEX (idx_match_game_reserved)
				INNER JOIN match_games AS mg ON mg.match_game_id = mb.match_game_id
				WHERE mg.match_game_uid = @matchGameUid AND mb.is_reserved = 0";

			using var conn = new MySqlConnection(_database.GetConnectionString());
			await conn.OpenAsync(cancellationToken);

			using var countCmd = new MySqlCommand(countQuery, conn);
			countCmd.Parameters.AddWithValue("@matchGameUid", gameUid);

			var totalRows = Convert.ToInt32(await countCmd.ExecuteScalarAsync(cancellationToken));
			if (totalRows == 0)
				return Guid.Empty;

			// Step 2: Generate a random offset
			var random = new Random();
			var offset = random.Next(0, totalRows);

			// Step 3: Fetch a single random board using OFFSET
			const string randomBoardQuery = @"
				SELECT mb.board_uid 
				FROM match_boards AS mb FORCE INDEX (idx_match_game_reserved)
				INNER JOIN match_games AS mg ON mg.match_game_id = mb.match_game_id
				WHERE mg.match_game_uid = @matchGameUid AND mb.is_reserved = 0
				LIMIT 1 OFFSET @offset";

			using var randomBoardCmd = new MySqlCommand(randomBoardQuery, conn);
			randomBoardCmd.Parameters.AddWithValue("@matchGameUid", gameUid);
			randomBoardCmd.Parameters.AddWithValue("@offset", offset);

			var result = await randomBoardCmd.ExecuteScalarAsync(cancellationToken);

			return result == null || result == DBNull.Value
				? Guid.Empty
				: Guid.Parse(result.ToString());
		}

		public async Task<Guid> GetGameUidAsync(int gameId, CancellationToken token)
		{
			var query = $"SELECT match_game_uid FROM match_games WHERE match_game_id = {gameId}";
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = new MySqlCommand(query, conn);
			await conn.OpenAsync();
			var gameUid = (string)await command.ExecuteScalarAsync(token);

			if (string.IsNullOrWhiteSpace(gameUid))
			{
				return Guid.Empty;
			}

			return Guid.Parse(gameUid);
		}

		public async Task BulkInsertBoardsAndTiles(MatchGame matchGame, List<MatchBoard> boards, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			await conn.OpenAsync(cancellationToken);

			try
			{
				// Bulk insert boards
				var boardValues = new ConcurrentBag<string>();
				await Parallel.ForEachAsync(boards, new ParallelOptions { CancellationToken = cancellationToken }, async (board, token) =>
				{
					var tiles = JsonConvert.SerializeObject(board.Tiles.Select(tile => new
					{
						tile_uid = Guid.NewGuid().ToString(),
						image_id = tile.Image.Id,
						prize_id = tile.WinIndicator ? tile.Prize.Id.ToString() : "NULL",
						win_indicator = tile.WinIndicator ? 1 : 0
					}));
					string prizeId = board.IsWinnable ? $"{board.Prize.Id}" : "NULL";
					boardValues.Add($"('{board.Uid}', {board.MatchGame.Id}, {prizeId}, {Convert.ToInt32(board.IsWinnable)}, '{tiles}')");
					await Task.CompletedTask; // Placeholder for potential asynchronous work
				});

				var boardInsertQuery = $@"INSERT INTO match_boards (board_uid, match_game_id, prize_id, is_winnable, tiles) VALUES {string.Join(", ", boardValues)};";
				_logger.LogInformation("Executing query for the boards.");
				await ExecuteNonQuery(conn, boardInsertQuery, cancellationToken);
			}
			finally
			{
				if (conn.State == ConnectionState.Closed)
				{
					await conn.OpenAsync(cancellationToken);
				}
			}
		}

		private async Task ExecuteNonQuery(MySqlConnection conn, string query, CancellationToken cancellationToken)
		{
			if (conn.State == ConnectionState.Closed)
			{
				await conn.OpenAsync(cancellationToken);
			}

			using var command = new MySqlCommand(query, conn);
			command.CommandTimeout = 3600;
			await command.ExecuteNonQueryAsync();
		}

		public async Task CreateAsync(MatchBoard board, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_board_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("matchBoardUid", board.Uid)
				.AddParameter("isWinnable", board.IsWinnable)
				.AddParameter("matchGameUid", board.MatchGame.Uid);
			if (board.IsWinnable)
				command.AddParameter("matchPrizeUid", board.Prize.Uid);
			else
				command.AddParameter("matchPrizeUid", Guid.Empty);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<IEnumerable<MatchBoard>> GetForMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken)
		{
			var matchBoards = new List<MatchBoard>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_boards_get_by_match_game_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("matchGameUid", matchGameUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchBoards.Add(CreateFrom(reader));
				}
			}

			return matchBoards;
		}

		public async Task<MatchBoard> GetGameBoardAsync(Guid gameboardUid, CancellationToken token = default)
		{
			if (token.IsCancellationRequested)
			{
				token.ThrowIfCancellationRequested();
			}

			var matchBoard = default(MatchBoard);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_board_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("gameBoardUid", gameboardUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					matchBoard = CreateFrom(reader);
				}
			}

			return matchBoard;
		}

		/// <summary>
		/// Get a list of all the Board Uids for a specific game
		/// </summary>
		/// <param name="gameUid">The game unique identifier</param>
		/// <param name="cancellationToken">Cancellation Token</param>
		/// <returns>List<Guid></Guid></returns>
		public async Task<IEnumerable<Guid>> GetGameBoardUidsAsync(Guid gameUid, CancellationToken cancellationToken)
		{
			var matchBoardUids = new List<Guid>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_board_uids_get_by_game_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("gameUid", gameUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					matchBoardUids.Add(ReadGuidFrom(reader));
				}
			}

			return matchBoardUids;
		}

		private Guid ReadGuidFrom(MySqlDataReader reader)
		{
			return reader.GetGuid("board_uid");
		}

		private MatchBoard CreateFrom(MySqlDataReader reader)
		{
			var isWinnable = reader.GetBoolean("is_winnable");
			if (isWinnable)
			{
				return new MatchBoard()
				{
					Id = reader.GetInt32("board_id"),
					Uid = reader.GetGuid("board_uid"),
					IsWinnable = isWinnable,
					Prize = new MatchPrize
					{
						Uid = reader.GetGuid("prize_uid"),
						Id = reader.GetInt32("prize_id")
					},
					MatchGame = new MatchGame
					{
						Name = reader.GetString("match_game_name"),
						Uid = reader.GetGuid("match_game_uid"),
						Id = reader.GetInt32("match_game_id")
					}
				};
			}

			return new MatchBoard()
			{
				Id = reader.GetInt32("board_id"),
				Uid = reader.GetGuid("board_uid"),
				IsWinnable = isWinnable,
				MatchGame = new MatchGame
				{
					Name = reader.GetString("match_game_name"),
					Uid = reader.GetGuid("match_game_uid"),
					Id = reader.GetInt32("match_game_id")
				}
			};
		}

		public async Task ReserveBoardAsync(Guid uid, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_board_reserve";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("boardUid", uid);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task UpdateAsPlayedAsync(Guid boardUid, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_board_mark_as_played";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("boardUid", boardUid);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<IEnumerable<Guid>> GetForMatchGameByIdAsync(int gameId, CancellationToken token)
		{
			var boardUids = new List<Guid>();
			int.TryParse(_config["BatchGetIdFromDatabase"], out var batchSize); // Adjust based on your needs
			if (batchSize == 0)
			{
				batchSize = 50000;
			}

			using var conn = new MySqlConnection(_database.GetConnectionString());
			await conn.OpenAsync(token);
			var query = $"SELECT board_uid FROM match_boards WHERE match_game_id = {gameId}";
			using var command = conn.CreateCommand();
			command.CommandText = query;
			using (var reader = await command.ExecuteReaderAsync(token))
			{
				while(await reader.ReadAsync(token))
				{
					boardUids.Add(reader.GetGuid("board_uid"));
				}
			}

			return boardUids;
		}

		public async Task<Dictionary<Guid, int>> GetInsertedBoardIds(List<MatchBoard> boards, CancellationToken cancellationToken)
		{
			var insertedBoardIds = new Dictionary<Guid, int>();
			int.TryParse(_config["BatchGetIdFromDatabase"], out var batchSize); // Adjust based on your needs
			if (batchSize == 0)
			{
				batchSize = 50000;
			}
			var totalBoards = boards.Count;

			using var conn = new MySqlConnection(_database.GetConnectionString());
			await conn.OpenAsync(cancellationToken);

			try
			{
				for (var i = 0; i < totalBoards; i += batchSize)
				{
					var batch = boards.Skip(i).Take(batchSize).ToList();
					var boardUids = batch.Select(b => $"'{b.Uid}'").ToList();
					var uidsParameter = string.Join(",", boardUids);

					var query = $"SELECT board_id, board_uid FROM match_boards WHERE board_uid IN ({uidsParameter})";

					using var command = new MySqlCommand(query, conn);
					using var reader = await command.ExecuteReaderAsync(cancellationToken);
					while (await reader.ReadAsync(cancellationToken))
					{
						var id = reader.GetInt32("board_id");
						var uid = reader.GetGuid("board_uid");

						insertedBoardIds[uid] = id;
					}
				}
			}
			finally
			{
				if (conn.State == ConnectionState.Closed)
				{
					await conn.OpenAsync(cancellationToken);
				}
			}

			return insertedBoardIds;
		}
	}
}
