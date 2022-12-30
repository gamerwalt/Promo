using Newtonsoft.Json;
using System;

namespace hrvster.Models.Subscriptions.Flutterwave
{
	public class Entity
	{
		[JsonProperty("card6")]
		public long Card6 { get; set; }

		[JsonProperty("card_last4")]
		public long CardLast4 { get; set; }

		[JsonProperty("card_country_iso")]
		public string CardCountryIso { get; set; }

		[JsonProperty("createdAt")]
		public DateTimeOffset CreatedAt { get; set; }
	}
}
