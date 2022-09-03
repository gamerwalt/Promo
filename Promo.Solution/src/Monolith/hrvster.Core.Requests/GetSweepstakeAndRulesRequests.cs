using hrvster.Models.WebModels.Sweeps;
using MediatR;
using System;

namespace hrvster.Core.Requests
{
	public class GetSweepstakeAndRulesRequests : IRequest<SweepstakeAndRules>
	{
		private Guid sweepUid;

		public GetSweepstakeAndRulesRequests(Guid sweepUid)
		{
			this.sweepUid = sweepUid;
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}
	}
}
