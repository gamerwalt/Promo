using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace hrvster.Models.Subscriptions.Flutterwave
{
	public class FlutterwaveEvent
	{
		public string Payload { get; set; }
		public FlutterwaveEventStatus EventStatus { get; set; }
		public int FlutterwaveEventId { get; set; }
		public Guid FlutterwaveEventUid { get; set; }
	}

	public class FlutterwaveCardTransaction
	{
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("txRef")]
		public string TxRef { get; set; }

		[JsonProperty("flwRef")]
		public string FlwRef { get; set; }

		[JsonProperty("orderRef")]
		public string OrderRef { get; set; }

		[JsonProperty("paymentPlan")]
		public long PaymentPlan { get; set; }

		[JsonProperty("paymentPage")]
		public long PaymentPage { get; set; }

		[JsonProperty("amount")]
		public long Amount { get; set; }

		[JsonProperty("charged_amount")]
		public long ChargedAmount { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("customer")]
		public Customer Customer { get; set; }

		[JsonProperty(PropertyName = "event.type")]
		public dynamic EventType { get; set; }
	}
}
