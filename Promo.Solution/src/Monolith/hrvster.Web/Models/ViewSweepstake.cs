using hrvster.Models.WebModels.Sweeps;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class ViewSweepstake
	{
		public Sweepstake Sweepstake { get; set; }
		public IEnumerable<Rule> Rules { get; set; }
		public EntryRule EntryRule { get; set; }
		public DrawRule DrawRule { get; set; }
		public IEnumerable<SweepEntry> SweepEntries { get; set; }
		public IEnumerable<Prize> Prizes { get; set; }
		public IEnumerable<Winner> Winners { get; set; }
		public UpdateSweepstake UpdateSweepstake { get; set; }
	}
}
