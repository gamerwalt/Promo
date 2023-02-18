namespace hrvster.Models.Options
{
	public class FlutterwaveConfigurations
	{
		public const string SectionName = "Flutterwave";

		public string SecretKey { get; set; }
		public string PublicKey { get; set; }
		public string EncryptionKey { get; set; }
		public string SecretWebhookHash { get; set; }
		public string BaseUri { get; set; }
	}

	public static class FlutterwaveUrls
	{
		public const string Subscriptions = "subscriptions";
	}

	public static class FlutterwaveParameters
	{
		public const string Email = "email";
		public const string Status = "status";
	}
}
