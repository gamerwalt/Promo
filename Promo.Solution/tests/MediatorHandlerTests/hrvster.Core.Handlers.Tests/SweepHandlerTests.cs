using FluentAssertions;
using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Models;
using hrvster.Web.Models;
using hrvster.Web.Mvc.Common.Steps;
using TestUtilities;
using TestUtilities.Common.Steps;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
	[Collection("Sequential")]
	public class SweepHandlerTests : BuilderClassAbstract, IClassFixture<MediatorHandlerFeature>
	{
		private readonly MediatorHandlerFeature _feature;

		public SweepHandlerTests(MediatorHandlerFeature feature)
			=> _feature = feature;

		[Fact]
		public async Task When_We_Have_No_Backup_Winners_To_Promote_We_DoNot_Promote_Anyone()
		{
			_feature.GivenACleanSlate();

            var account = BuildNewAccount();
            var campaign = BuildNewCampaign(account, BuildNewCampaignType());

            await _feature.AccountRepository.CreateAsync(account);
            await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);

            account = await _feature.AccountRepository.GetAsync(account.AccountUid);
            campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

            var sweep = BuildNewSweep(account, campaign);
            await _feature.SweepsRepository.CreateAsync(sweep, TestData.WindowsTimezone, false);
			sweep = await _feature.SweepsRepository.GetAsync(sweep.SweepUid);

			var prize1 = BuildNewPrize(TestData.PrizeUid1, sweep.SweepUid, TestData.PrizeName1, TestData.PrizeName1, TestData.PrizeValue1, 1);
			await _feature.SweepsPrizesRepository.CreateSweepPrizeAsync(prize1);

			var sweepWinner1 = BuildNewSweepWinner(sweep, TestData.SweepWinnerUid1, TestData.UserIdentifier1, 1);
			await _feature.SweepWinnersRepository.SaveSweepWinnerAsync(sweepWinner1);
			sweepWinner1 = await _feature.SweepWinnersRepository.GetSweepWinnerAsync(TestData.SweepWinnerUid1);

			var request = new PromoteNextBackupWinnerRequest(sweep, prize1, sweepWinner1, "someReason");
			await _feature.Mediator.Send(request);

			var promotedBackupWinner = await _feature.SweepWinnersRepository.GetSweepWinnerAsync(TestData.SweepWinnerUid1);

			promotedBackupWinner.Should().NotBeNull();
			promotedBackupWinner.UserIdentifier.Should().Be(sweepWinner1.UserIdentifier);
		}

		[Fact]
		public async Task When_We_Have_1_Backup_Winner_To_Promote_We_Promote_That_BackupWinner_To_The_Winner()
		{
			_feature.GivenACleanSlate();

			var account = BuildNewAccount();
			var campaign = BuildNewCampaign(account, BuildNewCampaignType());

			await _feature.AccountRepository.CreateAsync(account);
			await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);

			account = await _feature.AccountRepository.GetAsync(account.AccountUid);
			campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

			var sweep = BuildNewSweep(account, campaign);
			await _feature.SweepsRepository.CreateAsync(sweep, TestData.WindowsTimezone, false);
			sweep = await _feature.SweepsRepository.GetAsync(sweep.SweepUid);

			var prize1 = BuildNewPrize(TestData.PrizeUid1, sweep.SweepUid, TestData.PrizeName1, TestData.PrizeName1, TestData.PrizeValue1, 1);
			await _feature.SweepsPrizesRepository.CreateSweepPrizeAsync(prize1);

			var sweepWinner1 = BuildNewSweepWinner(sweep, TestData.SweepWinnerUid1, TestData.UserIdentifier1, 1);
			await _feature.SweepWinnersRepository.SaveSweepWinnerAsync(sweepWinner1);
			sweepWinner1 = await _feature.SweepWinnersRepository.GetSweepWinnerAsync(TestData.SweepWinnerUid1);

			var backupWinner1 = BuildNewSweepWinner(sweep, sweepWinner1.SweepWinnerUid, TestData.UserIdentifier2, 1);
			await _feature.SweepWinnersRepository.SaveSweepBackupWinnerAsync(backupWinner1);

			var request = new PromoteNextBackupWinnerRequest(sweep, prize1, sweepWinner1, "someReason");
			await _feature.Mediator.Send(request);

			var promotedBackupWinner = await _feature.SweepWinnersRepository.GetSweepWinnerAsync(TestData.SweepWinnerUid1);

			promotedBackupWinner.Should().NotBeNull();
			promotedBackupWinner.UserIdentifier.Should().Be(backupWinner1.UserIdentifier);
		}

		[Fact]
		public async Task When_We_Have_2_Backup_Winner_To_Promote_We_Promote_Each_Backup_Winner_1_At_A_Time()
		{
            _feature.GivenACleanSlate();

            var account = BuildNewAccount();
            var campaign = BuildNewCampaign(account, BuildNewCampaignType());

            await _feature.AccountRepository.CreateAsync(account);
            await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);

            account = await _feature.AccountRepository.GetAsync(account.AccountUid);
            campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

            var sweep = BuildNewSweep(account, campaign);
            await _feature.SweepsRepository.CreateAsync(sweep, TestData.WindowsTimezone, false);
            sweep = await _feature.SweepsRepository.GetAsync(sweep.SweepUid);

            var prize1 = BuildNewPrize(TestData.PrizeUid1, sweep.SweepUid, TestData.PrizeName1, TestData.PrizeName1, TestData.PrizeValue1, 1);
            await _feature.SweepsPrizesRepository.CreateSweepPrizeAsync(prize1);

            var sweepWinner1 = BuildNewSweepWinner(sweep, TestData.SweepWinnerUid1, TestData.UserIdentifier1, 1);
            await _feature.SweepWinnersRepository.SaveSweepWinnerAsync(sweepWinner1);
            sweepWinner1 = await _feature.SweepWinnersRepository.GetSweepWinnerAsync(TestData.SweepWinnerUid1);

            var backupWinner1 = BuildNewSweepWinner(sweep, sweepWinner1.SweepWinnerUid, TestData.UserIdentifier2, 1);
			var backupWinner2 = BuildNewSweepWinner(sweep, sweepWinner1.SweepWinnerUid, TestData.UserIdentifier3, 2);
			
			await _feature.SweepWinnersRepository.SaveSweepBackupWinnerAsync(backupWinner1);
            await _feature.SweepWinnersRepository.SaveSweepBackupWinnerAsync(backupWinner2);

            var request = new PromoteNextBackupWinnerRequest(sweep, prize1, sweepWinner1, "someReason");
            await _feature.Mediator.Send(request);

            var promotedBackupWinner = await _feature.SweepWinnersRepository.GetSweepWinnerAsync(TestData.SweepWinnerUid1);

            promotedBackupWinner.Should().NotBeNull();
            promotedBackupWinner.UserIdentifier.Should().Be(backupWinner1.UserIdentifier);

            request = new PromoteNextBackupWinnerRequest(sweep, prize1, sweepWinner1, "someReason");
            await _feature.Mediator.Send(request);

            var nextPromotedBackupWinner = await _feature.SweepWinnersRepository.GetSweepWinnerAsync(TestData.SweepWinnerUid1);

			nextPromotedBackupWinner.Should().NotBeNull();
			nextPromotedBackupWinner.UserIdentifier.Should().Be(backupWinner2.UserIdentifier);
        }

        [Fact]
        public async Task When_We_Have_3_Backup_Winner_To_Promote_We_Promote_Each_Backup_Winner_1_At_A_Time()
        {
            _feature.GivenACleanSlate();

            var account = BuildNewAccount();
            var campaign = BuildNewCampaign(account, BuildNewCampaignType());

            await _feature.AccountRepository.CreateAsync(account);
            await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);

            account = await _feature.AccountRepository.GetAsync(account.AccountUid);
            campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

            var sweep = BuildNewSweep(account, campaign);
            await _feature.SweepsRepository.CreateAsync(sweep, TestData.WindowsTimezone, false);
            sweep = await _feature.SweepsRepository.GetAsync(sweep.SweepUid);

            var prize1 = BuildNewPrize(TestData.PrizeUid1, sweep.SweepUid, TestData.PrizeName1, TestData.PrizeName1, TestData.PrizeValue1, 1);
            await _feature.SweepsPrizesRepository.CreateSweepPrizeAsync(prize1);

            var sweepWinner1 = BuildNewSweepWinner(sweep, TestData.SweepWinnerUid1, TestData.UserIdentifier1, 1);
            await _feature.SweepWinnersRepository.SaveSweepWinnerAsync(sweepWinner1);
            sweepWinner1 = await _feature.SweepWinnersRepository.GetSweepWinnerAsync(TestData.SweepWinnerUid1);

            var backupWinner1 = BuildNewSweepWinner(sweep, sweepWinner1.SweepWinnerUid, TestData.UserIdentifier2, 1);
            var backupWinner2 = BuildNewSweepWinner(sweep, sweepWinner1.SweepWinnerUid, TestData.UserIdentifier3, 2);
            var backupWinner3 = BuildNewSweepWinner(sweep, sweepWinner1.SweepWinnerUid, TestData.UserIdentifier4, 3);

            await _feature.SweepWinnersRepository.SaveSweepBackupWinnerAsync(backupWinner1);
            await _feature.SweepWinnersRepository.SaveSweepBackupWinnerAsync(backupWinner2);
            await _feature.SweepWinnersRepository.SaveSweepBackupWinnerAsync(backupWinner3);

            var request = new PromoteNextBackupWinnerRequest(sweep, prize1, sweepWinner1, "someReason");
            await _feature.Mediator.Send(request);

            var promotedBackupWinner = await _feature.SweepWinnersRepository.GetSweepWinnerAsync(TestData.SweepWinnerUid1);

            promotedBackupWinner.Should().NotBeNull();
            promotedBackupWinner.UserIdentifier.Should().Be(backupWinner1.UserIdentifier);

            request = new PromoteNextBackupWinnerRequest(sweep, prize1, sweepWinner1, "someReason");
            await _feature.Mediator.Send(request);

            var nextPromotedBackupWinner = await _feature.SweepWinnersRepository.GetSweepWinnerAsync(TestData.SweepWinnerUid1);

            nextPromotedBackupWinner.Should().NotBeNull();
            nextPromotedBackupWinner.UserIdentifier.Should().Be(backupWinner2.UserIdentifier);

            request = new PromoteNextBackupWinnerRequest(sweep, prize1, sweepWinner1, "someReason");
            await _feature.Mediator.Send(request);

            var nextPromotedBackupWinner1 = await _feature.SweepWinnersRepository.GetSweepWinnerAsync(TestData.SweepWinnerUid1);

            nextPromotedBackupWinner1.Should().NotBeNull();
            nextPromotedBackupWinner1.UserIdentifier.Should().Be(backupWinner3.UserIdentifier);

            request = new PromoteNextBackupWinnerRequest(sweep, prize1, sweepWinner1, "some other reason");
            await _feature.Mediator.Send(request);

            var nextPromotedBackupWinner2 = await _feature.SweepWinnersRepository.GetSweepWinnerAsync(TestData.SweepWinnerUid1);
            nextPromotedBackupWinner2.Should().NotBeNull();
            nextPromotedBackupWinner2.UserIdentifier.Should().Be(backupWinner3.UserIdentifier);
        }
    }
}