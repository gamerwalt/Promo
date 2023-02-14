using System;

namespace hrvster.Models.Subscriptions
{
	public class ClientAccountSubscription
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public Guid SubscriptionUid { get; set; }
		public string ExternalAccountId { get; set; }
		public string ExternalSubscriptionId { get; set; }
	}
}
