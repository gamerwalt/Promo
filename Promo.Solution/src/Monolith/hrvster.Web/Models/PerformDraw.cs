using hrvster.Models;
using hrvster.Models.WebModels.Sweeps;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class PerformDrawManually
	{
		public List<hrvster.Models.SweepEntry> SweepstakeEntrants { get; set; }
		public SweepstakeAndRules SweepstakeAndRules { get; set; }
		public List<SweepWinner> Winners { get; set; }
	}
}
