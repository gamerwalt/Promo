using hrvster.Models;
using System;

namespace hrvster.Web.Models
{
	public class FulfillWinnerDetails
	{
		public Sweep Sweepstake { get; set; }
		public SweepWinner SweepWinner { get; set; }
		public SweepPrize Prize { get; set; }
		public string ReturnAction { get; set; }
		public string ReturnController { get; set; }
		public Guid SweepUid { get; set; }
		public Guid SweepWinnerUid { get; set; }
		public Guid PrizeUid { get; set; }
	}
}
