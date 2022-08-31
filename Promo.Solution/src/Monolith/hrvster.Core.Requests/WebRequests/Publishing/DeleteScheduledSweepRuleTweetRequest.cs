using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Publishing
{
	public class DeleteScheduledSweepRuleTweetRequest : IRequest
	{
		private Guid _scheduledUid;
		private Guid _ruleUid;

		public DeleteScheduledSweepRuleTweetRequest(Guid scheduledUid, Guid ruleUid)
		{
			_scheduledUid = scheduledUid;
			_ruleUid = ruleUid;
		}

		public Guid ScheduledUid { get { return _scheduledUid; } }
		public Guid RuleUid { get { return _ruleUid; } }
	}
}
