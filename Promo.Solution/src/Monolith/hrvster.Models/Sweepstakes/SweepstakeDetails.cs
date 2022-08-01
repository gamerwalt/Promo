using System.Collections.Generic;

namespace hrvster.Models.Sweepstakes
{
	public class SweepstakeDetails
	{
		public List<SweepDetail> SweepDetails { get; set; }
	}

	public class SweepDetail
	{
		public Sweep Sweepstake { get; set; }
		public List<SweepEntry> SweepEntries { get; set; }
		public List<SweepTweet> TweetsHarvested { get; set; }
	}
}
