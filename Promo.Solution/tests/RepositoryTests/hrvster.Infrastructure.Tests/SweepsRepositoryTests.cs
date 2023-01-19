using FluentAssertions;
using hrvster.Infrastructure.Tests.Common;
using hrvster.Web.Mvc.Common.Steps;
using TestUtilities;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Infrastructure.Tests;

[Collection("Sequential")]
public class SweepsRepositoryTests : BuilderClassAbstract, IClassFixture<RepositoryFeature>
{
	private readonly RepositoryFeature _feature;

	public SweepsRepositoryTests(RepositoryFeature feature)
		=> _feature = feature;

	[Fact]
	public async Task When_We_Call_To_Get_Sweepstakes_That_Are_Going_Live_In_1_Hour_We_Receive_Nothing()
	{
		_feature.GivenACleanSlate();

		var sweeps = await _feature.SweepsRepository.GetAllGoingLiveInHoursAsync(1, default);

		sweeps.Count().Should().Be(0);
	}

	[Fact]
	public async Task When_We_Have_3_Sweeps_And_2_Sweeps_Going_Live_In_An_Hour_We_Return_Only2()
	{
		_feature.GivenACleanSlate();
		
		var account = BuildNewAccount();
		await _feature.AccountsRepository.CreateAsync(account);
		var campaignType = BuildNewCampaignType();
		var campaign = BuildNewCampaign(account, campaignType);
		await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);
		var sweep1 = BuildNewSweep(account, campaign, TestData.SweepUid, DateTime.Now.AddMinutes(50), DateTime.Now.AddDays(30));
		await _feature.SweepsRepository.CreateAsync(sweep1, TestData.WindowsTimezone, false);

		var sweep2 = BuildNewSweep(account, campaign, TestData.SweepUid2, DateTime.Now.AddMinutes(30), DateTime.Now.AddDays(30));
		sweep2.Name = $"{sweep2.Name}2";
		await _feature.SweepsRepository.CreateAsync(sweep2, TestData.WindowsTimezone, false);

		var sweep3 = BuildNewSweep(account, campaign, TestData.SweepUid3, DateTime.Now.AddMinutes(121), DateTime.Now.AddDays(30));
		sweep3.Name = $"{sweep3.Name}3";
		await _feature.SweepsRepository.CreateAsync(sweep3, TestData.WindowsTimezone, false);

		var sweeps = await _feature.SweepsRepository.GetAllGoingLiveInHoursAsync(1, default);

		sweeps.Count().Should().Be(2);
	}
}
