using hrvster.Models;
using System;

namespace hrvster.Web.Models
{
	public class PromoteBackupDetails
	{
		public Sweep Sweepstake { get; internal set; }
		public SweepWinner SweepWinner { get; internal set; }
		public string ReturnAction { get; internal set; }
		public string ReturnController { get; internal set; }
		public SweepPrize Prize { get; internal set; }
		public Guid SweepUid { get; set; }
		public Guid SweepWinnerUid { get; set; }
		public Guid PrizeUid { get; set; }
		public string Reason { get; set; }
	}
}
