using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class PromoteNextBackupWinnerRequest : IRequest
	{
		private Sweep _sweep;
		private SweepPrize _prize;
		private SweepWinner _sweepWinner;
		private string _reason;

		public PromoteNextBackupWinnerRequest(Sweep sweep, SweepPrize prize, SweepWinner sweepWinner, string reason)
		{
			_sweep = sweep;
			_prize = prize;
			_sweepWinner = sweepWinner;
			_reason = reason;
		}

		public Sweep Sweepstake { get { return _sweep; } }
		public SweepPrize Prize { get { return _prize; } }
		public SweepWinner Winner { get { return _sweepWinner; } }
		public string Reason { get { return _reason; } }
	}
}
