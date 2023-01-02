using System;

namespace hrvster.Models.Subscriptions
{
	public class AccountSubscription
	{
		public int SubscriptionId { get; set; }
		public Guid SubscriptionUid { get; set; }
		public SubscriptionStatus SubscriptionStatus { get; set; }
		public Account Account { get; set; }
		public Plan Plan { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime StartDateUtc { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime EndDateUtc { get; set; }
		public bool IsDiscounted { get; set; }
		public int DiscountedBy { get; set; }
	}
}
