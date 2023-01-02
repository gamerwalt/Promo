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
	}
}
