using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure
{
	public interface ICampaignRepository
	{
		//Creates
		Task CreateCampaignAsync(Campaign campaign, string timezone, bool isDayLight);
		
		//Updates
		Task UpdateCampaignAsync(Guid campaignUid, string campaignName, string description, Guid campaignTypeUid, DateTime startDate, DateTime endDate, string timezoneId, bool isDaylight);
		Task UpdateCampaignAsync(Guid campaignUid, Campaign campaign, string timezoneId, bool isDaylight);

		//Get Asyncs
		Task<Campaign> GetCampaignByUidAsync(Guid campaignUid, Guid accountUid);
		Task<Campaign> GetCampaignByUidAsync(Guid campaignUid);
		Task<List<Campaign>> GetAccountCampaignsAsync(Guid accountUid);

		//Gets
		List<Campaign> GetAccountCampaigns(Guid userUid);
		Campaign GetCampaignByUid(Guid campaignUid, Guid userUid);
	}

	public class CampaignRepository : ICampaignRepository
	{
		private readonly IDatabase _database;

		public CampaignRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateCampaignAsync(Campaign campaign, string timezone, bool isDayLight)
		{
			var startDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(campaign.StartDate, TimeZoneInfo.Local.Id, timezone);
			var endDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(campaign.EndDate, TimeZoneInfo.Local.Id, timezone);

			if(!isDayLight)
			{
				startDateUtc = startDateUtc.AddHours(-2);
				endDateUtc = endDateUtc.AddHours(-2);
			}

			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "campaign_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("campaignUid", campaign.CampaignUid)
				.AddParameter("campaignName", campaign.CampaignName)
				.AddParameter("campaignDescription", campaign.Description)
				.AddParameter("startDate", campaign.StartDate)
				.AddParameter("startDateUtc", startDateUtc)
				.AddParameter("endDate", campaign.EndDate)
				.AddParameter("endDateUtc", endDateUtc)
				.AddParameter("campaignTypeUid", campaign.CampaignType.CampaignTypeUid)
				.AddParameter("accountUid", campaign.Account.AccountUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public List<Campaign> GetAccountCampaigns(Guid accountUid)
		{
			var campaigns = new List<Campaign>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "campaigns_get_by_account_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", accountUid);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					campaigns.Add(CreateCampaignFrom(reader));
				}
			}

			return campaigns;
		}

		public async Task<List<Campaign>> GetAccountCampaignsAsync(Guid accountUid)
		{
			var campaigns = new List<Campaign>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "campaigns_get_by_account_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", accountUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					campaigns.Add(CreateCampaignFrom(reader));
				}
			}

			return campaigns;
		}

		public Campaign GetCampaignByUid(Guid campaignUid, Guid accountUid)
		{
			var campaign = default(Campaign);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "campaign_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(campaignUid), campaignUid);
				command.AddParameter(nameof(accountUid), accountUid);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					campaign = CreateCampaignFrom(reader);
				}
			}

			return campaign;
		}

		public async Task<Campaign> GetCampaignByUidAsync(Guid campaignUid, Guid accountUid)
		{
			var campaign = default(Campaign);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "campaign_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(campaignUid), campaignUid);
				command.AddParameter(nameof(accountUid), accountUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					campaign = CreateCampaignFrom(reader);
				}
			}

			return campaign;
		}

		public async Task<Campaign> GetCampaignByUidAsync(Guid campaignUid)
		{
			var campaign = default(Campaign);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "campaign_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(campaignUid), campaignUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					campaign = CreateCampaignFrom(reader);
				}
			}

			return campaign;
		}

		public async Task UpdateCampaignAsync(Guid campaignUid, Campaign campaign, string timezoneId, bool isDaylight)
		{
			var startDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(campaign.StartDate, TimeZoneInfo.Local.Id, timezoneId);
			var endDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(campaign.EndDate, TimeZoneInfo.Local.Id, timezoneId);

			if (!isDaylight)
			{
				startDateUtc = startDateUtc.AddHours(-2);
				endDateUtc = endDateUtc.AddHours(-2);
			}

			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "campaign_update_via_uid";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("campaignUid", campaignUid)
				.AddParameter("campaignName", campaign.CampaignName)
				.AddParameter("campaignDescription", campaign.Description)
				.AddParameter("startDate", campaign.StartDate)
				.AddParameter("startDateUtc", startDateUtc)
				.AddParameter("endDate", campaign.EndDate)
				.AddParameter("endDateUtc", endDateUtc)
				.AddParameter("activeInd", campaign.ActiveInd)
				.AddParameter("campaignTypeUid", campaign.CampaignType?.CampaignTypeUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task UpdateCampaignAsync(Guid campaignUid, string campaignName, string description, Guid campaignTypeUid, DateTime startDate, DateTime endDate, string timezoneId, bool isDaylight)
		{
			var startDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(startDate, TimeZoneInfo.Local.Id, timezoneId);
			var endDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(endDate, TimeZoneInfo.Local.Id, timezoneId);

			if (!isDaylight)
			{
				startDateUtc = startDateUtc.AddHours(-2);
				endDateUtc = endDateUtc.AddHours(-2);
			}

			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "campaign_update_via_uid";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("campaignUid", campaignUid)
				.AddParameter("campaignName", campaignName)
				.AddParameter("campaignDescription", description)
				.AddParameter("startDate", startDate)
				.AddParameter("startDateUtc", startDateUtc)
				.AddParameter("endDate", endDate)
				.AddParameter("endDateUtc", endDateUtc)
				.AddParameter("activeInd", true)
				.AddParameter("campaignTypeUid", campaignTypeUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		private Campaign CreateCampaignFrom(MySqlDataReader reader)
		{
			return new Campaign
			{
				CampaignId = reader.GetInt32("campaign_id"),
				CampaignUid = reader.GetGuid("campaign_uid"),
				CampaignName = reader.GetString("campaign_name"),
				Description = reader.GetStringValue("campaign_description"),
				StartDate = reader.GetDateTime("start_date"),
				StartDateUtc = reader.GetDateTime("start_date_utc"),
				EndDate = reader.GetDateTime("end_date"),
				EndDateUtc = reader.GetDateTime("end_date_utc"),
				CreatedDate = reader.GetDateTime("created_date"),
				ActiveInd = reader.GetBoolean("active_ind"),
				Account = GetAccount(reader),
				CampaignType = new CampaignType
				{
					CampaignTypeId = reader.GetInt32("campaign_type_id"),
					CampaignTypeUid = reader.GetGuid("campaign_type_uid"),
					CampaignTypeCode = (CampaignTypeCode)Enum.Parse(typeof(CampaignTypeCode), reader.GetString("campaign_type_code")),
					CampaignTypeName = reader.GetString("campaign_type_name"),
					CamapaignTypeDescription = reader.GetStringValue("campaign_type_description"),
					IsActive = reader.GetBoolean("campaign_type_active_ind")
				}
			};
		}

		private Account GetAccount(MySqlDataReader reader)
		{
			return new Account
			{
				AccountId = reader.GetInt32("account_id"),
				AccountUid = reader.GetGuid("account_uid"),
				EmailAddress = reader.GetString("email_address"),
				PhoneNumber = reader.GetString("phone_number"),
				DisplayName = reader.GetString("display_name"),
				TermsAccepted = reader.GetBoolean("terms_accepted")
			};
		}
	}
}
