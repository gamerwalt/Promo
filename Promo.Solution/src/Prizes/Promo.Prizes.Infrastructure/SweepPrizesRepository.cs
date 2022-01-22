using MySqlConnector;
using Promo.Core;
using Promo.Core.Extensions;
using Promo.Prizes.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Promo.Prizes.Infrastructure
{
	public interface ISweepPrizesRepository
	{
		Task CreateSweepPrizeAsync(SweepPrize sweepPrize);
		Task<SweepPrize> GetSweepPrizeAsync(Guid prizeUid);
		Task<IEnumerable<SweepPrize>> GetSweepPrizesAsync(Guid sweepUid);
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

		private SweepPrize CreateSweepPrizeFrom(MySqlDataReader reader)
		{
			return new SweepPrize
			{
				PrizeId = reader.GetInt32("prize_id"),
				PrizeUid = reader.GetGuid("prize_uid"),
				SweepUid = reader.GetGuid("sweep_uid"),
				Enabled = reader.GetBoolean("enabled_ind"),
				PrizeName = reader.GetString("prize_name"),
				PrizeDescription = reader.GetString("prize_description"),
				Locale = reader.GetString("locale"),
				PrizePosition = reader.GetInt32("prize_position"),
				PrizeValue = reader.GetDecimal("prize_value"),
				CreatedDate = reader.GetDateTime("created_date"),
				UpdatedDate = GetNullableDateTimeValue(reader, "updated_date")
			};
		}

		private DateTime? GetNullableDateTimeValue(MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			return reader.GetDateTime(fieldName);
		}
	}
}
