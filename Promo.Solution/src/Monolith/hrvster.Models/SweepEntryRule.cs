using System;

namespace hrvster.Models
{
	public class SweepEntryRule : EntryRule
	{
		public Guid SweepUid { get; set; }
		public int SweepId { get; set; }
		public int EntryPerUser { get; set; }
	}
}
