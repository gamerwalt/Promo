using hrvster.Models;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps.Updates
{
	public class UpdateEntryRuleRequest : IRequest
	{
		private Sweep _sweep;
		private Guid _entryRuleUid;
		private int _entryPerUser;

		public UpdateEntryRuleRequest(Sweep sweep, Guid entryRuleUid, int entryPerUser)
		{
			_sweep = sweep;
			_entryRuleUid = entryRuleUid;
			_entryPerUser = entryPerUser;
		}

		public Sweep Sweep { get { return _sweep; } }
		public Guid EntryRuleUid { get { return _entryRuleUid; } }
		public int EntryPerUser { get { return _entryPerUser; } }
	}
}
