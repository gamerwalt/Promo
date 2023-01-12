using hrvster.Models;
using System;
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
			.WithDrawDate(endDate.AddDays(1))
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
	}
}
