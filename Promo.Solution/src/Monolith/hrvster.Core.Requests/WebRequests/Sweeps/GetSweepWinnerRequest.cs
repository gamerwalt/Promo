using hrvster.Models;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class GetSweepWinnerRequest : IRequest<SweepWinner>
	{
		private Guid _sweepWinnerUid;

		public GetSweepWinnerRequest(Guid sweepWinnerUid)
		{
			_sweepWinnerUid = sweepWinnerUid;
		}

		public Guid SweepWinnerUid { get { return _sweepWinnerUid; } }
	}
}
