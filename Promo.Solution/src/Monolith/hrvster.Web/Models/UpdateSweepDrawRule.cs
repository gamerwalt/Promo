using System;

namespace hrvster.Web.Models
{
	public class UpdateSweepDrawRule
	{
		public Guid SweepDrawRuleUid { get; set; }
		public int SweepstakeId { get; set; }
		public Guid SweepstakeUid { get; set; }
		public string SweepstakeName { get; set; }
		public int WinnerCount { get; set; }
		public int WinnerBackupCount { get; set; }
		public string ReturnAction { get; set; }
		public string ReturnController { get; set; }
	}
}
