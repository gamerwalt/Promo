using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests;
using hrvster.Core.Requests.ApiRequests;
using hrvster.Core.Requests.Notifications.Publishing;
using hrvster.Core.Requests.WebRequests.Promotions;
using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Models;
using hrvster.Models.Profiles;
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

        public static CreateSweepRequest GetCreateSweepRequest2(Models.Account account, Campaign campaign)
            => new CreateSweepRequest(
                    $"{TestData.SweepName}2",
                    DateTime.Now.AddHours(2),
                    DateTime.Now.AddDays(20),
                    DateTime.Now.AddDays(21),
                    campaign.CampaignUid,
                    account.AccountUid,
                    TestData.WindowsTimezone,
                    false);

        public static CreateSweepRequest GetCreateSweepRequest(Models.Account account, Campaign campaign)
            => new CreateSweepRequest(
                    TestData.SweepName,
                    DateTime.Now.AddHours(1),
                    DateTime.Now.AddDays(30),
                    DateTime.Now.AddDays(31),
                    campaign.CampaignUid,
                    account.AccountUid,
                    TestData.WindowsTimezone,
                    false);

        public static NewSweepRequest CreateNewSweepRequest(Models.Account account, Campaign campaign)
            => new NewSweepRequest
                {
                    AccountUid = account.AccountUid,
                    CampaignUid = campaign.CampaignUid,
                    Name = TestData.SweepName,
                    StartDate = DateTime.Now.AddHours(2),
                    EndDate = DateTime.Now.AddDays(60),
                    DrawDate = DateTime.Now.AddDays(61)
                };

        public static NewSweepRequest CreateNewSweepRequest2(Models.Account account, Campaign campaign)
            => new NewSweepRequest
            {
                AccountUid = account.AccountUid,
                CampaignUid = campaign.CampaignUid,
                Name = $"{TestData.SweepName}2",
                StartDate = DateTime.Now.AddHours(4),
                EndDate = DateTime.Now.AddDays(30),
                DrawDate = DateTime.Now.AddDays(31)
            };

        public async Task<Campaign> BuildAndCreateCampaignAsync(MediatorHandlerFeature _feature, Models.Account account)
        {
            var campaignType = BuildNewCampaignType();
            var campaign = await CreateCampaignAsync(_feature, account, campaignType);
            return campaign;
        }

        public async Task<Sweep> BuildAndCreateSweepAsync(
            MediatorHandlerFeature _feature, Models.Account account, Campaign campaign)
            => await _feature.Mediator.Send(
                new CreateSweep(
                    CreateNewSweepRequest(account, campaign),
                    TestData.WindowsTimezone, false));

        public static SweepRuleTweetPublished CreateSweepRuleTweetPublished(Sweep sweep, Profile profile)
            => new SweepRuleTweetPublished(TestData.TweetId, new Clients.Common.Message
            {
                WatchTypeId = TestData.WatchTypeId,
                Profile = profile,
                Hashtag = TestData.Hashtag,
                SweepUid = sweep.SweepUid,
                SweepName = sweep.Name,
                Text = TestData.TweetText,
                IsScheduled = false
            }, true);

        public static SweepRuleTweetPublished CreateSweepRuleTweetPublished2(Sweep sweep, Profile profile)
            => new SweepRuleTweetPublished(TestData.TweetId, new Clients.Common.Message
            {
                WatchTypeId = TestData.WatchTypeId,
                Profile = profile,
                Hashtag = TestData.Hashtag2,
                SweepUid = sweep.SweepUid,
                SweepName = $"{sweep.Name}_2",
                Text = TestData.TweetText2,
                IsScheduled = false
            }, true);

        public static SweepRuleTweetPublished CreateSweepRuleTweetPublished3(Sweep sweep, Profile profile)
            => new SweepRuleTweetPublished(TestData.TweetId, new Clients.Common.Message
            {
                WatchTypeId = TestData.WatchTypeId,
                Profile = profile,
                Hashtag = TestData.Hashtag3,
                SweepUid = sweep.SweepUid,
                SweepName = $"{sweep.Name}_3",
                Text = TestData.TweetText3,
                IsScheduled = false
            }, true);

        public static PublishSweepRuleTweetRequest GetPublishSweepRuleTweetRequest(Profile profile, Sweep sweep, bool postNow = true)
            => new PublishSweepRuleTweetRequest(
                sweep,
                profile,
                TestData.WatchTypeId,
                TestData.WatchValue,
                TestData.TweetText,
                postNow,
                DateTime.Now.AddHours(1),
                true,
                null,
                TestData.WindowsTimezone,
                false);

        public static PublishSweepRuleTweetRequest GetPublishSweepRuleTweetRequest2(Profile profile, Sweep sweep, bool postNow = true)
            => new PublishSweepRuleTweetRequest(
                sweep,
                profile,
                TestData.WatchTypeId,
                TestData.WatchValue2,
                TestData.TweetText2,
                postNow,
                DateTime.Now.AddHours(2),
                true,
                null,
                TestData.WindowsTimezone,
                false);
        public static PublishSweepRuleTweetRequest GetPublishSweepRuleTweetRequest3(Profile profile, Sweep sweep, bool postNow = true)
            => new PublishSweepRuleTweetRequest(
                sweep,
                profile,
                TestData.WatchTypeId,
                TestData.WatchValue3,
                TestData.TweetText3,
                postNow,
                DateTime.Now.AddHours(2),
                true,
                null,
                TestData.WindowsTimezone,
                false);

        public async Task<Profile> BuildAndCreateProfileAsync(MediatorHandlerFeature feature, Models.Account account)
        {
            var profileBuilt = BuildNewTwitterProfile(account);

            await feature.ProfileRespository.UpsertProfileAsync(profileBuilt);

            return await feature.ProfileRespository.GetProfileAsync(account.AccountUid, ProfileType.Twitter);
        }
    }
}
