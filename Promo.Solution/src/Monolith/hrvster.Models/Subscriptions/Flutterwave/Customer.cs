using Newtonsoft.Json;
using System;

namespace hrvster.Models.Subscriptions.Flutterwave
{
	public class Customer
	{
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("phone")]
		public object Phone { get; set; }

		[JsonProperty("fullName")]
		public string FullName { get; set; }

		[JsonProperty("customertoken")]
		public object Customertoken { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("createdAt")]
		public DateTimeOffset CreatedAt { get; set; }

		[JsonProperty("updatedAt")]
		public DateTimeOffset UpdatedAt { get; set; }

		[JsonProperty("deletedAt")]
		public object DeletedAt { get; set; }

		[JsonProperty("AccountId")]
		public long AccountId { get; set; }
	}
}
