using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class EntriesExistsForSweepsAndRuleRequest : IRequest<bool>
	{
		private readonly Guid _ruleUid;
		private readonly Guid _sweepUid;

		public EntriesExistsForSweepsAndRuleRequest(Guid ruleUid, Guid sweepUid)
		{
			_ruleUid = ruleUid;
			_sweepUid = sweepUid;
		}

		public Guid RuleUid { get { return _ruleUid; } }
		public Guid SweepUid { get { return _sweepUid; } }
	}
}
