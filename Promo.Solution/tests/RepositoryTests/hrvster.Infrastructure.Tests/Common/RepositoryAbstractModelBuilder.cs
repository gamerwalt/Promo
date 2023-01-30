using hrvster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities;
using TestUtilities.Data;

namespace hrvster.Infrastructure.Tests.Common
{
    public abstract class RepositoryAbstractModelBuilder : BuilderClassAbstract
    {
        public async Task<Models.Account> BuildAccountAndSubscribeToBasicPlan(
            RepositoryFeature _feature)
        {
            var account = await CreateAccountAsync(_feature);

            var plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.BasicPlanUid, default);

            await _feature.SubscriptionService.SubscribeUserToAsync(plan, account.AccountUid);
            return account;
        }

        public async Task<Models.Campaign> CreateCampaignAsync(
            RepositoryFeature _feature, Models.Account account, Models.CampaignType campaignType)
        {
            var campaign = BuildNewCampaign(account, campaignType);

            await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);

            return await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid, account.AccountUid);
        }

        public async Task<Models.Campaign> CreateCampaignAsync2(
            RepositoryFeature _feature, Models.Account account, Models.CampaignType campaignType)
        {
            var campaign = BuildNewCampaign2(account, campaignType);

            await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);
            
            return await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid, account.AccountUid);
        }

        public async Task<Campaign> BuildAndCreateCampaignAsync(RepositoryFeature _feature, Models.Account account)
        {
            var campaignType = BuildNewCampaignType();
            var campaign = await CreateCampaignAsync(_feature, account, campaignType);
            return campaign;
        }

        public async Task<Campaign> BuildAndCreateCampaignAsync2(RepositoryFeature _feature, Models.Account account)
        {
            var campaignType = BuildNewCampaignType();
            var campaign = await CreateCampaignAsync2(_feature, account, campaignType);
            return campaign;
        }

        public async Task<Models.Account> CreateAccountAsync(RepositoryFeature _feature)
        {
            var account = BuildNewAccount();
            await _feature.AccountsRepository.CreateAsync(account);

            return await _feature.AccountsRepository.GetByEmailAddressAsync(account.EmailAddress);
        }
    }
}
