namespace hrvster.Models.Options
{
	public class NotificationServiceOptions
	{
		public static string SectionName = "NotificationServiceConfigurationOptions";

		public int DismissInSeconds { get; set; } = 0;
	}
}
