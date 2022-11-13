namespace hrvster.Models.Options
{
	public class HarvesterSettingsOptions
	{
		public static string SectionName = "HrvsterSettings";

		public int UserTweetRateLimiterInMinutes { get; set; } = 0;
		public string HrvsterUrl { get; set; }
		public double HrvsterRestartMinutes { get; set; } = 5;
	}
}
