using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using MySqlConnector;
using Newtonsoft.Json.Linq;
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
		Task DeleteAsync(Guid prizeUid, CancellationToken cancellationToken);
		MatchPrize Get(Guid matchPrizeUid);
		Task<MatchPrize> GetAsync(Guid prizeUid, CancellationToken cancellationToken);
		Task<MatchPrize> GetByGameboardUidAsync(Guid gameboardUid, CancellationToken cancellationToken);
		IEnumerable<MatchPrize> GetForMatchGame(Guid matchGameUid, object value);
		Task<IEnumerable<MatchPrize>> GetForMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken);
		Task UpdateAsync(Guid prizeUid, string name, string description, int quantity, decimal value, Guid imageUid, CancellationToken cancellationToken);
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
				.AddParameter("quantity", matchPrize.Quantity)
				.AddParameter("matchImageUid", matchPrize.Image.Uid);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<MatchPrize> GetByGameboardUidAsync(Guid gameboardUid, CancellationToken cancellationToken)
		{
			var matchPrize = default(MatchPrize);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_prize_get_by_gameboard_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("gameboardUid", gameboardUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchPrize = CreateFrom(reader);
				}
			}

			return matchPrize;
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
				Quantity = reader.GetInt32("quantity"),
				MatchGame = new MatchGame
				{
					Id = reader.GetInt32("match_game_id"),
					Uid = reader.GetGuid("match_game_uid")
				},
				Image = new MatchImage
				{
					Id = reader.GetInt32("image_id"),
					Uid = reader.GetGuid("image_uid"),
					Name = reader.GetStringValue("image_name"),
					LocationUrl = reader.GetStringValue("location_url")
				}
			};

		public IEnumerable<MatchPrize> GetForMatchGame(Guid matchGameUid, object value)
		{
			var matchPrizes = new List<MatchPrize>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_prizes_get_by_match_game_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("matchGameUid", matchGameUid);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					matchPrizes.Add(CreateFrom(reader));
				}
			}

			return matchPrizes;
		}

		public async Task<MatchPrize> GetAsync(Guid prizeUid, CancellationToken cancellationToken)
		{
			var matchPrize = default(MatchPrize);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_prize_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("prizeUid", prizeUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchPrize = CreateFrom(reader);
				}
			}

			return matchPrize;
		}

		public MatchPrize Get(Guid prizeUid)
		{
			var matchPrize = default(MatchPrize);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_prize_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("prizeUid", prizeUid);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					matchPrize = CreateFrom(reader);
				}
			}

			return matchPrize;
		}

		public async Task UpdateAsync(
			Guid prizeUid, 
			string name, 
			string description, 
			int quantity, 
			decimal value, 
			Guid imageUid, 
			CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_prize_update";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("matchPrizeUid", prizeUid)
				.AddParameter("matchPrizeName", name)
				.AddParameter("matchPrizeDescription", description)
				.AddParameter("matchPrizeValue", value)
				.AddParameter("quantity", quantity)
				.AddParameter("matchImageUid", imageUid);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task DeleteAsync(Guid prizeUid, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_prize_delete";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("matchPrizeUid", prizeUid);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}
	}
}
