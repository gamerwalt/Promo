using hrvster.Models;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class GetSweepRuleByUidRequest : IRequest<Rule>
	{
		private Guid _ruleUid;

		public GetSweepRuleByUidRequest(Guid ruleUid)
		{
			_ruleUid = ruleUid;
		}

		public Guid RuleUid { get { return _ruleUid; } }
	}
}
