namespace hrvster.Models.Options
{
	public class HarvesterSettingsOptions
	{
		public static string SectionName = "HrvsterSettings";

		public int UserTweetRateLimiterInMinutes { get; set; } = 0;
	}
}
