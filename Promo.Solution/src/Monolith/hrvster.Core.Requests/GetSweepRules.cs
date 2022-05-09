using hrvster.Models.WebModels.Sweeps;
using MediatR;
using System;

namespace hrvster.Core.Requests
{
	public class GetSweepRules : IRequest<SweepRule>
	{
		private Guid sweepUid;

		public GetSweepRules(Guid sweepUid)
		{
			this.sweepUid = sweepUid;
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}
	}
}
