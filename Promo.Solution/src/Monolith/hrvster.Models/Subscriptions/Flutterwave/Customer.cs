using Newtonsoft.Json;

namespace hrvster.Models.Subscriptions.Flutterwave
{
	public class Customer
	{
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("fullName")]
		public string FullName { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("AccountId")]
		public long AccountId { get; set; }
	}
}
