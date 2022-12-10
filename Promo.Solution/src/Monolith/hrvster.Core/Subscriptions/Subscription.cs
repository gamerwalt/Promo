namespace hrvster.Core.Subscriptions
{
	public class Subscription
	{
		public string PlanName { get; set; }
		public bool IsDiscounted { get; set; }
		public double DiscountedBy { get; set; }
		public int PromotionsPerMonth { get; set; }
		public int SweepstakesPerMonth { get; set; }
		public int SweepstakesMaximumEntries { get; set; }
		public double PricePerMonth { get; set; }
		public double PricePerYear { get; set; }
	}
}
