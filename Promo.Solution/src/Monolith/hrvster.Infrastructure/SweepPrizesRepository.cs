using hrvster.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using hrvster.Infrastructure.Extensions;

namespace hrvster.Infrastructure
{
	public interface ISweepPrizesRepository
	{
		Task CreateSweepPrizeAsync(SweepPrize sweepPrize);
		Task<SweepPrize> GetSweepPrizeAsync(Guid prizeUid);
		Task<IEnumerable<SweepPrize>> GetSweepPrizesAsync(Guid sweepUid);
		Task UpdateSweepPrize(Guid prizeUid, string prizeName, string prizeDescription, decimal monetaryValue);
		Task DeleteSweepPrizeAsync(Guid prizeUid);
		IEnumerable<SweepPrize> GetSweepPrizes(Guid sweepUid);
	}

	public class SweepPrizesRepository : ISweepPrizesRepository
	{
		private readonly IDatabase _database;

		public SweepPrizesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateSweepPrizeAsync(SweepPrize sweepPrize)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_prizes_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("prizeUid", sweepPrize.PrizeUid)
				.AddParameter("sweepUid", sweepPrize.SweepUid)
				.AddParameter("enabledInd", sweepPrize.Enabled)
				.AddParameter("prizeName", sweepPrize.PrizeName)
				.AddParameter("prizeDescription", sweepPrize.PrizeDescription)
				.AddParameter("prizeValue", sweepPrize.PrizeValue)
				.AddParameter("locale", sweepPrize.Locale)
				.AddParameter("prizePosition", sweepPrize.PrizePosition);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task DeleteSweepPrizeAsync(Guid prizeUid)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_prizes_delete";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("prizeUid", prizeUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<SweepPrize> GetSweepPrizeAsync(Guid prizeUid)
		{
			var sweepPrize = default(SweepPrize);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_prize_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(prizeUid), prizeUid);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						sweepPrize = CreateSweepPrizeFrom(reader);
					}
				}
			}

			return sweepPrize;
		}

		public IEnumerable<SweepPrize> GetSweepPrizes(Guid sweepUid)
		{
			var sweepPrizes = new List<SweepPrize>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_prizes_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(sweepUid), sweepUid);

				conn.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						sweepPrizes.Add(CreateSweepPrizeFrom(reader));
					}
				}
			}

			return sweepPrizes;
		}

		public async Task<IEnumerable<SweepPrize>> GetSweepPrizesAsync(Guid sweepUid)
		{
			var sweepPrizes = new List<SweepPrize>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_prizes_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(sweepUid), sweepUid);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						sweepPrizes.Add(CreateSweepPrizeFrom(reader));
					}
				}
			}

			return sweepPrizes;
		}

		public async Task UpdateSweepPrize(Guid prizeUid, string prizeName, string prizeDescription, decimal monetaryValue)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_prizes_update";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("prizeUid", prizeUid)
				.AddParameter("prizeName", prizeName)
				.AddParameter("prizeDescription", prizeDescription)
				.AddParameter("prizeValue", monetaryValue);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		private SweepPrize CreateSweepPrizeFrom(MySqlDataReader reader)
		{
			return new SweepPrize
			{
				PrizeId = reader.GetInt32("prize_id"),
				PrizeUid = reader.GetGuid("prize_uid"),
				Enabled = reader.GetBoolean("enabled_ind"),
				PrizeName = reader.GetString("prize_name"),
				PrizeDescription = reader.GetString("prize_description"),
				Locale = reader.GetString("locale"),
				PrizePosition = reader.GetInt32("prize_position"),
				PrizeValue = reader.GetDecimal("prize_value"),
				Sweep = new Sweep
				{
					SweepId = reader.GetInt32("sweep_id"),
					SweepUid = reader.GetGuid("sweep_uid")
				}
			};
		}
	}
}
