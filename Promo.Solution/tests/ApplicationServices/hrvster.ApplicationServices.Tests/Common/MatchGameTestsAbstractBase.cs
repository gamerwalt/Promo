using hrvster.Models;
using TestUtilities;
using TestUtilities.Data;

namespace hrvster.ApplicationServices.Tests.Common
{
    public abstract class MatchGameTestsAbstractBase : BuilderClassAbstract
    {
        public async Task<Account> BuildAccountAndSubscribeToBasicPlan(
            MatchGameFeature _feature)
        {
            var account = await CreateAccountAsync(_feature);

            var plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.BasicPlanUid, default);

            await _feature.SubscriptionService.SubscribeUserToAsync(plan, account.AccountUid);
            return account;
        }

        public async Task<Campaign> CreateCampaignAsync(
            MatchGameFeature _feature, Account account, CampaignType campaignType)
        {
            var campaign = BuildNewCampaign(account, campaignType);

            await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);

            var campaigns = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);

            return campaigns.FirstOrDefault();
        }

        public async Task<Campaign> BuildAndCreateCampaignAsync(MatchGameFeature _feature, Account account)
        {
            var campaignType = BuildNewCampaignType();
            var campaign = await CreateCampaignAsync(_feature, account, campaignType);
            return campaign;
        }

        public async Task<Account> CreateAccountAsync(MatchGameFeature _feature)
        {
            var account = BuildNewAccount();
            await _feature.AccountsRepository.CreateAsync(account);

            return await _feature.AccountsRepository.GetByEmailAddressAsync(account.EmailAddress);
        }
    }
}
