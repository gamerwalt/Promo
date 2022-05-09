using hrvster.Models;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure
{
	public interface IDrawRuleRepository
	{
		Task<DrawRule> GetSweepDrawRuleAsync(Guid sweepUid);
	}

	public class DrawRuleRepository : IDrawRuleRepository
	{
		private readonly IDatabase _database;

		public DrawRuleRepository(IDatabase database)
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
	}
}
