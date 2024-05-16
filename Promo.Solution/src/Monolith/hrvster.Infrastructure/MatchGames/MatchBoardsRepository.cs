using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.MatchGames
{
	public interface IMatchBoardsRepository
	{
		Task BulkInsertBoardsAndTiles(MatchGame matchGame, List<MatchBoard> currentBatch, CancellationToken cancellationToken);
		Task CreateAsync(MatchBoard board, CancellationToken cancellationToken);
		Task<IEnumerable<MatchBoard>> GetForMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken);
		Task<MatchBoard> GetGameBoardAsync(Guid gameboardUid, CancellationToken token = default);
		/// <summary>
		/// Get a list of all the Board Uids for a specific game
		/// </summary>
		/// <param name="gameUid">The game unique identifier</param>
		/// <param name="cancellationToken">Cancellation Token</param>
		/// <returns>List<Guid></Guid></returns>
		Task<IEnumerable<Guid>> GetGameBoardUidsAsync(Guid gameUid, CancellationToken cancellationToken);
		Task<Dictionary<Guid, int>> GetInsertedBoardIds(List<MatchBoard> boards, CancellationToken cancellationToken);
		Task ReserveBoardAsync(Guid uid, CancellationToken cancellationToken);
		Task UpdateAsPlayedAsync(Guid boardUid, CancellationToken cancellationToken);
	}

	public class MatchBoardsRepository : IMatchBoardsRepository
	{
		private readonly IDatabase _database;
		private readonly ILogger<MatchBoardsRepository> _logger;

		public MatchBoardsRepository(IDatabase database, ILoggerFactory loggerFactory)
		{
			_database = database;
			_logger = loggerFactory.CreateLogger<MatchBoardsRepository>();
		}

		public async Task BulkInsertBoardsAndTiles(MatchGame matchGame, List<MatchBoard> boards, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			await conn.OpenAsync(cancellationToken);

			// Adjust MySQL settings for bulk operations
			_logger.LogInformation("Adjust MySQL settings for bulk operations.");
			var originalMaxPacket = await GetGlobalVariable(conn, "max_allowed_packet", cancellationToken);
			var originalForeignKeyChecks = await GetGlobalVariable(conn, "foreign_key_checks", cancellationToken);
			await SetGlobalVariable(conn, "max_allowed_packet", "419430400", cancellationToken);
			await SetGlobalVariable(conn, "foreign_key_checks", "0", cancellationToken);

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
				if(conn.State == ConnectionState.Closed)
				{
					await conn.OpenAsync(cancellationToken);
				}

				_logger.LogInformation("Restore original MySQL settings.");
				// Restore original MySQL settings
				await SetGlobalVariable(conn, "max_allowed_packet", originalMaxPacket, cancellationToken);
				await SetGlobalVariable(conn, "foreign_key_checks", originalForeignKeyChecks, cancellationToken);
			}
		}

		private async Task<string> GetGlobalVariable(MySqlConnection conn, string variableName, CancellationToken cancellationToken)
		{
			if (conn.State == ConnectionState.Closed)
			{
				await conn.OpenAsync(cancellationToken);
			}

			using var command = new MySqlCommand($"SELECT @@{variableName};", conn);
			return (await command.ExecuteScalarAsync()).ToString();
		}

		private async Task SetGlobalVariable(MySqlConnection conn, string variableName, string value, CancellationToken cancellationToken)
		{
			if (conn.State == ConnectionState.Closed)
			{
				await conn.OpenAsync(cancellationToken);
			}

			using var command = new MySqlCommand($"SET GLOBAL {variableName} = {value};", conn);
			await command.ExecuteNonQueryAsync();
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
			if(token.IsCancellationRequested)
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
			if(isWinnable)
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

		public async Task<Dictionary<Guid, int>> GetInsertedBoardIds(List<MatchBoard> boards, CancellationToken cancellationToken)
		{
			var insertedBoardIds = new Dictionary<Guid, int>();
			var batchSize = 10000; // Adjust based on your needs
			var totalBoards = boards.Count;

			using var conn = new MySqlConnection(_database.GetConnectionString());
			await conn.OpenAsync(cancellationToken);

			var originalMaxPacket = await GetGlobalVariable(conn, "max_allowed_packet", cancellationToken);
			var originalForeignKeyChecks = await GetGlobalVariable(conn, "foreign_key_checks", cancellationToken);
			await SetGlobalVariable(conn, "max_allowed_packet", "419430400", cancellationToken);
			await SetGlobalVariable(conn, "foreign_key_checks", "0", cancellationToken);

			try
			{
				for (var i = 0; i < totalBoards; i += batchSize)
				{
					var batch = boards.Skip(i).Take(batchSize).ToList();
					var boardUids = batch.Select(b => $"'{b.Uid}'").ToList();
					var uidsParameter = string.Join(",", boardUids);

					var query = $"SELECT board_id, board_uid FROM hrvster.match_boards WHERE board_uid IN ({uidsParameter})";

					using (var command = new MySqlCommand(query, conn))
					{
						using (var reader = await command.ExecuteReaderAsync(cancellationToken))
						{
							while (await reader.ReadAsync(cancellationToken))
							{
								var id = reader.GetInt32("board_id");
								var uid = reader.GetGuid("board_uid");

								insertedBoardIds[uid] = id;
							}
						}
					}
				}
			}
			finally
			{
				if (conn.State == ConnectionState.Closed)
				{
					await conn.OpenAsync(cancellationToken);
				}

				_logger.LogInformation("Restore original MySQL settings.");
				// Restore original MySQL settings
				await SetGlobalVariable(conn, "max_allowed_packet", originalMaxPacket, cancellationToken);
				await SetGlobalVariable(conn, "foreign_key_checks", originalForeignKeyChecks, cancellationToken);
			}

			return insertedBoardIds;
		}
	}
}
