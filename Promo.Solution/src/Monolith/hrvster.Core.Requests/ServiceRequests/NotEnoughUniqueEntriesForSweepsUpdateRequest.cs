using MediatR;
using System;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class NotEnoughUniqueEntriesForSweepsUpdateRequest : IRequest
	{
		private Guid _sweepUid;

		public NotEnoughUniqueEntriesForSweepsUpdateRequest(Guid sweepUid)
		{
			_sweepUid = sweepUid;
		}

		public Guid SweepUid { get { return _sweepUid; } }
	}
}
