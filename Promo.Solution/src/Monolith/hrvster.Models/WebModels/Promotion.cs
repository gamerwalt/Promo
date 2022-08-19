using hrvster.Models.WebModels.Sweeps;
using System;
using System.Collections.Generic;

namespace hrvster.Models.WebModels
{
	public class Promotion
	{
		public int CampaignId { get; set; }
		public Guid CampaignUid { get; set; }
		public Account Account { get; set; }
		public PromotionType PromotionType { get; set; }
		public string CampaignName { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool ActiveInd { get; set; }
		public IEnumerable<Sweepstake> Sweepstakes { get; set; }

		public bool IsActive()
		{
			return DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
		}
	}
}
