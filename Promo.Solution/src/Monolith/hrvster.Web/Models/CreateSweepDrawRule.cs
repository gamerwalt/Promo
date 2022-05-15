using System;

namespace hrvster.Web.Models
{
	public class CreateSweepDrawRule
	{
		public Guid SweepstakeUid { get; set; }
		public string SweepstakeName { get; set; }
		public int SweepstakeId { get; set; }
		public int WinnerCount { get; set; }
		public int WinnerBackupCount { get; set; }
		public string ReturnController { get; set; }
		public string ReturnAction { get; set; }
	}
}
