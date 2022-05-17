using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class UpdateSweepEntryRuleRequest : IRequest
	{
		private Guid sweepUid;
		private int sweepId;
		private Guid entryRuleUid;
		private int entryPerUser;

		public UpdateSweepEntryRuleRequest(Guid sweepUid, int sweepId, Guid entryRuleUid, int entryPerUser)
		{
			this.sweepUid = sweepUid;
			this.sweepId = sweepId;
			this.entryRuleUid = entryRuleUid;
			this.entryPerUser = entryPerUser;
		}

		public Guid SweepUid { get { return sweepUid; } }
		public int SweepId { get { return sweepId; } }
		public Guid SweepEntryRuleUid { get { return entryRuleUid; } }
		public int SweepEntryPerUser { get { return entryPerUser; } }
	}
}
