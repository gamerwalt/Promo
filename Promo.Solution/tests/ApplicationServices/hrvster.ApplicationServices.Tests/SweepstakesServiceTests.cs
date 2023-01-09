using FluentAssertions;
using hrvster.ApplicationServices.Tests.Common;
using hrvster.Web.Mvc.Common.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities;
using TestUtilities.Data;
using Xunit;

namespace hrvster.ApplicationServices.Tests
{
	public class SweepstakesServiceTests : BuilderClassAbstract, IClassFixture<SweepsFeature>
	{
		private readonly SweepsFeature _feature;

		public SweepstakesServiceTests(SweepsFeature feature)
			=> _feature = feature;

		[Fact]
		public async Task When_We_Retrieve_The_Sweepstakes_Going_Live_We_Should_Return_None()
		{
			_feature.GivenACleanSlate();

			var sweepsGoingLive = await _feature.SweepstakesService.GetLiveSweepsAsync(default);

			sweepsGoingLive.Count().Should().Be(0);
		}

		[Fact]
		public async Task Should_Return_2_Sweepstakes_Going_Live()
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

			var sweep2 = BuildNewSweep(account, campaign, TestData.SweepUid2, DateTime.Now.AddMinutes(30), DateTime.Now.AddDays(30));
			sweep2.Name = "Sweep2";
			await _feature.SweepsRepository.CreateAsync(sweep2, TestData.WindowsTimezone, false);

			var sweep3 = BuildNewSweep(account, campaign, TestData.SweepUid3, DateTime.Now.AddHours(2), DateTime.Now.AddDays(30));
			sweep3.Name = "Sweep3";
			await _feature.SweepsRepository.CreateAsync(sweep3, TestData.WindowsTimezone, false);

			var sweepsGoingLive = await _feature.SweepstakesService.GetLiveSweepsAsync(default);

			sweepsGoingLive.Count().Should().Be(2);
		}
	}
}
