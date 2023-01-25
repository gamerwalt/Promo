using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests;
using hrvster.Core.Requests.ApiRequests;
using hrvster.Core.Requests.WebRequests.Promotions;
using hrvster.Models;
using TestUtilities;
using TestUtilities.Data;

namespace hrvster.Core.Handlers.Tests
{
    public abstract class MediatorHandlerTestBase : BuilderClassAbstract
    {
        public static CreateCampaignRequest GetCreateCampaignRequest2(
            Models.Account account,
            DateTime startDate,
            DateTime endDate)
            => new CreateCampaignRequest(GetCampaignRequest2(startDate, endDate), account.AccountUid, TestData.WindowsTimezone);

        public static CreateCampaignRequest GetCreateCampaignRequest2(
            Models.Account account)
            => new CreateCampaignRequest(GetCampaignRequest2(), account.AccountUid, TestData.WindowsTimezone);

        public static CreateCampaignRequest GetCreateCampaignRequest1(Models.Account account)
            => new CreateCampaignRequest(GetCampaignRequest(), account.AccountUid, TestData.WindowsTimezone);

        public static CreatePromotionRequest GetCreatePromotionRequest2(Models.Account account, Guid campaignUid)
            => new CreatePromotionRequest(
                campaignUid,
                TestData.CampaignName2,
                TestData.CampaignDescription2,
                TestData.CampaignTypeUid,
                DateTime.Now.AddDays(3),
                DateTime.Now.AddDays(60),
                account.AccountUid,
                TestData.WindowsTimezone,
                false);

        public static CreatePromotionRequest GetCreatePromotionRequest(Models.Account account, Guid campaignUid)
            => new CreatePromotionRequest(
                campaignUid,
                TestData.CampaignName,
                TestData.CampaignDescription,
                TestData.CampaignTypeUid,
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(30),
                account.AccountUid,
                TestData.WindowsTimezone,
                false);

        public static NewCampaignRequest GetCampaignRequest2(DateTime startDate, DateTime endDate)
            => new NewCampaignRequest
            {
                Name = $"{TestData.CampaignName}2",
                Description = TestData.CampaignDescription,
                CampaignTypeUid = TestData.CampaignTypeUid,
                StartDate = startDate,
                EndDate = endDate
            };

        public static NewCampaignRequest GetCampaignRequest2()
            => new NewCampaignRequest
            {
                Name = $"{TestData.CampaignName}2",
                Description = TestData.CampaignDescription,
                CampaignTypeUid = TestData.CampaignTypeUid,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30)
            };

        public static NewCampaignRequest GetCampaignRequest()
            => new NewCampaignRequest
            {
                Name = TestData.CampaignName,
                Description = TestData.CampaignDescription,
                CampaignTypeUid = TestData.CampaignTypeUid,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(30)
            };

        public async Task<Models.Account> BuildAccountAndSubscribeToBasicPlan(MediatorHandlerFeature _feature)
        {
            var account = await CreateAccountAsync(_feature);

            var plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.BasicPlanUid, default);

            await _feature.SubscriptionService.SubscribeUserToAsync(plan, account.AccountUid);
            return account;
        }

        public async Task<Models.Account> BuildAccountAndSubscribeToStarterPlan(MediatorHandlerFeature _feature)
        {
            var account = await CreateAccountAsync(_feature);

            var plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.StarterPlanUid, default);

            await _feature.SubscriptionService.SubscribeUserToAsync(plan, account.AccountUid);
            return account;
        }

        public async Task<Models.Account> CreateAccountAsync(MediatorHandlerFeature _feature)
        {
            var account = BuildNewAccount();
            await _feature.AccountRepository.CreateAsync(account);

            return await _feature.AccountRepository.GetByEmailAddressAsync(account.EmailAddress);
        }

        public async Task<Models.Campaign> CreateCampaignAsync(
            MediatorHandlerFeature _feature, Models.Account account, Models.CampaignType campaignType)
        {
            var campaign = BuildNewCampaign(account, campaignType);

            await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);

            var campaigns = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);

            return campaigns.FirstOrDefault();
        }
    }
}
