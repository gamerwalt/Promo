using hrvster.Models.WebModels.Sweeps;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class GetSweepEntryRuleByUid : IRequest<EntryRule>
	{
		private Guid sweepEntryUid;

		public GetSweepEntryRuleByUid(Guid sweepEntryUid)
		{
			this.sweepEntryUid = sweepEntryUid;
		}

		public Guid SweepEntryUid
		{
			get { return sweepEntryUid; }
		}
	}
}
