using System;

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
	}
}
