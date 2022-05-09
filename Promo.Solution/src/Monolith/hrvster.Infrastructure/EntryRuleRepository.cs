using hrvster.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure
{
	public interface IEntryRulesRepository
	{
		Task CreateAsync(EntryRule entryRule);
		Task<EntryRule> GetAsync(Guid entryRuleUid);
		Task<EntryRule> GetBySweepAndCampaignUidAsync(Guid sweepUid, Guid campaignUid);
		Task<EntryRule> GetSweepEntryRuleAsync(Guid sweepUid);
	}

	public class EntryRulesRepository : IEntryRulesRepository
	{
		private readonly IDatabase _database;

		public EntryRulesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(EntryRule entryRule)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "entry_rule_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("entryRuleUid", entryRule.EntryRuleUid)
				.AddParameter("campaignUid", entryRule.CampaignUid)
				.AddParameter("sweepUid", entryRule.SweepUid)
				.AddParameter("entryPerUser", entryRule.EntryPerUser);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<EntryRule> GetAsync(Guid entryRuleUid)
		{
			var entryRule = default(EntryRule);

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

		public async Task<EntryRule> GetBySweepAndCampaignUidAsync(Guid sweepUid, Guid campaignUid)
		{
			var entryRule = default(EntryRule);

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

		public async Task<EntryRule> GetSweepEntryRuleAsync(Guid sweepUid)
		{
			var entryRule = default(EntryRule);

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

		private EntryRule CreateRecordFrom(MySqlDataReader reader)
		{
			return new EntryRule
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
