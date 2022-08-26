using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class GetSweepRuleEntriesRequest : IRequest<IEnumerable<SweepEntry>>
	{
		private Guid _sweepUid;
		private Guid _ruleUid;

		public GetSweepRuleEntriesRequest(Guid sweepUid, Guid ruleUid)
		{
			_sweepUid = sweepUid;
			_ruleUid = ruleUid;
		}

		public Guid SweepUid { get { return _sweepUid; } }
		public Guid RuleUid { get { return _ruleUid; } }
	}
}
