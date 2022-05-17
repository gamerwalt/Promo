using System;

namespace hrvster.Web.Models
{
	public class UpdateSweepEntryRule
	{
		public int SweepstakeId { get; set; }
		public string SweepstakeName { get; set; }
		public Guid SweepstakeUid { get; set; }
		public int EntryPerUser { get; set; }
		public Guid SweepEntryRuleUid { get; set; }
		public string ReturnAction { get; set; }
		public string ReturnController { get; set; }
	}
}
