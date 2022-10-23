using hrvster.Models;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps.Gets
{
	public class GetSweepEntryRuleForSweepRequest : IRequest<SweepEntryRule>
	{
		private Guid _sweepUid;

		public GetSweepEntryRuleForSweepRequest(Guid sweepUid)
		{
			_sweepUid = sweepUid;
		}

		public Guid SweepUid { get { return _sweepUid; } }
	}
}
