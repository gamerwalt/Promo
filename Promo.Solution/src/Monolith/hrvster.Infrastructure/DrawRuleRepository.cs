using hrvster.Models;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;
using hrvster.Infrastructure.Extensions;

namespace hrvster.Infrastructure
{
	public interface ISweepDrawRuleRepository
	{
		Task<DrawRule> GetSweepDrawRuleAsync(Guid sweepUid);
		Task CreateAsync(DrawRule drawRule);
		Task<DrawRule> GetDrawRuleAsync(Guid drawRuleUid);
		Task UpdateDrawRuleAsync(Guid drawRuleUid, int winnerCount, int winnerBackupCount);
	}

	public class SweepDrawRuleRepository : ISweepDrawRuleRepository
	{
		private readonly IDatabase _database;

		public SweepDrawRuleRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<DrawRule> GetSweepDrawRuleAsync(Guid sweepUid)
		{
			var drawRule = default(DrawRule);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweeps_draw_rule_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(sweepUid), sweepUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					drawRule = CreateRecordFrom(reader);
				}
			}

			return drawRule;
		}

		private DrawRule CreateRecordFrom(MySqlDataReader reader)
		{
			return new DrawRule
			{
				SweepDrawruleId = reader.GetInt32("sweep_draw_rule_id"),
				SweepDrawRuleUid = reader.GetGuid("sweep_draw_rule_uid"),
				WinnerCount = reader.GetInt32("winner_count"),
				WinnerBackupCount = reader.GetInt32("winner_backup_count"),
				CreatedDate = reader.GetDateTime("created_date"),
				UpdatedDate = GetNullableDateTimeValue(reader, "updated_date")
			};
		}

		private static DateTime? GetNullableDateTimeValue(MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			return reader.GetDateTime(fieldName);
		}

		public async Task CreateAsync(DrawRule drawRule)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_draw_rule_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("sweepDrawRuleUid", drawRule.SweepDrawRuleUid)
				.AddParameter("sweepUid", drawRule.SweepUid)
				.AddParameter("winnerCount", drawRule.WinnerCount)
				.AddParameter("winnerBackupCount", drawRule.WinnerBackupCount);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<DrawRule> GetDrawRuleAsync(Guid drawRuleUid)
		{
			var drawRule = default(DrawRule);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweeps_draw_rule_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(drawRuleUid), drawRuleUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					drawRule = CreateRecordFrom(reader);
				}
			}

			return drawRule;
		}

		public async Task UpdateDrawRuleAsync(Guid drawRuleUid, int winnerCount, int winnerBackupCount)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_draw_rule_update";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter(nameof(drawRuleUid), drawRuleUid)
				.AddParameter(nameof(winnerCount), winnerCount)
				.AddParameter(nameof(winnerBackupCount), winnerBackupCount);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}
