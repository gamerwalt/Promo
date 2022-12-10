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
		public double PricePerMonth { get; set; }
		public double PricePerYear { get; set; }
	}
}
