using FluentAssertions;
using hrvster.Infrastructure.Tests.Common;
using hrvster.Models;
using hrvster.Web.Mvc.Common.Steps;
using TestUtilities;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Infrastructure.Tests
{
	[Collection("Sequential")]
	public class SweepsNotificationRepositoryTests : BuilderClassAbstract, IClassFixture<RepositoryFeature>
	{
		private readonly RepositoryFeature _feature;

		public SweepsNotificationRepositoryTests(RepositoryFeature feature)
			=> _feature = feature;

		[Fact]
		public async Task Should_Create_A_Record_With_Draw_Attempted_True_If_We_Create_A_New_Sweep_Notification()
		{
			_feature.GivenACleanSlate();

			Account account = await CreateNewAccountAsync();
			Campaign campaign = await CreateCampaignAsync(account);
			var sweep1 = BuildNewSweep(account, campaign);
			await _feature.SweepsRepository.CreateAsync(sweep1, TestData.WindowsTimezone, true);
			sweep1 = await _feature.SweepsRepository.GetAsync(sweep1.SweepUid);

			await _feature.SweepNotificationRepository.ToggleDrawAttemptedAsync(sweep1.SweepId, default);

			var sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.DrawAttemptHappened.Should().Be(true);
		}

		[Fact]
		public async Task Should_Update_The_Draw_Attempted_True_If_We_Do_Already_Have_A_Sweep_Notification()
		{
			_feature.GivenACleanSlate();

			Account account = await CreateNewAccountAsync();
			Campaign campaign = await CreateCampaignAsync(account);
			var sweep1 = BuildNewSweep(account, campaign);
			await _feature.SweepsRepository.CreateAsync(sweep1, TestData.WindowsTimezone, true);
			sweep1 = await _feature.SweepsRepository.GetAsync(sweep1.SweepUid);

			await _feature.SweepNotificationRepository.ToggleDrawHappenedAsync(sweep1.SweepId, default);

			var sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.DrawAttemptHappened.Should().Be(false);

			await _feature.SweepNotificationRepository.ToggleDrawAttemptedAsync(sweep1.SweepId, default);
			sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.DrawAttemptHappened.Should().Be(true);
		}

		[Fact]
		public async Task Should_Create_A_Record_With_Draw_Happened_If_We_Call_ToggleDrawHappendAsync()
		{
			_feature.GivenACleanSlate();

			Account account = await CreateNewAccountAsync();
			Campaign campaign = await CreateCampaignAsync(account);
			var sweep1 = BuildNewSweep(account, campaign);
			await _feature.SweepsRepository.CreateAsync(sweep1, TestData.WindowsTimezone, true);
			sweep1 = await _feature.SweepsRepository.GetAsync(sweep1.SweepUid);

			await _feature.SweepNotificationRepository.ToggleDrawHappenedAsync(sweep1.SweepId, default);

			var sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.DrawHappened.Should().Be(true);
		}

		[Fact]
		public async Task Should_Update_The_Draw_Happened_True_If_We_Do_Already_Have_A_Sweep_Notification()
		{
			_feature.GivenACleanSlate();

			Account account = await CreateNewAccountAsync();
			Campaign campaign = await CreateCampaignAsync(account);
			var sweep1 = BuildNewSweep(account, campaign);
			await _feature.SweepsRepository.CreateAsync(sweep1, TestData.WindowsTimezone, true);
			sweep1 = await _feature.SweepsRepository.GetAsync(sweep1.SweepUid);

			await _feature.SweepNotificationRepository.ToggleDrawAttemptedAsync(sweep1.SweepId, default);

			var sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.DrawHappened.Should().Be(false);

			await _feature.SweepNotificationRepository.ToggleDrawHappenedAsync(sweep1.SweepId, default);
			sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.DrawHappened.Should().Be(true);
		}

		[Fact]
		public async Task Should_Create_A_Record_With_Sweeps_Live_SetTrue_If_We_Call_ToggleSweepLiveAsync()
		{
			_feature.GivenACleanSlate();

			Account account = await CreateNewAccountAsync();
			Campaign campaign = await CreateCampaignAsync(account);
			var sweep1 = BuildNewSweep(account, campaign);
			await _feature.SweepsRepository.CreateAsync(sweep1, TestData.WindowsTimezone, true);
			sweep1 = await _feature.SweepsRepository.GetAsync(sweep1.SweepUid);

			await _feature.SweepNotificationRepository.ToggleSweepsLiveAsync(sweep1.SweepId, default);

			var sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.SweepsLive.Should().Be(true);
		}

		[Fact]
		public async Task Should_Update_Sweeps_Live_True_If_We_Do_Already_Have_A_Record()
		{
			_feature.GivenACleanSlate();

			Account account = await CreateNewAccountAsync();
			Campaign campaign = await CreateCampaignAsync(account);
			var sweep1 = BuildNewSweep(account, campaign);
			await _feature.SweepsRepository.CreateAsync(sweep1, TestData.WindowsTimezone, true);
			sweep1 = await _feature.SweepsRepository.GetAsync(sweep1.SweepUid);

			await _feature.SweepNotificationRepository.ToggleDrawHappenedAsync(sweep1.SweepId, default);

			var sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.SweepsLive.Should().Be(false);

			await _feature.SweepNotificationRepository.ToggleSweepsLiveAsync(sweep1.SweepId, default);
			sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.SweepsLive.Should().Be(true);
		}

		[Fact]
		public async Task Should_Create_A_Record_With_SweepsGoingLive_SetTrue_If_We_Call_ToggleSweepsGoingLiveAsync()
		{
			_feature.GivenACleanSlate();

			Account account = await CreateNewAccountAsync();
			Campaign campaign = await CreateCampaignAsync(account);
			var sweep1 = BuildNewSweep(account, campaign);
			await _feature.SweepsRepository.CreateAsync(sweep1, TestData.WindowsTimezone, true);
			sweep1 = await _feature.SweepsRepository.GetAsync(sweep1.SweepUid);

			await _feature.SweepNotificationRepository.ToggleSweepsGoingLiveAsync(sweep1.SweepId, default);

			var sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.SweepsGoingLive.Should().Be(true);
		}

		[Fact]
		public async Task Should_Update_SweepsGoingLive_If_We_Do_Already_Have_A_Record()
		{
			_feature.GivenACleanSlate();

			Account account = await CreateNewAccountAsync();
			Campaign campaign = await CreateCampaignAsync(account);
			var sweep1 = BuildNewSweep(account, campaign);
			await _feature.SweepsRepository.CreateAsync(sweep1, TestData.WindowsTimezone, true);
			sweep1 = await _feature.SweepsRepository.GetAsync(sweep1.SweepUid);

			await _feature.SweepNotificationRepository.ToggleDrawHappenedAsync(sweep1.SweepId, default);

			var sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.SweepsGoingLive.Should().Be(false);

			await _feature.SweepNotificationRepository.ToggleSweepsGoingLiveAsync(sweep1.SweepId, default);
			sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.SweepsGoingLive.Should().Be(true);
		}

		[Fact]
		public async Task Should_Create_A_Record_With_NotEnoughUniqueEntrants_SetTrue_If_We_CallTo_Upsert()
		{
			_feature.GivenACleanSlate();

			Account account = await CreateNewAccountAsync();
			Campaign campaign = await CreateCampaignAsync(account);
			var sweep1 = BuildNewSweep(account, campaign);
			await _feature.SweepsRepository.CreateAsync(sweep1, TestData.WindowsTimezone, true);
			sweep1 = await _feature.SweepsRepository.GetAsync(sweep1.SweepUid);

			await _feature.SweepNotificationRepository.ToggleNotEnoughUniqueEntrantsAsync(sweep1.SweepId, default);

			var sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.NotEnoughUniqueEntrants.Should().Be(true);
		}

		[Fact]
		public async Task Should_Update_NotEnoughUniqueEntrants_If_We_Do_Already_Have_A_Record()
		{
			_feature.GivenACleanSlate();

			var sweep1 = await SetupSweepAsync();

			await _feature.SweepNotificationRepository.ToggleDrawHappenedAsync(sweep1.SweepId, default);

			var sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.NotEnoughUniqueEntrants.Should().Be(false);

			await _feature.SweepNotificationRepository.ToggleNotEnoughUniqueEntrantsAsync(sweep1.SweepId, default);
			sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.NotEnoughUniqueEntrants.Should().Be(true);
		}

		[Fact]
		public async Task If_We_Already_Have_A_Property_Set_To_True_It_Should_Remain_True_If_We_Upsert_Another_Property()
		{
			_feature.GivenACleanSlate();

			var sweep1 = await SetupSweepAsync();

			await _feature.SweepNotificationRepository.ToggleDrawHappenedAsync(sweep1.SweepId, default);

			var sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.DrawHappened.Should().Be(true);
			sweepNotification.NotEnoughUniqueEntrants.Should().BeFalse();

			await _feature.SweepNotificationRepository.ToggleNotEnoughUniqueEntrantsAsync(sweep1.SweepId, default);
			sweepNotification = await _feature.SweepNotificationRepository.GetSweepNotificationAsync(sweep1.SweepId, default);

			sweepNotification.DrawHappened.Should().Be(true);
			sweepNotification.NotEnoughUniqueEntrants.Should().Be(true);
		}

		private async Task<Sweep> SetupSweepAsync()
		{
			Account account = await CreateNewAccountAsync();
			Campaign campaign = await CreateCampaignAsync(account);
			var sweep1 = BuildNewSweep(account, campaign);
			await _feature.SweepsRepository.CreateAsync(sweep1, TestData.WindowsTimezone, true);
			sweep1 = await _feature.SweepsRepository.GetAsync(sweep1.SweepUid);
			return sweep1;
		}

		private async Task<Campaign> CreateCampaignAsync(Account account)
		{
			var campaignType = BuildNewCampaignType();
			var campaign = BuildNewCampaign(account, campaignType);

			await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, true);

			return campaign;
		}

		private async Task<Account> CreateNewAccountAsync()
		{
			var account = BuildNewAccount();
			await _feature.AccountsRepository.CreateAsync(account);

			return account;
		}
	}
}
