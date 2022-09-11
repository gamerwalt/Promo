using hrvster.Models.Profiles;
using System.Collections.Generic;

namespace hrvster.Models.WebModels.Sweeps
{
	public class SweepstakeRulesAndPrizes : SweepstakeAndRules
	{
		public IEnumerable<Models.SweepPrize> Prizes { get; set; }
		public Profile Profile { get; set; }
	}
}
