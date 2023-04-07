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
		Task UpdateRuleAsAddedAsync(Guid ruleUid);
		Task UpdateRuleExternalId(Guid ruleUid, string externalId);
		Task DeleteAsync(Guid ruleUid);
		Task DeactivateAsync(Guid sweepUid, Guid ruleUid);
		Task ActivateAsync(Guid sweepUid, Guid ruleUid);

		Task<Models.Rule> GetAsync(Guid ruleUid);
		Task<IEnumerable<Models.Rule>> GetRules(string watchTypeCode);
		Task<IEnumerable<Models.Rule>> GetRuleByHashTagAsync(string text);
		Task<IEnumerable<Models.Rule>> GetSweepRuleAsync(Guid sweepUid);
		Task<IEnumerable<Models.Rule>> GetSweepRuleAsync();
		IEnumerable<Models.Rule> GetRuleMatching(string watchValue);
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

		public async Task<IEnumerable<Models.Rule>> GetSweepRuleAsync()
		{
			var rules = new List<Models.Rule>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_rules_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("currentUtc", DateTime.UtcNow);

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
			return new Models.Rule
			{
				RuleId = reader.GetInt32("rule_id"),
				Campaign = new Campaign
				{
					CampaignUid = reader.GetGuid("campaign_uid"),
					CampaignId = reader.GetInt32("campaign_id"),
					CampaignName = reader.GetStringValue("campaign_name"),
					Description = reader.GetStringValue("campaign_description"),
					ActiveInd = reader.GetBoolean("campaign_active_ind"),
					StartDate = reader.GetDateTime("campaign_start_date"),
					StartDateUtc = reader.GetDateTime("campaign_start_date_utc"),
					EndDate = reader.GetDateTime("campaign_end_date"),
					EndDateUtc = reader.GetDateTime("campaign_end_date_utc"),
					CampaignType = new CampaignType
					{
						CampaignTypeName = reader.GetStringValue("campaign_type_name"),
						CampaignTypeCode = (CampaignTypeCode)Enum.Parse(typeof(CampaignTypeCode), reader.GetStringValue("campaign_type_code")),
						CamapaignTypeDescription = reader.GetStringValue("campaign_type_description")
					},
					Account = new Account
					{
						AccountUid = reader.GetGuid("account_uid")
					}
				},
				Sweep = new Sweep
				{
					SweepUid = reader.GetGuid("sweep_uid"),
					SweepId = reader.GetInt32("sweep_id"),
					Name = reader.GetStringValue("sweep_name"),
					RulesUrl = reader.GetStringValue("rules_url"),
					StartDate = reader.GetDateTime("start_date"),
					StartDateUtc = reader.GetDateTime("start_date_utc"),
					EndDate = reader.GetDateTime("end_date"),
					EndDateUtc = reader.GetDateTime("end_date_utc"),
					DrawDate = reader.GetDateTime("draw_date"),
					DrawDateUtc =reader.GetDateTime("draw_date_utc"),
					DateDrawn = reader.GetNullableDateTimeValue("date_drawn"),
					DrawAttempted = reader.GetBoolean("draw_attempted"),
					Account = new Account
					{
						AccountUid = reader.GetGuid("account_uid")
					}
				},
				Game = new Game
				{
					Id = reader.GetInt32("sweep_id"),
					Uid = reader.GetGuid("sweep_uid"),
					Name = reader.GetString("sweep_name"),
					GameType = GameTypesEnum.Sweeps,
					StartDate = reader.GetDateTime("start_date"),
					StartDateUtc = reader.GetDateTime("start_date_utc"),
					EndDate = reader.GetDateTime("end_date"),
					EndDateUtc = reader.GetDateTime("end_date_utc")
				},
				RuleUid = reader.GetGuid("rule_uid"),
				WatchValue = reader.GetStringValue("watch_value"),
				WatchTypeCode = (WatchTypeCode)Enum.Parse(typeof(WatchTypeCode), reader.GetStringValue("watch_type_code")),
				ExternalId = reader.GetStringValue("external_id"),
				ActiveInd = reader.GetBoolean("active_ind"),
				MustBeFollowing = reader.GetBoolean("must_be_following"),
				NotifyWinners = reader.GetBoolean("notify_winners"),
				CreatedDate = reader.GetDateTime("created_date"),
				UpdatedDate = reader.GetNullableDateTimeValue("updated_date")
			};
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

		public async Task DeactivateAsync(Guid sweepUid, Guid ruleUid)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_rule_deactivate";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("ruleUid", ruleUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task ActivateAsync(Guid sweepUid, Guid ruleUid)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_rule_activate";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("ruleUid", ruleUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public IEnumerable<Models.Rule> GetRuleMatching(string watchValue)
		{
			var rules = new List<Models.Rule>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_rule_get_matching";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("watchValue", watchValue);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					rules.Add(CreateRecordFrom(reader));
				}
			}

			return rules;
		}
	}
}
