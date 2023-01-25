using FluentAssertions;
using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests;
using hrvster.Core.Requests.ApiRequests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Models;
using hrvster.Web.Mvc.Common.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
    public class SweepstakesCreationHandlerTests : MediatorHandlerTestBase, IClassFixture<MediatorHandlerFeature>
    {
        private readonly MediatorHandlerFeature _feature;

        public SweepstakesCreationHandlerTests(MediatorHandlerFeature feature)
            => _feature = feature;

        [Fact]
        public async Task An_Account_WithoutASubscription_ShouldNotBeAllowed_To_Create_A_Sweepstake()
        {
            _feature.GivenACleanSlate();

            ////var account = await BuildAccountAndSubscribeToStarterPlan(_feature);
            var account = await CreateAccountAsync(_feature);
            var campaign = await CreateCampaignAsync(account);

            var request = new NewSweepRequest
            {
                AccountUid = account.AccountUid,
                CampaignUid = campaign.CampaignUid,
                Name = TestData.SweepName,
                StartDate = DateTime.Now.AddHours(2),
                EndDate = DateTime.Now.AddDays(60),
                DrawDate = DateTime.Now.AddDays(61)
            };

            await _feature.Mediator.Send(new CreateSweep(request, TestData.WindowsTimezone, false));

            var sweeps = await _feature.SweepsRepository.GetAllForAccountAsync(account.AccountUid);
            sweeps.Count().Should().Be(0);
        }

        [Fact]
        public async Task An_Account_WithoutASubscription_ShouldNotBeAllowed_To_Create_A_Sweepstake_2()
        {
            _feature.GivenACleanSlate();

            ////var account = await BuildAccountAndSubscribeToStarterPlan(_feature);
            var account = await CreateAccountAsync(_feature);
            var campaign = await CreateCampaignAsync(account);

            var request = new CreateSweepRequest(
                TestData.SweepName, 
                DateTime.Now.AddHours(1), 
                DateTime.Now.AddDays(30), 
                DateTime.Now.AddDays(31), 
                campaign.CampaignUid, 
                account.AccountUid, 
                TestData.WindowsTimezone, 
                false);

            await _feature.Mediator.Send(request);

            var sweeps = await _feature.SweepsRepository.GetAllForAccountAsync(account.AccountUid);
            sweeps.Count().Should().Be(0);
        }

        private async Task<Campaign> CreateCampaignAsync(Models.Account account)
        {
            var campaignType = BuildNewCampaignType();
            var campaign = await CreateCampaignAsync(_feature, account, campaignType);
            return campaign;
        }
    }
}
