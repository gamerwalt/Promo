using hrvster.Models;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class ViewSweepPrizes
	{
		public IEnumerable<SweepPrize> Prizes { get; set; }
		public Sweep Sweep { get; set; }
		public int WinnerCount { get; internal set; }
	}
}
