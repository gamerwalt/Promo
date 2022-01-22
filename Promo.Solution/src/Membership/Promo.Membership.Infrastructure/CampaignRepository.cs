using MySqlConnector;
using Promo.Membership.Infrastructure.Models;
using Promo.Core;
using Promo.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Promo.Membership.Infrastructure
{
    public interface ICampaignRepository
    {
        Task CreateCampaignAsync(Models.Campaign campaign);
        Task<Models.Campaign> GetCampaignByUidAsync(Guid campaignUid);
        Task UpdateCampaignAsync(Guid campaignUid, Models.Campaign campaign);
        Task<List<Models.Campaign>> GetAccountCampaignsAsync(Guid accountUid);
    }

    public class CampaignRepository : ICampaignRepository
    {
        private readonly IDatabase _database;
        public CampaignRepository(IDatabase database)
        {
            _database = database;
        }

        public async Task CreateCampaignAsync(Models.Campaign campaign)
        {
            using var conn = new MySqlConnection(_database.GetConnectionString());
            var command = conn.CreateCommand();
            command.CommandText = "campaign_create";
            command.CommandType = CommandType.StoredProcedure;
            command
                .AddParameter("campaignUid", campaign.CampaignUid)
                .AddParameter("campaignName", campaign.CampaignName)
                .AddParameter("campaignDescription", campaign.Description)
                .AddParameter("startDate", campaign.StartDate)
                .AddParameter("endDate", campaign.EndDate)
                .AddParameter("campaignTypeUid", campaign.CampaignType.CampaignTypeUid)
                .AddParameter("accountUid", campaign.AccountUid);

            await conn.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Models.Campaign>> GetAccountCampaignsAsync(Guid accountUid)
        {
            var campaigns = new List<Models.Campaign>();

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

        public async Task<Models.Campaign> GetCampaignByUidAsync(Guid campaignUid)
        {
            var campaign = default(Models.Campaign);

            using (var conn = new MySqlConnection(_database.GetConnectionString()))
            {
                var command = conn.CreateCommand();
                command.CommandText = "campaign_get_by_uid";
                command.CommandType = CommandType.StoredProcedure;
                command.AddParameter("campaignUid", campaignUid);

                await conn.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    campaign = CreateCampaignFrom(reader);
                }
            }

            return campaign;
        }

        public async Task UpdateCampaignAsync(Guid campaignUid, Models.Campaign campaign)
        {
            using var conn = new MySqlConnection(_database.GetConnectionString());
            var command = conn.CreateCommand();
            command.CommandText = "campaign_update_via_uid";
            command.CommandType = CommandType.StoredProcedure;
            command
                .AddParameter("campaignUid", campaignUid)
                .AddParameter("campaignName", campaign.CampaignName)
                .AddParameter("campaignDescription", campaign.Description)
                .AddParameter("startDate", campaign.StartDate)
                .AddParameter("endDate", campaign.EndDate)
                .AddParameter("activeInd", campaign.ActiveInd)
                .AddParameter("campaignTypeUid", campaign.CampaignType?.CampaignTypeUid);

            await conn.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        private Models.Campaign CreateCampaignFrom(MySqlDataReader reader)
        {
            return new Models.Campaign
            {
                CampaignId = reader.GetInt32("campaign_id"),
                CampaignUid = reader.GetGuid("campaign_uid"),
                CampaignName = reader.GetString("campaign_name"),
                Description = reader.GetString("campaign_description"),
                StartDate = reader.GetDateTime("start_date"),
                EndDate = reader.GetDateTime("end_date"),
                CreatedDate = reader.GetDateTime("created_date"),
                ActiveInd = reader.GetBoolean("active_ind"),
                AccountUid = reader.GetGuid("account_uid"),
                CampaignType = new CampaignType
                {
                    CampaignTypeId = reader.GetInt32("campaign_type_id"),
                    CampaignTypeUid = reader.GetGuid("campaign_type_uid"),
                    CampaignTypeCode = (CampaignTypeCode)Enum.Parse(typeof(CampaignTypeCode), reader.GetString("campaign_type_code")),
                    CampaignTypeName = reader.GetString("campaign_type_name"),
                    IsActive = reader.GetBoolean("campaign_type_active_ind")
                }
            };
        }
    }
}
