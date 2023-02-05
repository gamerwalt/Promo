using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.MatchGames
{
	public interface IMatchPrizesRepository
	{
		Task CreateAsync(MatchPrize matchPrize, CancellationToken cancellationToken);
		Task<IEnumerable<MatchPrize>> GetForMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken);
	}

	public class MatchPrizesRepository : IMatchPrizesRepository
	{
		private readonly IDatabase _database;

		public MatchPrizesRepository(IDatabase database)
			=> _database = database;

		public async Task CreateAsync(MatchPrize matchPrize, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_prize_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("matchGameUid", matchPrize.MatchGame.Uid)
				.AddParameter("matchPrizeUid", matchPrize.Uid)
				.AddParameter("matchPrizeName", matchPrize.Name)
				.AddParameter("matchPrizeDescription", matchPrize.Description)
				.AddParameter("matchPrizeValue", matchPrize.Value)
				.AddParameter("matchImageUid", matchPrize.Image.Uid);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<IEnumerable<MatchPrize>> GetForMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken)
		{
			var matchPrizes = new List<MatchPrize>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_prizes_get_by_match_game_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("matchGameUid", matchGameUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchPrizes.Add(CreateFrom(reader));
				}
			}

			return matchPrizes;
		}

		private static MatchPrize CreateFrom(MySqlDataReader reader)
			=> new MatchPrize
			{
				Id = reader.GetInt32("prize_id"),
				Uid = reader.GetGuid("prize_uid"),
				Name = reader.GetString("prize_name"),
				Description = reader.GetString("prize_description"),
				Value = reader.GetDecimal("prize_value"),
				MatchGame = new MatchGame
				{
					Id = reader.GetInt32("match_game_id"),
					Uid = reader.GetGuid("match_game_uid")
				},
				Image = new MatchImage
				{
					Id = reader.GetInt32("image_id"),
					Uid = reader.GetGuid("image_uid")
				}
			};
	}
}
