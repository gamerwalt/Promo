using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps.Updates
{
	public class ToggleSweepstakesDrawAttemptedRequest : IRequest
	{
		private Guid _sweepUid;
		private bool _value;

		public ToggleSweepstakesDrawAttemptedRequest(Guid sweepUid, bool value)
		{
			_sweepUid = sweepUid;
			_value = value;
		}

		public Guid SweepUid { get { return _sweepUid; } }
		public bool Value { get { return _value; } }
	}
}
