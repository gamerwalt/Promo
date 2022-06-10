using hrvster.Models.Harvester;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using hrvster.Infrastructure.Extensions;

namespace hrvster.Infrastructure.Harvester
{
	public interface ISweepPrizeRepository
	{
		Task CreateSweepPrizeAsync(SweepPrize sweepPrize);
		Task<SweepPrize> GetSweepPrizeAsync(Guid prizeUid);
		Task<IEnumerable<SweepPrize>> GetSweepPrizesAsync(Guid sweepUid);
	}

	public class SweepPrizeRepository : ISweepPrizeRepository
	{
		private readonly IDatabase _database;

		public SweepPrizeRepository(IDatabase database)
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
				.AddParameter("sweepUid", sweepPrize.SweepUid)
				.AddParameter("prizeUid", sweepPrize.PrizeUid)
				.AddParameter("prizePosition", sweepPrize.PrizePosition)
				.AddParameter("enabledInd", sweepPrize.Enabled)
				.AddParameter("prizeName", sweepPrize.PrizeName)
				.AddParameter("prizeValue", sweepPrize.PrizeValue);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<SweepPrize> GetSweepPrizeAsync(Guid prizeUid)
		{
			var sweepPrize = default(SweepPrize);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_prizes_get_by_prize_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(prizeUid), prizeUid);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						sweepPrize = CreateRecordFrom(reader);
					}
				}
			}

			return sweepPrize;
		}

		public async Task<IEnumerable<SweepPrize>> GetSweepPrizesAsync(Guid sweepUid)
		{
			var sweepPrizes = new List<SweepPrize>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_prizes_get_by_sweep_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(sweepUid), sweepUid);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						sweepPrizes.Add(CreateRecordFrom(reader));
					}
				}
			}

			return sweepPrizes;
		}

		private SweepPrize CreateRecordFrom(MySqlDataReader reader)
		{
			return new SweepPrize
			{
				PrizeId = reader.GetInt32("prize_id"),
				PrizeUid = reader.GetGuid("prize_uid"),
				SweepUid = reader.GetGuid("sweep_uid"),
				PrizePosition = reader.GetInt32("prize_position"),
				Enabled = reader.GetBoolean("enabled_ind"),
				PrizeName = reader.GetString("prize_name"),
				PrizeValue = reader.GetDecimal("prize_value"),
				CreatedDate = reader.GetDateTime("created_date")
			};
		}
	}
}
