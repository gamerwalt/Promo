using hrvster.Models;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class GetSweepPrizeByPrizeUid : IRequest<SweepPrize>
	{
		private Guid prizeUid;

		public GetSweepPrizeByPrizeUid(Guid prizeUid)
		{
			this.prizeUid = prizeUid;
		}

		public Guid PrizeUid
		{
			get { return prizeUid; }
		}
	}
}
