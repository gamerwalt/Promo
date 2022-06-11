using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Publishing
{
	public class DeleteSweepTweetRuleRequest : IRequest
	{
		private Guid sweepUid;
		private Guid ruleUid;
		private Guid userUid;

		public DeleteSweepTweetRuleRequest(Guid sweepUid, Guid ruleUid, Guid userUid)
		{
			this.sweepUid = sweepUid;
			this.ruleUid = ruleUid;
			this.userUid = userUid;
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}

		public Guid RuleUid
		{
			get { return ruleUid; }
		}

		public Guid UserUid
		{
			get { return userUid; }
		}
	}
}
