using System;

namespace hrvster.Models.Sweepstakes
{
	public class SweepstakeDrawObject
	{
		public int Index { get; set; }
		public string UserIdentifier { get; set; }
		public Guid SweepEntryUid { get; set; }
	}
}
