using hrvster.Models.Subscriptions.Flutterwave;
using hrvster.Models.Subscriptions.Stripe;
using System;

namespace hrvster.Models.Subscriptions
{
	public class Plan
	{
		public int PlanId { get; set; }
		public Guid PlanUid { get; set; }
		public string PlanName { get; set; }
		public string PlanDescription { get; set; }
		public int PromotionsPerMonth { get; set; }
		public int SweepstakesPerMonth { get; set; }
		public int SweepstakesMaximumEntries { get; set; }
		public int HashtagsPerMonth { get; set; }
		public decimal PricePerMonth { get; set; }
		public decimal PricePerYear { get; set; }
		public FlutterwavePlan FlutterwavePlan { get; set; }
		public StripePlan StripePlan { get; set; }
	}
}
