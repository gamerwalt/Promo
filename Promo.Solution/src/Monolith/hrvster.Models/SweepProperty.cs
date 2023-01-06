using System.Collections.Generic;

namespace hrvster.Models
{
	public class SweepProperty
	{
		public IEnumerable<SweepPrize> Prizes { get; set; }
		public DrawRule DrawRule { get; set; }
		public IEnumerable<Rule> Rules { get; set; }
		public SweepEntryRule EntryRule { get; set; }
	}
}
