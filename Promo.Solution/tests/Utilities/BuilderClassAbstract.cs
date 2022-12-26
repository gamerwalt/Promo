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
	}
}
