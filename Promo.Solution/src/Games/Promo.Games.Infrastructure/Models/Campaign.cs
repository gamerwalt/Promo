using Promo.Core;
using System;

namespace Promo.Games.Infrastructure.Models
{
	public class Campaign
	{
		public int CampaignId { get; set; }
		public string CampaignName { get; set; }
		public Guid CampaignUid { get; set; }
		public Account Account { get; set; }
		public CampaignTypeCode CampaignTypeCode { get; set; }
		public bool ActiveInd { get; set; }
	}
}
