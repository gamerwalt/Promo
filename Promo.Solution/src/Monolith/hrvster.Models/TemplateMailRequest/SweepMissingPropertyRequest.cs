namespace hrvster.Models.TemplateMailRequest
{
	public class SweepMissingPropertyRequest
	{
		public string EmailAddress { get; set; }
		public string SweepstakeName { get; set; }
		public string DisplayName { get; set; }
		public string DashboardLink { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public string RulesCount { get; set; }
		public string RulesList { get; set; }
		public string PrizesCount { get; set; }
		public string PrizeList { get; set; }
		public string WinnerCount { get; set; }
		public string WinnerBackupCount { get; set; }
		public string EntryPerUser { get; set; }

		public EmailTemplate EmailTemplate { get; set; }
	}
}
