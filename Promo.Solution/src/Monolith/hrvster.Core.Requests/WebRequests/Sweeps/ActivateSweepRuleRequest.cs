using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class ActivateSweepRuleRequest : IRequest
	{
		private Guid _sweepUid;
		private Guid _ruleUid;

		public ActivateSweepRuleRequest(Guid sweepUid, Guid ruleUid)
		{
			_sweepUid = sweepUid;
			_ruleUid = ruleUid;
		}

		public Guid SweepUid { get { return _sweepUid; } }
		public Guid RuleUid { get { return _ruleUid; } }
	}
}
