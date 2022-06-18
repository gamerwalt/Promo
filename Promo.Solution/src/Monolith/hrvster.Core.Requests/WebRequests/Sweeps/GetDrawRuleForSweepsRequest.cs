using hrvster.Models.WebModels.Sweeps;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class GetDrawRuleForSweepsRequest : IRequest<DrawRule>
	{
		private Guid sweepUid;

		public GetDrawRuleForSweepsRequest(Guid sweepUid)
		{
			this.sweepUid = sweepUid;
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}
	}
}
