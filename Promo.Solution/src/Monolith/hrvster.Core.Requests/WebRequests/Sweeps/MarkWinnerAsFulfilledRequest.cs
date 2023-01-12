using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class MarkWinnerAsFulfilledRequest : IRequest
	{
		private Sweep _sweep;
		private SweepPrize _prize;
		private SweepWinner _sweepWinner;

		public MarkWinnerAsFulfilledRequest(Sweep sweep, SweepPrize prize, SweepWinner sweepWinner)
		{
			_sweep = sweep;
			_prize = prize;
			_sweepWinner = sweepWinner;
		}

		public Sweep Sweepstake { get { return _sweep; } }
		public SweepPrize SweepstakePrize { get { return _prize; } }
		public SweepWinner SweepWinner { get { return _sweepWinner; } }
	}
}
