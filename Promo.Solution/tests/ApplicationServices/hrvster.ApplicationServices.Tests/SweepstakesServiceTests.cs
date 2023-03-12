using Bogus;
using FluentAssertions;
using hrvster.ApplicationServices.Tests.Common;
using hrvster.Models;
using hrvster.Models.Subscriptions;
using hrvster.Web.Mvc.Common.Steps;
using TestUtilities;
using TestUtilities.Data;
using Xunit;

namespace hrvster.ApplicationServices.Tests
{
    [Collection("Sequential")]
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
        public async Task Should_Return_2_Sweepstakes_That_Are_Going_Live_In_An_Hour()
        {
            _feature.GivenACleanSlate();

            var account = BuildNewAccount(Guid.NewGuid());
            var campaign = BuildNewCampaign(account, BuildNewCampaignType());
            await _feature.AccountRepository.CreateAsync(account);
            await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);
            account = await _feature.AccountRepository.GetAsync(account.AccountUid);
            campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

            var sweep = BuildNewSweep(account, campaign, TestData.SweepUid, DateTime.Now.AddMinutes(50), DateTime.Now.AddDays(30));
            await _feature.SweepsRepository.CreateAsync(sweep, TestData.WindowsTimezone, false);

            var sweep2 = BuildNewSweep(account, campaign, TestData.SweepUid2, DateTime.Now.AddMinutes(40), DateTime.Now.AddDays(30));
            sweep2.Name = "Sweep2";
            await _feature.SweepsRepository.CreateAsync(sweep2, TestData.WindowsTimezone, false);

            var sweep3 = BuildNewSweep(account, campaign, TestData.SweepUid3, DateTime.Now.AddHours(3), DateTime.Now.AddDays(30));
            sweep3.Name = "Sweep3";
            await _feature.SweepsRepository.CreateAsync(sweep3, TestData.WindowsTimezone, false);

            var sweepsGoingLive = await _feature.SweepstakesService.GetSweepsGoingLiveInHourAsync(1, default);

            sweepsGoingLive.Count().Should().Be(2);
        }

        [Fact]
        public async Task Should_Return_2_Sweepstakes_That_Are_Live()
        {
            _feature.GivenACleanSlate();

            var account = BuildNewAccount();
            var campaign = BuildNewCampaign(account, BuildNewCampaignType());
            await _feature.AccountRepository.CreateAsync(account);
            await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);
            account = await _feature.AccountRepository.GetAsync(account.AccountUid);
            campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

            var sweep = BuildNewSweep(account, campaign, TestData.SweepUid, DateTime.Now.AddMinutes(50), DateTime.Now.AddDays(30));
            await _feature.SweepsRepository.CreateAsync(sweep, TestData.WindowsTimezone, false);

            var sweep2 = BuildNewSweep(account, campaign, TestData.SweepUid2, DateTime.Now.AddMinutes(30), DateTime.Now.AddDays(30));
            sweep2.Name = "Sweep2";
            await _feature.SweepsRepository.CreateAsync(sweep2, TestData.WindowsTimezone, false);

            var sweep3 = BuildNewSweep(account, campaign, TestData.SweepUid3, DateTime.Now.AddHours(3), DateTime.Now.AddDays(30));
            sweep3.Name = "Sweep3";
            await _feature.SweepsRepository.CreateAsync(sweep3, TestData.WindowsTimezone, false);

            var liveSweeps = await _feature.SweepstakesService.GetLiveSweepsAsync(default);

            liveSweeps.Count().Should().Be(2);
        }

        [Theory]
        [InlineData(5000, SubscriptionPlans.Basic)]
		[InlineData(5001, SubscriptionPlans.Basic)]
		[InlineData(4999, SubscriptionPlans.Basic)]
		[InlineData(12000, SubscriptionPlans.Starter)]
		[InlineData(12001, SubscriptionPlans.Starter)]
		[InlineData(11999, SubscriptionPlans.Starter)]
		[InlineData(30000, SubscriptionPlans.Advanced)]
		[InlineData(30001, SubscriptionPlans.Advanced)]
		[InlineData(29999, SubscriptionPlans.Advanced)]
		public async Task Should_Return_False_If_The_Sweepstake_IsNot_Maxed_Out(
            int entryCount, 
            SubscriptionPlans subscriptionPlan)
        {
			_feature.GivenACleanSlate();

			var account = await BuildAccountAndSubscribeToPlan(_feature, subscriptionPlan);

			var campaign = BuildNewCampaign(account, BuildNewCampaignType());
			await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);
			account = await _feature.AccountRepository.GetAsync(account.AccountUid);
			campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

			var sweep = BuildNewSweep(account, campaign, TestData.SweepUid, DateTime.Now, DateTime.Now.AddDays(30));
			await _feature.SweepsRepository.CreateAsync(sweep, TestData.WindowsTimezone, false);

            var accountSubscription = await _feature.AccountSubscriptionService.GetAccountSubscriptionAsync(account.AccountUid);

            var ruleUid = Guid.NewGuid();
            var rule = BuildRule(ruleUid, campaign, sweep, TestData.WatchValue);

            await _feature.SweepRulesRepository.CreateRuleAsync(rule);

            var entryFaker = new Faker<SweepEntry>();
            entryFaker.RuleFor(e => e.Sweep, f => sweep)
                .RuleFor(e => e.Rule, f => rule)
                .RuleFor(e => e.AcceptedTerms, f => true)
                .RuleFor(e => e.SweepEntryUid, f => Guid.NewGuid())
                .RuleFor(e => e.UserIdentifier, f => f.Internet.UserName())
                .RuleFor(e => e.UserIdentifierType, f => UserIdentifierType.TwitterHandle);

            var entries = entryFaker.Generate(entryCount);
            var counter = 0;

            foreach(var entry in entries)
            {
                if(counter < accountSubscription.Plan.SweepstakesMaximumEntries)
                {
                    await _feature.SweepEntryRepository.CreateAsync(entry);
                    counter++;
                }
            }

            var result = await _feature.SweepstakesService.IsMaxedOut(sweep, default);

            if (entryCount >= accountSubscription.Plan.SweepstakesMaximumEntries)
                result.Should().BeTrue();
            else
                result.Should().BeFalse();
		}

		public async Task<Models.Account> BuildAccountAndSubscribeToPlan(
            SweepsFeature _feature, SubscriptionPlans subscriptionPlans = SubscriptionPlans.Basic)
		{
			var account = await CreateAccountAsync(_feature);
            var plan = default(Plan);

			if (subscriptionPlans == SubscriptionPlans.Basic)
			    plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.BasicPlanUid, default);
            else if(subscriptionPlans == SubscriptionPlans.Starter)
				plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.StarterPlanUid, default);
            else if(subscriptionPlans == SubscriptionPlans.Advanced)
				plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.AdvancedPlanUid, default);

			await _feature.SubscriptionService.SubscribeUserToAsync(plan, account.AccountUid);

			return account;
		}

		public async Task<Models.Account> CreateAccountAsync(SweepsFeature _feature)
		{
			var account = BuildNewAccount();
			await _feature.AccountRepository.CreateAsync(account);

			return await _feature.AccountRepository.GetByEmailAddressAsync(account.EmailAddress);
		}
	}
}
