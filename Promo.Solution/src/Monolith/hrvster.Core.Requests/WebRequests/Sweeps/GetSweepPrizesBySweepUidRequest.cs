using hrvster.Web.Models;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class GetSweepPrizesBySweepUidRequest : IRequest<SweepPrizes>
	{
		private Guid _sweepUid;

		public GetSweepPrizesBySweepUidRequest(Guid sweepUid)
		{
			_sweepUid = sweepUid;
		}

		public Guid SweepUid
		{
			get { return _sweepUid; }
		}
	}
}
