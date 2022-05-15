using System;

namespace hrvster.Web.Models
{
	public class CreateSweepEntryRule
	{
		public Guid SweepstakeUid { get; set; }
		public string SweepstakeName { get; set; }
		public int SweepstakeId { get; set; }
		public int EntryPerUser { get; set; }
		public string ReturnController { get; set; }
		public string ReturnAction { get; set; }
	}
}
