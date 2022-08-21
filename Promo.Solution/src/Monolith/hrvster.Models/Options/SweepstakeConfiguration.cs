namespace hrvster.Models.Options
{
	public class SweepstakeConfiguration
	{
		public const string SectionName = "SweepstakeConfigurationOptions";

		//Entry Rule Defaults
		public int EntryPerUser { get; set; } = 0;

		//Draw Rule Defaults
		public int WinnerCount { get; set; } = 0;
		public int WinnerBackupCount { get; set; } = 0;
		public int EndDateDaysAfterStartDate { get; set; }
		public int DrawDateDaysAfterEndDate { get; set; }

		//Tweet Scheduling
		public int TweetScheduleDefaultMinutes { get; set; }
	}
}
