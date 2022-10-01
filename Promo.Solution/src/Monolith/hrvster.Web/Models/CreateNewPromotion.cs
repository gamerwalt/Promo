using System;

namespace hrvster.Web.Models
{
	public class CreateNewPromotion
	{
		public string PromotionName { get; set; }
		public string PromotionDescription { get; set; }
		public DateTime PromotionStartDate { get; set; }
		public DateTime PromotionEndDate { get; set; }
		public Guid PromotionTypeUid { get; set; }

		public CreateNewSweep CreateNewSweep { get; set; }
	}
}
