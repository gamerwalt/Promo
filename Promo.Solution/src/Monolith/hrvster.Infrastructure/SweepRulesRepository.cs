using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure
{
	public interface ISweepRulesRepository
	{
		Task CreateRuleAsync(Models.Rule rule);
		Task<Models.Rule> GetAsync(Guid ruleUid);
		Task<IEnumerable<Models.Rule>> GetRules(string watchTypeCode);
		Task UpdateRuleAsAddedAsync(Guid ruleUid);
		Task<IEnumerable<Models.Rule>> GetRuleByHashTagAsync(string text);
		Task<IEnumerable<Models.Rule>> GetSweepRuleAsync(Guid sweepUid);
		Task UpdateRuleExternalId(Guid ruleUid, string externalId);
		Task DeleteAsync(Guid ruleUid);
	}

	public class SweepRulesRepository : ISweepRulesRepository
	{
		private readonly IDatabase _database;

		public SweepRulesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateRuleAsync(Models.Rule rule)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_rule_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("ruleUid", rule.RuleUid)
				.AddParameter("sweepUid", rule.Sweep.SweepUid)
				.AddParameter("watchTypeCode", rule.WatchTypeCode.ToString())
				.AddParameter("watchValue", rule.WatchValue)
				.AddParameter("externalId", rule.ExternalId)
				.AddParameter("activeInd", rule.ActiveInd)
				.AddParameter("mustBeFollowing", rule.MustBeFollowing)
				.AddParameter("scheduledInd", rule.ScheduledInd);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<Models.Rule> GetAsync(Guid ruleUid)
		{
			var rule = default(Models.Rule);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_rule_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("ruleUid", ruleUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					rule = CreateRecordFrom(reader);
				}
			}

			return rule;
		}

		public async Task<IEnumerable<Models.Rule>> GetRuleByHashTagAsync(string text)
		{
			var rules = new List<Models.Rule>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_rules_get_by_watchvalue";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("watchValue", text);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					rules.Add(CreateRecordFrom(reader));
				}
			}

			return rules;
		}

		public async Task<IEnumerable<Models.Rule>> GetRules(string watchTypeCode)
		{
			var rules = new List<Models.Rule>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_rules_get_by_watch_type_code";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("watchTypeCode", watchTypeCode);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					rules.Add(CreateRecordFrom(reader));
				}
			}

			return rules;
		}

		public async Task<IEnumerable<Models.Rule>> GetSweepRuleAsync(Guid sweepUid)
		{
			var rules = new List<Models.Rule>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_rules_get_by_sweepUid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("sweepUid", sweepUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					rules.Add(CreateRecordFrom(reader));
				}
			}

			return rules;
		}

		public async Task UpdateRuleAsAddedAsync(Guid ruleUid)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_rule_update_as_added";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("ruleUid", ruleUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		private Models.Rule CreateRecordFrom(MySqlDataReader reader)
		{
			var watchTypeCode = (WatchTypeCode)Enum.Parse(typeof(WatchTypeCode), reader.GetString("watch_type_code"));

			return new Models.Rule
			{
				RuleId = reader.GetInt32("rule_id"),
				Campaign = new Campaign
				{
					CampaignUid = reader.GetGuid("campaign_uid"),
					CampaignId = reader.GetInt32("campaign_id"),
					CampaignName = reader.GetString("campaign_name"),
					Description = reader.GetString("campaign_description"),
					ActiveInd = reader.GetBoolean("campaign_active_ind"),
					StartDate = reader.GetDateTime("campaign_start_date"),
					EndDate = reader.GetDateTime("campaign_end_date")
				},
				Sweep = new Sweep
				{
					SweepUid = reader.GetGuid("sweep_uid"),
					SweepId = reader.GetInt32("sweep_id"),
					Name = reader.GetString("sweep_name"),
					RulesUrl = GetStringValue(reader, "rules_url"),
					StartDate = reader.GetDateTime("start_date"),
					EndDate = reader.GetDateTime("end_date"),
					DrawDate = reader.GetDateTime("draw_date"),
					DateDrawn = GetNullableDateTimeValue(reader, "date_drawn")
				},
				RuleUid = reader.GetGuid("rule_uid"),
				WatchValue = reader.GetString("watch_value"),
				WatchTypeCode = (WatchTypeCode)Enum.Parse(typeof(WatchTypeCode), reader.GetString("watch_type_code")),
				ExternalId = GetLongValue(reader, "external_id"),
				ActiveInd = reader.GetBoolean("active_ind"),
				MustBeFollowing = reader.GetBoolean("must_be_following"),
				CreatedDate = reader.GetDateTime("created_date"),
				UpdatedDate = GetNullableDateTimeValue(reader, "updated_date")
			};
		}

		private string GetStringValue(MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if(string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}

			return reader.GetString(fieldName);
		}

		private long GetLongValue(MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if (string.IsNullOrWhiteSpace(value))
			{
				return 0;
			}

			return reader.GetInt64(fieldName);
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

		public async Task UpdateRuleExternalId(Guid ruleUid, string externalId)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_rule_update_external_id";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("ruleUid", ruleUid)
				.AddParameter("externalId", externalId);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task DeleteAsync(Guid ruleUid)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_rule_delete";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("ruleUid", ruleUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}
