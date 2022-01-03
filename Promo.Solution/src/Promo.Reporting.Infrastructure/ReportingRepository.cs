using MySqlConnector;
using Promo.Core;
using Promo.Core.Extensions;
using Promo.Reporting.Infrastructure.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Promo.Reporting.Infrastructure
{
	public interface IReportingRepository
	{
		Task UpsertSweepEntryReportAsync(SweepEntryReport sweepEntryReport);
		Task<SweepEntryReport> GetCampaignSweepUserReportAsync(string screenName, string userIdentifierType, string externalUserId, Guid campaignUid, Guid sweepUid);
	}

	public class ReportingRepository : IReportingRepository
	{
		private readonly IDatabase _database;

		public ReportingRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<SweepEntryReport> GetCampaignSweepUserReportAsync(
			string screenName, 
			string userIdentifierType, 
			string externalUserId, 
			Guid campaignUid, Guid sweepUid)
		{
			var entryReport = default(SweepEntryReport);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_entry_report_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("campaignUid", campaignUid)
					.AddParameter("sweepUid", sweepUid)
					.AddParameter("userIdentifierType", userIdentifierType)
					.AddParameter("externalUserId", externalUserId)
					.AddParameter("screenName", screenName);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					entryReport = CreateRecordFrom(reader);
				}
			}

			return entryReport;
		}

		private SweepEntryReport CreateRecordFrom(MySqlDataReader reader)
		{
			return new SweepEntryReport
			{
				Id = reader.GetInt32("id"),
				SweepEntryUid = reader.GetGuid("sweep_entry_uid"),
				CampaignUid = reader.GetGuid("campaign_uid"),
				SweepUid = reader.GetGuid("sweep_uid"),
				ScreenName = reader.GetString("screen_name"),
				UserIdentifierType = reader.GetString("user_identifier_type"),
				ExternalId = reader.GetString("external_id"),
				ExternalUserId = reader.GetString("external_user_id"),
				Entries = reader.GetInt32("entries")
			};
		}

		public async Task UpsertSweepEntryReportAsync(SweepEntryReport sweepEntryReport)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_entry_upsert";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("sweepEntryReportUid", sweepEntryReport.SweepEntryUid)
				.AddParameter("campaignUid", sweepEntryReport.CampaignUid)
				.AddParameter("sweepUid", sweepEntryReport.SweepUid)
				.AddParameter("screenName", sweepEntryReport.ScreenName)
				.AddParameter("userIdentifierType", sweepEntryReport.UserIdentifierType)
				.AddParameter("externalId", sweepEntryReport.ExternalId)
				.AddParameter("externalUserId", sweepEntryReport.ExternalUserId)
				.AddParameter("count", sweepEntryReport.Entries);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}
