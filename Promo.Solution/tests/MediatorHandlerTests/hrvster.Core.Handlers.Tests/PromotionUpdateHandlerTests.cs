using FluentAssertions;
using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Promotions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Web.Mvc.Common.Steps;
using Moq;
using NToastNotify;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
    [Collection("Sequential")]
    public class PromotionUpdateHandlerTests : MediatorHandlerTestBase, IClassFixture<MediatorHandlerFeature>
    {
        private readonly MediatorHandlerFeature _feature;

        public PromotionUpdateHandlerTests(MediatorHandlerFeature feature)
            => _feature = feature;

        [Fact]
        public async Task We_ShouldNotAllow_UpdateOf_Prmotions_To_Go_Past_Our_Plan_Maximum()
        {
            _feature.GivenACleanSlate()
                .MockedToastNotification.Setup(t => t.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()));

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            await _feature.Mediator.Send(GetCreateCampaignRequest2(account, DateTime.Now.AddDays(-60), DateTime.Now.AddDays(-5)));
            var campaigns = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);
            campaigns.Count().Should().Be(1);
            var campaign = campaigns.FirstOrDefault();

            await _feature.Mediator.Send(GetCreateCampaignRequest1(account));
            
            campaigns = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);
            campaigns.Count().Should().Be(2);
            var activeCampaigns = campaigns.Where(c => c.IsActive() || c.WillBeInProgress()).ToList();
            activeCampaigns.Count().Should().Be(1);

            //Get the campaign that is not active and attempt to make it active
            campaign = campaigns.FirstOrDefault(c => !c.IsActive() && !c.WillBeInProgress());
            campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

            //Update it here
            campaign.StartDate = DateTime.Now.AddDays(1);
            campaign.EndDate = DateTime.Now.AddDays(30);

            await _feature.Mediator.Send(new UpdatePromotionRequest(
                campaign.CampaignUid,
                campaign.CampaignName,
                campaign.Description,
                campaign.CampaignType.CampaignTypeUid,
                campaign.StartDate,
                campaign.EndDate,
                TestData.WindowsTimezone,
                false));

            campaigns = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);
            activeCampaigns = campaigns.Where(c => c.IsActive() || c.WillBeInProgress()).ToList();
            activeCampaigns.Count().Should().Be(1);
        }

        [Fact]
        public async Task We_ShouldNotAllow_UpdateOf_Wizard_Prmotions_To_Go_Past_Our_Plan_Maximum()
        {
            _feature.GivenACleanSlate()
                .MockedToastNotification.Setup(t => t.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()));

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            await _feature.Mediator.Send(GetCreateCampaignRequest2(account, DateTime.Now.AddDays(-60), DateTime.Now.AddDays(-5)));
            var campaigns = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);
            campaigns.Count().Should().Be(1);
            var campaign = campaigns.FirstOrDefault();

            await _feature.Mediator.Send(GetCreateCampaignRequest1(account));

            campaigns = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);
            campaigns.Count().Should().Be(2);
            var activeCampaigns = campaigns.Where(c => c.IsActive() || c.WillBeInProgress()).ToList();
            activeCampaigns.Count().Should().Be(1);

            //Get the campaign that is not active and attempt to make it active
            campaign = campaigns.FirstOrDefault(c => !c.IsActive() && !c.WillBeInProgress());
            campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

            //Update it here
            campaign.StartDate = DateTime.Now.AddDays(1);
            campaign.EndDate = DateTime.Now.AddDays(30);

            await _feature.Mediator.Send(new UpdateCampaignViaUidRequest(campaign.CampaignUid, account.AccountUid, new Requests.ApiRequests.UpdateCampaignRequest
            {
                CampaignName = $"{campaign.CampaignName}_updated",
                ActiveInd = true,
                CampaignTypeUid = campaign.CampaignType.CampaignTypeUid,
                Description = campaign.Description,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate
            }, TestData.WindowsTimezone, false));

            campaigns = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);
            activeCampaigns = campaigns.Where(c => c.IsActive() || c.WillBeInProgress()).ToList();
            activeCampaigns.Count().Should().Be(1);
        }

        [Fact]
        public async Task We_ShouldAllow_UpdateOf_TheSameActive_WizardPrmotions_ToBeUpdated()
        {
            _feature.GivenACleanSlate()
                .MockedToastNotification.Setup(t => t.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()));

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);

            await _feature.Mediator.Send(GetCreateCampaignRequest1(account));

            var campaigns = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);
            
            var campaign = campaigns.FirstOrDefault();
            campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

            var updatedCampaignName = "This is an update";

            campaign.StartDate = DateTime.Now.AddDays(1);
            campaign.EndDate = DateTime.Now.AddDays(30);

            await _feature.Mediator.Send(new UpdateCampaignViaUidRequest(campaign.CampaignUid, account.AccountUid, new Requests.ApiRequests.UpdateCampaignRequest
            {
                CampaignName = updatedCampaignName,
                ActiveInd = true,
                CampaignTypeUid = campaign.CampaignType.CampaignTypeUid,
                Description = campaign.Description,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate
            }, TestData.WindowsTimezone, false));

            campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

            campaign.CampaignName.Should().Be(updatedCampaignName);
        }

        [Fact]
        public async Task We_ShouldAllow_UpdateOf_SameActivePrmotion_ToBeUpdated()
        {
            _feature.GivenACleanSlate()
                .MockedToastNotification.Setup(t => t.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()));

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            
            await _feature.Mediator.Send(GetCreateCampaignRequest1(account));

            var campaigns = await _feature.CampaignRepository.GetAccountCampaignsAsync(account.AccountUid);
            
            var campaign = campaigns.FirstOrDefault();
            campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

            //Update it here
            var updatedCampaignName = "This is an update";
            campaign.StartDate = DateTime.Now.AddDays(1);
            campaign.EndDate = DateTime.Now.AddDays(30);

            await _feature.Mediator.Send(new UpdatePromotionRequest(
                campaign.CampaignUid,
                updatedCampaignName,
                campaign.Description,
                campaign.CampaignType.CampaignTypeUid,
                campaign.StartDate,
                campaign.EndDate,
                TestData.WindowsTimezone,
                false));

            campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

            campaign.CampaignName.Should().Be(updatedCampaignName);
        }

    }
}
