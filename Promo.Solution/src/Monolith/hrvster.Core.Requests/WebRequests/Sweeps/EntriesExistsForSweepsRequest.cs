using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class EntriesExistsForSweepsRequest : IRequest<bool>
	{
		private Guid _sweepUid;

		public EntriesExistsForSweepsRequest(Guid sweepUid)
		{
			_sweepUid = sweepUid;
		}

		public Guid SweepUid { get { return _sweepUid; } }
	}
}
