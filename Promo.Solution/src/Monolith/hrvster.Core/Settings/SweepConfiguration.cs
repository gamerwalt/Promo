namespace hrvster.Core.Settings
{
	public class SweepConfiguration
	{
		public const string SectionName = "SweepstakeConfigurationOptions";

		//Entry Rule Defaults
		public int EntryPerUser { get; set; } = 0;

		//Draw Rule Defaults
		public int WinnerCount { get; set; } = 0;
		public int WinnerBackupCount { get; set; } = 0;
	}
}
