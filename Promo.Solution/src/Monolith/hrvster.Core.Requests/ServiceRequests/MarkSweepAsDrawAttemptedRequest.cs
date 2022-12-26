using MediatR;
using System;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class MarkSweepAsDrawAttemptedRequest : IRequest
	{
		private Guid _sweepUid;

		public MarkSweepAsDrawAttemptedRequest(Guid sweepUid)
		{
			_sweepUid = sweepUid;
		}

		public Guid SweepUid
		{
			get { return _sweepUid; }
		}
	}
}