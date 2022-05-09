using System;

namespace hrvster.Models.WebModels.Sweeps
{
	public class DrawRule
	{
		public Guid DrawRuleUid { get; set; }
		public int WinnerCount { get; set; }
		public int WinnerBackupCount { get; set; }
	}
}
