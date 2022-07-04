using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class DeleteSweepPrizeRequest : IRequest
	{
		private Guid prizeUid;

		public DeleteSweepPrizeRequest(Guid prizeUid)
		{
			this.prizeUid = prizeUid;
		}

		public Guid PrizeUid
		{
			get { return prizeUid; }
		}
	}
}
