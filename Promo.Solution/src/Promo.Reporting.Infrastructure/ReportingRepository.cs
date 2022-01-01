using MySqlConnector;
using Promo.Core;
using Promo.Core.Extensions;
using Promo.Reporting.Infrastructure.Models;
using System.Data;
using System.Threading.Tasks;

namespace Promo.Reporting.Infrastructure
{
	public interface IReportingRepository
	{
		Task UpsertSweepEntryReportAsync(SweepEntryReport sweepEntryReport);
	}

	public class ReportingRepository : IReportingRepository
	{
		private readonly IDatabase _database;

		public ReportingRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task UpsertSweepEntryReportAsync(SweepEntryReport sweepEntryReport)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_entry_upsert";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("sweepEntryReportUid", sweepEntryReport.SweepEntryReportUid)
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
