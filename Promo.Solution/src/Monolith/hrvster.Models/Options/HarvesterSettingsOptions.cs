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
		public int TrialDays { get; set; } = 15;
		public int TrialMatchGames { get; set; } = 1;
		public int TrialMatchRules { get; set; } = 1;
		public int TrialMatchLinkRules { get; set; } = 1;
		public int TrialMatchPlays { get; set; } = 100;
		public  int TrialMatchBoards { get; set; } = 100;
		public int TrialPromotions { get; set; } = 1;
		public int TrialSweepGames { get; set; } = 1;
		public int TrialSweepRules { get; set; } = 1;
		public int TrialSweepLinkRules { get; set; } = 1;
		public int TrialSweepEntries { get; set; } = 100;
		public int MaxRulesPerAccount { get; set; } = 2;
	}
}
