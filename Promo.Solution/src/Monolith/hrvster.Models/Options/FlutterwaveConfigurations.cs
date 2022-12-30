namespace hrvster.Models.Options
{
	public class FlutterwaveConfigurations
	{
		public const string SectionName = "Flutterwave";

		public string SecretKey { get; set; }
		public string PublicKey { get; set; }
		public string EncryptionKey { get; set; }
		public string SecretWebhookHash { get; set; }
	}
}
