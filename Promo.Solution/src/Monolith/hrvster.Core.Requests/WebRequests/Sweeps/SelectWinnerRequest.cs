using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class SelectWinnerRequest : IRequest
	{
		private Guid _sweepUid;
		private List<SweepEntry> _distinctEntrants;
		private List<SweepWinner> _winners;
		private bool _all;

		public SelectWinnerRequest(Guid sweepUid, List<SweepEntry> distinctEntrants, List<SweepWinner> winners, bool all = false)
		{
			_sweepUid = sweepUid;
			_distinctEntrants = distinctEntrants;
			_winners = winners;
			_all = all;
		}

		public Guid SweepUid { get { return _sweepUid; } }
		public List<SweepEntry> Entrants { get { return _distinctEntrants; } }
		public List<SweepWinner> Winners { get { return _winners; } }
		public bool SelectAllPossibleWinners { get { return _all; } }
	}
}
