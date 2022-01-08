using MySqlConnector;
using Promo.Core;
using Promo.Core.Extensions;
using Promo.Rules.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Promo.Rules.Infrastructure
{
	public interface IRulesRepository
	{
		Task CreateRuleAsync(Models.Rule rule);
		Task<Models.Rule> GetAsync(Guid ruleUid);
		Task<IEnumerable<Models.Rule>> GetRules(string watchTypeCode);
		Task UpdateRuleAsAddedAsync(Guid ruleUid);
		Task<IEnumerable<Models.Rule>> GetRuleByHashTagAsync(string text);
	}

	public class RulesRepository : IRulesRepository
	{
		private readonly IDatabase _database;

		public RulesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateRuleAsync(Models.Rule rule)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "rule_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("ruleUid", rule.RuleUid)
				.AddParameter("campaignUid", rule.CampaignUid)
				.AddParameter("sweepUid", rule.SweepUid)
				.AddParameter("watchTypeCode", rule.WatchTypeCode.ToString())
				.AddParameter("watchValue", rule.WatchValue)
				.AddParameter("activeInd", rule.ActiveInd);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<Models.Rule> GetAsync(Guid ruleUid)
		{
			var rule = default(Models.Rule);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "rule_get";
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
				command.CommandText = "rules_get_by_watchvalue";
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
				command.CommandText = "rules_get_by_watch_type_code";
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

		public async Task UpdateRuleAsAddedAsync(Guid ruleUid)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "rule_update_as_added";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("ruleUid", ruleUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		private Models.Rule CreateRecordFrom(MySqlDataReader reader)
		{
			return new Models.Rule
			{
				RuleId = reader.GetInt32("rule_id"),
				CampaignUid = reader.GetGuid("campaign_uid"),
				SweepUid = reader.GetGuid("sweep_uid"),
				RuleUid = reader.GetGuid("rule_uid"),
				WatchValue = reader.GetString("watch_value"),
				WatchTypeCode = (WatchTypeCode)Enum.Parse(typeof(WatchTypeCode), reader.GetString("watch_type_code")),
				ActiveInd = reader.GetBoolean("active_ind"),
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
