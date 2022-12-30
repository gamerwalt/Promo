using Newtonsoft.Json;
using System;

namespace hrvster.Models.Subscriptions.Flutterwave
{
	public class FlutterwaveEvent
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

		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("amount")]
		public long Amount { get; set; }

		[JsonProperty("charged_amount")]
		public long ChargedAmount { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("IP")]
		public string Ip { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("appfee")]
		public long Appfee { get; set; }

		[JsonProperty("merchantfee")]
		public long Merchantfee { get; set; }

		[JsonProperty("merchantbearsfee")]
		public long Merchantbearsfee { get; set; }

		[JsonProperty("customer")]
		public Customer Customer { get; set; }

		[JsonProperty("entity")]
		public Entity Entity { get; set; }

		[JsonProperty("event.type")]
		public string EventType { get; set; }

		public string Payload { get; set; }
		public FlutterwaveEventStatus EventStatus { get; set; }
		public int FlutterwaveEventId { get; set; }
		public Guid FlutterwaveEventUid { get; set; }
	}
}
