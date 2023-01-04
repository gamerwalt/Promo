using MediatR;
using System;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class UpdateSweepsAsNotificationGoingLiveSentRequest : IRequest
	{
		private Guid _sweepUid;

		public UpdateSweepsAsNotificationGoingLiveSentRequest(Guid sweepUid)
		{
			_sweepUid = sweepUid;
		}

		public Guid SweepUid { get { return _sweepUid; } }
	}
}
