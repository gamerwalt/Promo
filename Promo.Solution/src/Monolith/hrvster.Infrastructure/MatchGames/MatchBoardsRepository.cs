using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.MatchGames
{
	public interface IMatchBoardsRepository
	{
		Task CreateAsync(MatchBoard board, CancellationToken cancellationToken);
		Task<IEnumerable<MatchBoard>> GetForMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken);
		Task<MatchBoard> GetGameBoardAsync(Guid gameboardUid);
		/// <summary>
		/// Get a list of all the Board Uids for a specific game
		/// </summary>
		/// <param name="gameUid">The game unique identifier</param>
		/// <param name="cancellationToken">Cancellation Token</param>
		/// <returns>List<Guid></Guid></returns>
		Task<IEnumerable<Guid>> GetGameBoardUidsAsync(Guid gameUid, CancellationToken cancellationToken);
		Task ReserveBoardAsync(Guid uid, CancellationToken cancellationToken);
		Task UpdateAsPlayedAsync(Guid boardUid, CancellationToken cancellationToken);
	}

	public class MatchBoardsRepository : IMatchBoardsRepository
	{
		private readonly IDatabase _database;

		public MatchBoardsRepository(IDatabase database)
		{
			_database = database;
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

		public async Task<MatchBoard> GetGameBoardAsync(Guid gameboardUid)
		{
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
	}
}
