using hrvster.Models.WebModels;
using hrvster.Models.WebModels.Sweeps;
using System;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class ViewPromotion
	{
		public Guid CampaignUid { get; set; }
		public PromotionType PromotionType { get; set; }
		public string CampaignName { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public List<Sweepstake> Sweepstakes { get; set; }

		public UpdatePromotion UpdatePromotion { get; set; }

		public bool IsActive()
		{
			return DateTime.UtcNow >= StartDate.ToUniversalTime() && DateTime.UtcNow <= EndDate.ToUniversalTime();
		}

		public bool WillBeInProgress()
		{
			return DateTime.UtcNow < StartDate.ToUniversalTime();
		}
	}
}
