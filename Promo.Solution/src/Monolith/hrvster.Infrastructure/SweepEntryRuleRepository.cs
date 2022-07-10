using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure
{
	public interface ISweepEntryRulesRepository
	{
		Task CreateAsync(SweepEntryRule entryRule);
		Task<SweepEntryRule> GetAsync(Guid entryRuleUid);
		Task<SweepEntryRule> GetBySweepAndCampaignUidAsync(Guid sweepUid, Guid campaignUid);
		Task<SweepEntryRule> GetSweepEntryRuleAsync(Guid sweepUid);
		Task UpdateSweepEntryRule(SweepEntryRule sweepEntryRule);
	}

	public class SweepEntryRulesRepository : ISweepEntryRulesRepository
	{
		private readonly IDatabase _database;

		public SweepEntryRulesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(SweepEntryRule entryRule)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_entry_rule_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("entryRuleUid", entryRule.EntryRuleUid)
				.AddParameter("sweepUid", entryRule.SweepUid)
				.AddParameter("entryPerUser", entryRule.EntryPerUser);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task UpdateSweepEntryRule(SweepEntryRule sweepEntryRule)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_entry_rule_update";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("entryRuleUid", sweepEntryRule.EntryRuleUid)
				.AddParameter("sweepUid", sweepEntryRule.SweepUid)
				.AddParameter("sweepId", sweepEntryRule.SweepId)
				.AddParameter("entryPerUser", sweepEntryRule.EntryPerUser);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<SweepEntryRule> GetAsync(Guid entryRuleUid)
		{
			var entryRule = default(SweepEntryRule);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "entry_rule_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("entryRuleUid", entryRuleUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					entryRule = CreateRecordFrom(reader);
				}
			}

			return entryRule;
		}

		public async Task<SweepEntryRule> GetBySweepAndCampaignUidAsync(Guid sweepUid, Guid campaignUid)
		{
			var entryRule = default(SweepEntryRule);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "entry_rule_get_by_sweep_and_campaign_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("sweepUid", sweepUid)
					.AddParameter("campaignUid", campaignUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					entryRule = CreateRecordFrom(reader);
				}
			}

			return entryRule;
		}

		public async Task<SweepEntryRule> GetSweepEntryRuleAsync(Guid sweepUid)
		{
			var entryRule = default(SweepEntryRule);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_entry_rule_get_by_sweep_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("sweepUid", sweepUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					entryRule = CreateRecordFrom(reader);
				}
			}

			return entryRule;
		}

		private SweepEntryRule CreateRecordFrom(MySqlDataReader reader)
		{
			return new SweepEntryRule
			{
				Id = reader.GetInt32("entry_rule_id"),
				EntryRuleUid = reader.GetGuid("entry_rule_uid"),
				EntryPerUser = reader.GetInt32("entry_per_user"),
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
