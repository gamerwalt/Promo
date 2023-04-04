using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using MySqlConnector;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.MatchGames
{
	public interface IMatchGamePlayRepository
	{
		Task CreateAsync(Guid gameplayUid, int id, CancellationToken cancellationToken);
		Task<MatchPlay> GetAsync(Guid gameplayUid, CancellationToken cancellationToken);
	}

	public class MatchGamePlayRepository : IMatchGamePlayRepository
	{
		private readonly IDatabase _database;

		public MatchGamePlayRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(Guid gameplayUid, int id, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_play_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("gameplayUid", gameplayUid)
				.AddParameter("gameId", id);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<MatchPlay> GetAsync(Guid gameplayUid, CancellationToken cancellationToken)
		{
			var matchPlay = default(MatchPlay);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_gameplay_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("gameplayUid", gameplayUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchPlay = CreateFrom(reader);
				}
			}

			return matchPlay;
		}

		private MatchPlay CreateFrom(MySqlDataReader reader)
		{
			return new MatchPlay
			{
				Id = reader.GetInt32("game_play_id"),
				Uid = reader.GetGuid("game_play_uid"),
				PlayedIndicator = reader.GetBoolean("played_ind"),
				MatchBoard = new MatchBoard
				{
					Id = reader.GetInt32("board_id"),
					Uid = reader.GetGuid("board_uid"),
					IsWinnable = reader.GetBoolean("is_winnable"),
					PlayedIndicator = reader.GetBoolean("board_played_ind"),
					IsReserved = reader.GetBoolean("is_reserved")
				}
			};
		}
	}
}
