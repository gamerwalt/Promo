using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class GetSweepEntriesRequest : IRequest<IEnumerable<SweepEntry>>
	{
		private Guid _sweepUid;

		public GetSweepEntriesRequest(Guid sweepUid)
		{
			_sweepUid = sweepUid;
		}

		public Guid SweepUid
		{
			get { return _sweepUid; }
		}
	}
}
