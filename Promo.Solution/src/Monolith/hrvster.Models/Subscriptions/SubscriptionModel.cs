namespace hrvster.Models
{
	public class SubscriptionModel
	{
		public string PlanName { get; set; }
		public bool IsDiscounted { get; set; }
		public double DiscountedBy { get; set; }
		public int PromotionsPerMonth { get; set; }
		public int SweepstakesPerMonth { get; set; }
		public int SweepstakesMaximumEntries { get; set; }
		public decimal PricePerMonth { get; set; }
		public decimal PricePerYear { get; set; }
	}
}
