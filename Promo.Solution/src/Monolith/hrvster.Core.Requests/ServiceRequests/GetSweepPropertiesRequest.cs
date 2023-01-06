using hrvster.Models;
using hrvster.Models.Sweepstakes;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class GetSweepPropertiesRequest : IRequest<SweepProperty>
	{
		private Guid _sweepUid;

		public GetSweepPropertiesRequest(Guid sweepUid)
		{
			_sweepUid = sweepUid;
		}

		public Guid SweepUid { get { return _sweepUid; } }
	}
}
