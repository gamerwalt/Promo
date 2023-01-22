using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Web.Mvc.Common.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
    [Collection("Sequential")]
    public class SweepCreationHandlerTests : BuilderClassAbstract, IClassFixture<MediatorHandlerFeature>
    {
        private readonly MediatorHandlerFeature _feature;

        public SweepCreationHandlerTests(MediatorHandlerFeature feature)
            => _feature = feature;

        //[Fact]
        //public async Task Given_An_Account_Without_A_Subscription_We_Should_Not_Allow_Them_To_Create_A_Sweepstake()
        //{
            //_feature.GivenACleanSlate();

            //var account = BuildNewAccount();
            //var campaign = BuildNewCampaign(account, BuildNewCampaignType());

            //await _feature.AccountRepository.CreateAsync(account);
            //await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);

            //account = await _feature.AccountRepository.GetAsync(account.AccountUid);
            //campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

            //var sweep = BuildNewSweep(account, campaign);
            //await _feature.SweepsRepository.CreateAsync(sweep, TestData.WindowsTimezone, false);
            //sweep = await _feature.SweepsRepository.GetAsync(sweep.SweepUid);

            //var prize1 = BuildNewPrize(TestData.PrizeUid1, sweep.SweepUid, TestData.PrizeName1, TestData.PrizeName1, TestData.PrizeValue1, 1);
            //await _feature.SweepsPrizesRepository.CreateSweepPrizeAsync(prize1);

            //var sweepWinner1 = BuildNewSweepWinner(sweep, TestData.SweepWinnerUid1, TestData.UserIdentifier1, 1);
            //await _feature.SweepWinnersRepository.SaveSweepWinnerAsync(sweepWinner1);
            //sweepWinner1 = await _feature.SweepWinnersRepository.GetSweepWinnerAsync(TestData.SweepWinnerUid1);

            //var request = new PromoteNextBackupWinnerRequest(sweep, prize1, sweepWinner1, "someReason");
            //await _feature.Mediator.Send(request);

            //var promotedBackupWinner = await _feature.SweepWinnersRepository.GetSweepWinnerAsync(TestData.SweepWinnerUid1);

            //promotedBackupWinner.Should().NotBeNull();
        //    promotedBackupWinner.UserIdentifier.Should().Be(sweepWinner1.UserIdentifier);
        //}
    }
}
