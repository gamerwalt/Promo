using hrvster.Models;
using System;

namespace TestUtilities.Builders.Resources
{
	public class CampaignBuilder
	{
		public int CampaignId { get; set; }
		public Guid CampaignUid { get; set; }
		public Account Account { get; set; }
		public CampaignType CampaignType { get; set; }
		public string CampaignName { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public bool ActiveInd { get; set; }

		public CampaignBuilder(int campaignId, Guid campaignUid, string campaignName, string description, DateTime startDate, DateTime endDate, Account account, CampaignType campaignType)
		{
			CampaignId = campaignId;
			CampaignUid = campaignUid;
			CampaignName = campaignName;
			Description = description;
			StartDate = startDate;
			EndDate = endDate;
			Account = account;
			CampaignType = campaignType;
		}

		public CampaignBuilder Create(int campaignId, Guid campaignUid, string campaignName, string description, DateTime startDate, DateTime endDate, Account account, CampaignType campaignType)
			=> new CampaignBuilder(
				campaignId,
				campaignUid,
				campaignName,
				description,
				startDate,
				endDate,
				account,
				campaignType)
					.WithActiveInd(true)
					.WithAccount(account)
					.WithCampaignType(campaignType);

		public Campaign Build()
			=> new Campaign
			{
				Account = Account,
				ActiveInd = ActiveInd,
				CampaignId = CampaignId,
				CampaignUid = CampaignUid,
				Description = Description,
				StartDate = StartDate,
				EndDate = EndDate,
				CampaignName = CampaignName,
				CampaignType = CampaignType
			};

		public CampaignBuilder WithCampaignType(CampaignType campaignType)
		{
			CampaignType = campaignType;

			return this;
		}

		public CampaignBuilder WithActiveInd(bool activeInd)
		{
			ActiveInd = activeInd;

			return this;
		}

		public CampaignBuilder WithAccount(Account account)
		{
			Account = account;

			return this;
		}
	}
}
