using hrvster.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using hrvster.Infrastructure.Extensions;

namespace hrvster.Infrastructure
{
	public interface ISweepsEntriesRepository
	{
		Task CreateAsync(SweepEntry sweepEntry);
		Task<SweepEntry> GetAsync(Guid sweepEntryGuid);
		Task CreateCampaignSweepsEntryAsync(CampaignSweepsEntry campaignSweepsEntry);
		Task<IEnumerable<SweepEntry>> GetSweepEntriesAsync(Guid sweepUid);
		Task<IEnumerable<HarvestedTweet>> GetHarvestedTweetsAsync(Guid userUid, Guid campaignUid);
		Task<IEnumerable<SweepEntryChart>> GetSweepEntriesByCampaignAndUserAsync(Guid userUid, Guid campaignUid);
		Task<IEnumerable<SweepEntry>> GetAccountSweepEntriesAsync(Guid accountUid);
	}

	public class SweepsEntriesRepository : ISweepsEntriesRepository
	{
		private readonly IDatabase _database;

		public SweepsEntriesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(SweepEntry sweepEntry)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_add_entry";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("sweepEntryUid", sweepEntry.SweepEntryUid)
				.AddParameter("sweepUid", sweepEntry.Sweep.SweepUid)
				.AddParameter("ruleUid", sweepEntry.Rule.RuleUid)
				.AddParameter("userIdentifier", sweepEntry.UserIdentifier)
				.AddParameter("userIdentifierType", sweepEntry.UserIdentifierType.ToString())
				.AddParameter("acceptedTerms", sweepEntry.AcceptedTerms);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task CreateCampaignSweepsEntryAsync(CampaignSweepsEntry sweepEntry)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_add_entry_via_campaign_uid";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("sweepEntryUid", sweepEntry.SweepEntryUid)
				.AddParameter("sweepUid", sweepEntry.SweepUid)
				.AddParameter("campaignUid", sweepEntry.CampaignUid)
				.AddParameter("userIdentifier", sweepEntry.UserIdentifier)
				.AddParameter("userIdentifierType", sweepEntry.UserIdentifierType.ToString())
				.AddParameter("acceptedTerms", sweepEntry.AcceptedTerms);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<SweepEntry> GetAsync(Guid sweepEntryGuid)
		{
			var sweepEntry = default(SweepEntry);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_entry_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(sweepEntryGuid), sweepEntryGuid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					sweepEntry = CreateSweepEntryFrom(reader);
				}
			}

			return sweepEntry;
		}

		public async Task<IEnumerable<HarvestedTweet>> GetHarvestedTweetsAsync(Guid userUid, Guid campaignUid)
		{
			var harvestedTweets = new List<HarvestedTweet>();
			var fifteenMiniutes = DateTime.Now.AddMinutes(-15);
			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "harvested_tweets_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", userUid);
				command.AddParameter("campaignUid", campaignUid);
				command.AddParameter("previousTime", fifteenMiniutes);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					harvestedTweets.Add(CreateHarvestedTweetFrom(reader));
				}
			}

			return harvestedTweets;
		}

		public async Task<IEnumerable<SweepEntryChart>> GetSweepEntriesByCampaignAndUserAsync(Guid userUid, Guid campaignUid)
		{
			var entries = new List<SweepEntryChart>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "tweet_entries_get_by_campaign_and_user";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", userUid)
					.AddParameter("campaignUid", campaignUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					entries.Add(CreateSweepEntryChar(reader));
				}
			}

			return entries;
		}

		private SweepEntryChart CreateSweepEntryChar(MySqlDataReader reader)
		{
			return new SweepEntryChart
			{
				Count = reader.GetInt32("count"),
				Datepart = reader.GetDateTime("the_time"),
				SweepName = reader.GetString("sweep_name")
			};
		}

		private HarvestedTweet CreateHarvestedTweetFrom(MySqlDataReader reader)
		{
			return new HarvestedTweet
			{
				Count = reader.GetInt32("count"),
				Datepart = reader.GetDateTime("the_time"),
				SweepName = reader.GetString("sweep_name"),
				SweepUid = reader.GetGuid("sweep_uid")
			};
		}

		public async Task<IEnumerable<SweepEntry>> GetSweepEntriesAsync(Guid sweepUid)
		{
			var sweepEntries = new List<SweepEntry>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_entries_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(sweepUid), sweepUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					sweepEntries.Add(CreateSweepEntryFrom(reader));
				}
			}

			return sweepEntries;
		}

		private SweepEntry CreateSweepEntryFrom(MySqlDataReader reader)
		{
			return new SweepEntry
			{
				SweepEntryUid = reader.GetGuid("sweep_entry_uid"),
				SweepEntryId = reader.GetInt32("sweep_entry_id"),
				UserIdentifier = reader.GetString("user_identifier"),
				UserIdentifierType = (UserIdentifierType)Enum.Parse(typeof(UserIdentifierType), reader.GetString("user_identifier_type")),
				AcceptedTerms = reader.GetBoolean("accepted_terms"),
				CreatedDate = reader.GetDateTime("created_date"),
				Sweep = new Sweep
				{
					SweepId = reader.GetInt32("sweep_id"),
					SweepUid = reader.GetGuid("sweep_uid"),
					Name = reader.GetString("sweep_name"),
					StartDate = reader.GetDateTime("start_date"),
					StartDateUtc = reader.GetDateTime("start_date_utc"),
					EndDate = reader.GetDateTime("end_date"),
					EndDateUtc = reader.GetDateTime("end_date_utc"),
					DrawDate = reader.GetDateTime("draw_date"),
					DrawDateUtc = reader.GetDateTime("draw_date_utc"),
					DrawAttempted = reader.GetBoolean("draw_attempted"),
					CreatedDate = reader.GetDateTime("sweep_created_date")
				}
			};
		}

		public async Task<IEnumerable<SweepEntry>> GetAccountSweepEntriesAsync(Guid accountUid)
		{
			var sweepEntries = new List<SweepEntry>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_entries_get_by_account_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(accountUid), accountUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					sweepEntries.Add(CreateSweepEntryFrom(reader));
				}
			}

			return sweepEntries;
		}
	}
}
