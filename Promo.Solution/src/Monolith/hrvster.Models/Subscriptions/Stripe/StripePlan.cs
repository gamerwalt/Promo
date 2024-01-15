using System;

namespace hrvster.Models.Subscriptions.Stripe
{
	public class StripePlan
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public int PlanId { get; set; }
		public string ExternalId { get; set; }
	}
}
