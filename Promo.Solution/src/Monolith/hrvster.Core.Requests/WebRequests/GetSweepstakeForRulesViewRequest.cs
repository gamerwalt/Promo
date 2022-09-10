using hrvster.Models.WebModels.Sweeps;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests
{
	public class GetSweepstakeForRulesViewRequest : IRequest<SweepstakeRulesAndPrizes>
	{
		private Guid _sweepUid;

		public GetSweepstakeForRulesViewRequest(Guid sweepUid)
		{
			_sweepUid = sweepUid;
		}

		public Guid SweepUid { get { return _sweepUid; } }
	}
}
