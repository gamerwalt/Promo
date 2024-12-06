namespace hrvster.Models.Options
{
	public class HarvesterSettingsOptions
	{
		public static string SectionName = "HrvsterSettings";

		public int UserTweetRateLimiterInMinutes { get; set; } = 0;
		public string HrvsterUrl { get; set; }
		public int HrvsterRestartMinutes { get; set; } = 10;
		public string ServiceUsername { get; set; }
		public string ServicePassword { get; set; }
		public int ServiceWaitInSeconds { get; set; } = 10;
		public string ServiceUri { get; set; }
		public bool NotifyWinners { get; set; }
		public string SignatureKey { get; set; }
		public int SignatureExpiryInMinutes { get; set; }
		public string RedisCache { get; set; } = "";
		public string FacebookwebhookSecret { get; set; }
		public string RebrandlyApiKey { get; set; }
		public string GameLinkReplaceText { get; set; }
		public string HrvsterUrlForSubdomains { get; set; }
		public string HrvsterAzureStorageConnectionString { get; set; }
		public string EncryptionKey { get; set; }
		public string EncryptionIV { get; set; }
		public string StorageAccountName { get; set; }
		public string StorageAccountKey { get; set; }
	}
}
