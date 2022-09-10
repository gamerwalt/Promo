using hrvster.Models.WebModels.Sweeps;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class GetSweepPrizesBySweepUidRequest : IRequest<SweepPrize>
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
