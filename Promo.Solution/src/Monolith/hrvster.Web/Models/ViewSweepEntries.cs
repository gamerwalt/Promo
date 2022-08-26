using hrvster.Models;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class ViewSweepEntries
	{
		public IEnumerable<SweepEntry> SweepEntries { get; internal set; }
		public Sweep Sweepstake { get; internal set; }
		public Rule Rule { get; internal set; }
	}
}
