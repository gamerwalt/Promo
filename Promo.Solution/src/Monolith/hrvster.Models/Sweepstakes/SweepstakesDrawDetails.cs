using System.Collections.Generic;

namespace hrvster.Models.Sweepstakes
{
	public class SweepstakesDrawDetails
	{
		public SweepDrawPropertiesAndRule SweepDrawPropertiesAndRule { get; set; }
		public List<SweepEntry> SweepEntries { get; set; }
		public int FirstEntryId { get; set; }
		public int LastEntryId { get; set; }
		public List<int> MissingBetween { get; set; }
	}
}
