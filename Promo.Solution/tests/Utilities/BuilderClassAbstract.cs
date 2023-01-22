using Bogus;
using hrvster.Models;
using hrvster.Models.Profiles;
using System;
using System.Collections.Generic;
using TestUtilities.Builders.Resources;
using TestUtilities.Data;

namespace TestUtilities
{
	public abstract class BuilderClassAbstract
	{
		public Account BuildNewAccount()
			=> new AccountBuilder(
				TestData.AccountId,
				TestData.AccountUid,
				TestData.EmailAddress,
				true,
				TestData.PhoneNumber,
				TestData.DisplayName)
			.Build();

        public Account BuildNewAccount(Guid accountUid)
            => new AccountBuilder(
                TestData.AccountId,
                accountUid,
                TestData.EmailAddress,
                true,
                TestData.PhoneNumber,
                TestData.DisplayName)
            .Build();

        public Campaign BuildNewCampaign(Account account, CampaignType campaignType)
			=> new CampaignBuilder(
				TestData.CampaignId,
				TestData.CampaignUid,
				TestData.CampaignName,
				TestData.CampaignDescription,
				DateTime.Now.AddDays(-30),
				DateTime.Now.AddDays(20),
				account,
				campaignType)
			.Build();

		public Profile BuildNewTwitterProfile(Account account)
			=> new Profile
			{
				Account = account,
				AccessToken = TestData.AccessToken,
				AccessTokenSecret = TestData.AccessTokenSecret,
				ConsumerKey = TestData.ConsumerKey,
				ConsumerSecret = TestData.ConsumerKeySecret,
				DisplayName = TestData.DisplayName,
				ExternalProfileId = TestData.ExternalProfileId,
				ProfileUid = TestData.ProfileUid,
				ScreenName = TestData.ScreenName,
				ProfileType = ProfileType.Twitter,
				Description = TestData.ProfileDescription
			};


        public SweepEntryRule BuildNewSweepEntryRule(Guid entryRuleUid, Sweep sweep, Campaign campaign, int entryCount)
			=> new SweepEntryRuleBuilder(entryRuleUid, campaign.CampaignUid, sweep.SweepUid, entryCount)
			.Build();

		public DrawRule BuildSweepDrawRule(Guid sweepDrawRuleUid, Sweep sweep, int winnerCount, int winnerBackupCount)
			=> new SweepDrawRuleBuilder(sweepDrawRuleUid, sweep.SweepUid, winnerCount, winnerBackupCount)
			.Build();

		public Rule BuildRule(Guid ruleUid, Campaign campaign, Sweep sweep, string watchValue)
			=> new RuleBuilder(ruleUid, campaign, sweep, watchValue)
			.Build();

        public CampaignType BuildNewCampaignType()
			=> new CampaignType
			{
				CampaignTypeName = TestData.CampaignTypeName,
				CampaignTypeCode = CampaignTypeCode.AutoEnterSweepsFromTweetReplyHashtag,
				CampaignTypeId = TestData.CampaignTypeId,
				CampaignTypeUid = TestData.CampaignTypeUid,
				IsActive = true,
				CamapaignTypeDescription = TestData.CampaignTypeName
			};

		public Sweep BuildNewSweep(Account account, Campaign campaign)
			=> new SweepBuilder(
				TestData.SweepId,
				TestData.SweepUid,
				TestData.SweepName)
			.WithStartDate(DateTime.Now.AddDays(-30))
			.WithEndDate(DateTime.Now.AddDays(-1))
			.WithDrawDate(DateTime.Now.AddDays(-1))
			.WithAccount(account)
			.WithCampaign(campaign)
			.WithDrawn(false)
			.Build();

		public Sweep BuildNewSweep(Account account, Campaign campaign, Guid sweepUid)
			=> new SweepBuilder(
				TestData.SweepId,
				sweepUid,
				TestData.SweepName)
			.WithStartDate(DateTime.Now.AddDays(-30))
			.WithEndDate(DateTime.Now.AddDays(-1))
			.WithDrawDate(DateTime.Now.AddDays(-1))
			.WithAccount(account)
			.WithCampaign(campaign)
			.WithDrawn(false)
			.Build();

        public Sweep BuildNewSweep(Account account, Campaign campaign, Guid sweepUid, DateTime startDate, DateTime endDate)
            => new SweepBuilder(
                TestData.SweepId,
                sweepUid,
                TestData.SweepName)
            .WithStartDate(startDate)
            .WithEndDate(endDate)
            .WithDrawDate(endDate.AddDays(-1))
            .WithAccount(account)
            .WithCampaign(campaign)
            .WithDrawn(false)
            .Build();

        public Sweep BuildNewSweep(Account account, Campaign campaign, Guid sweepUid, DateTime startDate, DateTime endDate, DateTime drawDate)
			=> new SweepBuilder(
				TestData.SweepId,
				sweepUid,
				TestData.SweepName)
			.WithStartDate(startDate)
			.WithEndDate(endDate)
			.WithDrawDate(drawDate)
			.WithAccount(account)
			.WithCampaign(campaign)
			.WithDrawn(false)
			.Build();

		public SweepWinner BuildNewSweepWinner(Sweep sweep, Guid sweepWinnerUid, string userIdentifier, int assignedId)
			=> new SweepWinnerBuilder(sweep, sweepWinnerUid, userIdentifier, assignedId)
				.Build();

		public SweepPrize BuildNewPrize(Guid prizeUid, Guid sweepUid, string prizeName, string prizeDescription, decimal prizeValue, int prizePosition)
			=> new SweepPrizeBuilder(prizeUid, sweepUid, prizeName, prizeDescription, prizeValue, prizePosition)
				.Build();

		public SweepBackupWinner BuildNewBackupWinner(int sweepWinnerId, Guid sweepWinnerUid, Sweep sweep, string userIdentifier)
			=> new SweepBackupWinnerBuilder(sweepWinnerId, sweepWinnerUid, sweep, userIdentifier)
				.Build();

        public IEnumerable<SweepEntry> SeedEntriesFor(Sweep sweep, Rule rule)
        {
			var testSweepEntry = new Faker<SweepEntry>()
				.RuleFor(s => s.Sweep, sweep)
				.RuleFor(s => s.Rule, rule)
				.RuleFor(s => s.SweepEntryUid, f => f.Random.Guid())
				.RuleFor(s => s.UserIdentifier, f => f.Internet.UserName())
				.RuleFor(s => s.UserIdentifierType, f => UserIdentifierType.TwitterHandle)
				.RuleFor(s => s.AcceptedTerms, f => true);

			return testSweepEntry.Generate(30);
        }
    }
}
