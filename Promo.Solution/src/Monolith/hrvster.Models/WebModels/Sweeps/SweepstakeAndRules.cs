using System.Collections.Generic;

namespace hrvster.Models.WebModels.Sweeps
{
	public class SweepstakeAndRules
	{
		public Sweepstake Sweepstake { get; set; }
		public IEnumerable<Rule> Rules { get; set; }
		public EntryRule EntryRule { get; set; }
		public DrawRule DrawRule { get; set; }
	}

	public class SweepstakeRulesAndPrizes : SweepstakeAndRules
	{
		public IEnumerable<SweepPrize> Prizes { get; set; }
	}
}
