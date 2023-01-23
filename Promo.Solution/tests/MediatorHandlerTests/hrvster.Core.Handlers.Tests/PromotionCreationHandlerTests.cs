using FluentAssertions;
using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests;
using hrvster.Web.Mvc.Common.Steps;
using Moq;
using NToastNotify;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
    [Collection("Sequential")]
    public class PromotionCreationHandlerTests : MediatorHandlerTestBase, IClassFixture<MediatorHandlerFeature>
    {
        private readonly MediatorHandlerFeature _feature;

        public PromotionCreationHandlerTests(MediatorHandlerFeature feature)
            => _feature = feature;

        [Fact]
        public async Task An_Account_WithoutASubscription_ShouldNotBeAllowed_To_Create_A_Wizard_Promotion()
        {
            _feature.GivenACleanSlate()
                .MockedToastNotification.Setup(t => t.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()));

            var account = await CreateAccountAsync(_feature);

            await _feature.Mediator.Send(GetCreatePromotionRequest(account, TestData.CampaignUid));

            var campaigns = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);

            campaigns.Count().Should().Be(0);
        }

        [Fact]
        public async Task An_Account_WithoutASubscription_ShouldNotBeAllowed_To_Create_A_Promotion()
        {
            _feature.GivenACleanSlate()
                .MockedToastNotification.Setup(t => t.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()));

            var account = await CreateAccountAsync(_feature);

            await _feature.Mediator.Send(GetCreateCampaignRequest1(account));

            var campaign = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);

            campaign.Count().Should().Be(0);
        }

        [Fact]
        public async Task If_Plan_Has_1_Max_Promotions_We_ShouldAllow_CreationOf_1_Promotion()
        {
            _feature.GivenACleanSlate();

            _feature.MockedToastNotification.Setup(t => t.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()));

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);

            await _feature.Mediator.Send(GetCreateCampaignRequest1(account));

            await _feature.Mediator.Send(GetCreateCampaignRequest2(account));

            var campaign = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);

            campaign.Count().Should().Be(1);
        }

        [Fact]
        public async Task If_Plan_Has_1_Max_Promotions_We_ShouldAllow_CreationOf_1_WizardPromotion()
        {
            _feature.GivenACleanSlate();

            _feature.MockedToastNotification.Setup(t => t.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()));

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);

            await _feature.Mediator.Send(GetCreatePromotionRequest(account, TestData.CampaignUid));
            await _feature.Mediator.Send(GetCreatePromotionRequest2(account, TestData.CampaignUid2));

            var campaign = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);

            campaign.Count().Should().Be(1);
            campaign.FirstOrDefault()?.CampaignUid.Should().Be(TestData.CampaignUid);
        }

        [Fact]
        public async Task If_Plan_Has_Infinity_Promotions_We_ShouldAllow_Creation_Of_2_Promotions()
        {
            _feature.GivenACleanSlate();

            _feature.MockedToastNotification.Setup(t => t.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()));

            var account = await BuildAccountAndSubscribeToStarterPlan(_feature);
            await _feature.Mediator.Send(
                new CreateCampaignRequest(GetCampaignRequest(), account.AccountUid, TestData.WindowsTimezone));
            await _feature.Mediator.Send(new CreateCampaignRequest(GetCampaignRequest2(), account.AccountUid, TestData.WindowsTimezone));

            var campaign = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);

            campaign.Count().Should().Be(2);
        }

        [Fact]
        public async Task If_Plan_Has_Infinity_Promotions_We_ShouldAllow_Creation_Of_2_WizardPromotions()
        {
            _feature.GivenACleanSlate();

            _feature.MockedToastNotification.Setup(t => t.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()));

            var account = await BuildAccountAndSubscribeToStarterPlan(_feature);

            await _feature.Mediator.Send(GetCreatePromotionRequest(account, TestData.CampaignUid));
            await _feature.Mediator.Send(GetCreatePromotionRequest2(account, TestData.CampaignUid2));

            var campaign = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);

            campaign.Count().Should().Be(2);
            campaign[0].CampaignUid.Should().Be(TestData.CampaignUid);
            campaign[1].CampaignUid.Should().Be(TestData.CampaignUid2);
        }
    }
}
