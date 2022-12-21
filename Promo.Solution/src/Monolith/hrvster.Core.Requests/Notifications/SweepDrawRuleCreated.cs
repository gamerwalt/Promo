using hrvster.Models;
using MediatR;
using System;

namespace hrvster.Core.Requests.Notifications
{
	public class SweepDrawRuleCreated : INotification
	{
		private DrawRule _sweepstakeDrawRule;
		private Guid _accountUid;

		public SweepDrawRuleCreated(DrawRule sweepstakeDrawRule, Guid accountUid)
		{
			_sweepstakeDrawRule = sweepstakeDrawRule;
			_accountUid = accountUid;
		}

		public DrawRule SweepstakeDrawRule { get { return _sweepstakeDrawRule; } }
		public Guid AccountUid { get {  return _accountUid; } }
	}
}
