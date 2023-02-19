using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace hrvster.Models.Subscriptions.Flutterwave
{
	public class FlutterwaveSubscriptionBase
	{
		public string Status { get; set; }
		public string Message { get; set; }

		[JsonProperty("data")]
		public List<FlutterwaveSubscription> Subscriptions { get; set; }
	}

	public class FlutterwaveSubscription
	{
		public long Id { get; set; }
		public long Amount { get; set; }
		public Customer Customer { get; set; }
		public long Plan { get; set; }
		public string Status { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}
