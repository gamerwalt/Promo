using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Reporting
{
	public interface IMatchGamePlayReportingRepository
	{
		Task CreateAsync(Guid gameUid, Guid campaignUid, Guid gameplayUid, Guid boardUid, string screenName, string externalId, ExternalClientType externalClientType);
		Task<MatchGamePlayReport> GetGamePlayReportAsync(Guid gameUid, Guid campaignUid, string userIdentifierType, string externalUserId, string screenName, CancellationToken cancellationToken);
	}

	public class MatchGamePlayReportingRepository : IMatchGamePlayReportingRepository
	{
		private readonly IDatabase _database;

		public MatchGamePlayReportingRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(Guid gameUid, Guid campaignUid, Guid gameplayUid, Guid boardUid, string screenName, string externalUserId, ExternalClientType externalClientType)
		{
			using var conn = new MySqlConnection(_database.GetReportingConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_game_plays_upsert";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("gameUid", gameUid)
				.AddParameter("campaignUid", campaignUid)
				.AddParameter("screenName", screenName)
				.AddParameter("userIdentifierType", externalClientType.ToString())
				.AddParameter("externalUserId", externalUserId)
				.AddParameter("gameplayUid", gameplayUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<MatchGamePlayReport> GetGamePlayReportAsync(
			Guid gameUid,
			Guid campaignUid,
			string userIdentifierType,
			string externalUserId,
			string screenName,
			CancellationToken cancellationToken)
		{
			var report = default(MatchGamePlayReport);

			using (var conn = new MySqlConnection(_database.GetReportingConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_game_plays_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("gameUid", gameUid)
					.AddParameter("campaignUid", campaignUid)
					.AddParameter("userIdentifierType", userIdentifierType)
					.AddParameter("externalUserId", externalUserId)
					.AddParameter("screenName", screenName);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						report = CreateFrom(reader);
					}
				}
			}

			return report;
		}

		private MatchGamePlayReport CreateFrom(MySqlDataReader reader)
		{
			return new MatchGamePlayReport
			{
				Id = reader.GetInt32("id"),
				GameUid = reader.GetGuid("game_uid"),
				CampaignUid = reader.GetGuid("campaign_uid"),
				ScreenName = reader.GetString("screen_name"),
				UserIdentifierType = Enum.Parse<UserIdentifierType>(reader.GetString("user_identifier_type")),
				ExternalUserId = reader.GetString("external_user_id"),
				ExternalId = reader.GetString("external_id"),
				Plays = reader.GetInt32("plays")
			};
		}
	}
}
