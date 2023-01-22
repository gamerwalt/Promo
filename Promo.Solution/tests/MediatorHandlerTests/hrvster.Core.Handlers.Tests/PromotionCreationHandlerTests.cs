using FluentAssertions;
using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests;
using hrvster.Core.Requests.ApiRequests;
using hrvster.Models.Subscriptions;
using hrvster.Web.Mvc.Common.Steps;
using Moq;
using NToastNotify;
using TestUtilities;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
    [Collection("Sequential")]
    public class PromotionCreationHandlerTests : BuilderClassAbstract, IClassFixture<MediatorHandlerFeature>
    {
        private readonly MediatorHandlerFeature _feature;

        public PromotionCreationHandlerTests(MediatorHandlerFeature feature)
            => _feature = feature;

        [Fact]
        public async Task Given_An_Account_Without_A_Subscription_We_Should_Not_Allow_Them_To_Create_A_Promotion()
        {
            _feature.GivenACleanSlate();

            _feature.MockedToastNotification.Setup(t => t.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()));
            
            var account = BuildNewAccount();
            await _feature.AccountRepository.CreateAsync(account);

            account = await _feature.AccountRepository.GetByEmailAddressAsync(account.EmailAddress);

            var newCampaignRequest = new NewCampaignRequest
            {
                Name = TestData.CampaignName,
                Description = TestData.CampaignDescription,
                CampaignTypeUid = TestData.CampaignTypeUid,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(30)
            };

            var request = new CreateCampaignRequest(newCampaignRequest, account.AccountUid, TestData.WindowsTimezone);
            await _feature.Mediator.Send(request);

            var campaign = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);

            campaign.Count().Should().Be(0);
        }

        [Fact]
        public async Task If_Plan_Has_0_Max_Promotions_We_Should_Allow_Creation_Of_A_Promotion_If_Account_Is_Subscribed()
        {
            _feature.GivenACleanSlate();

            _feature.MockedToastNotification.Setup(t => t.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()));

            var account = BuildNewAccount();
            await _feature.AccountRepository.CreateAsync(account);

            account = await _feature.AccountRepository.GetByEmailAddressAsync(account.EmailAddress);
            var plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.BasicPlanUid, default);

            await _feature.SubscriptionService.SubscribeUserToAsync(plan, account.AccountUid);

            var newCampaignRequest = new NewCampaignRequest
            {
                Name = TestData.CampaignName,
                Description = TestData.CampaignDescription,
                CampaignTypeUid = TestData.CampaignTypeUid,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(30)
            };

            var request = new CreateCampaignRequest(newCampaignRequest, account.AccountUid, TestData.WindowsTimezone);
            await _feature.Mediator.Send(request);

            var campaign = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);

            campaign.Count().Should().Be(1);
        }
    }
}
