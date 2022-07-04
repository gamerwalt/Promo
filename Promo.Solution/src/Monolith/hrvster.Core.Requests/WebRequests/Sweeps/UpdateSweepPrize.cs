using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class UpdateSweepPrize : IRequest
	{
		private Guid sweepUid;
		private Guid prizeUid;
		private string prizeName;
		private string prizeDescription;
		private decimal monetaryValue;

		public UpdateSweepPrize(Guid sweepUid, Guid prizeUid, string prizeName, string prizeDescription, decimal monetaryValue)
		{
			this.sweepUid = sweepUid;
			this.prizeUid = prizeUid;
			this.prizeName = prizeName;
			this.prizeDescription = prizeDescription;
			this.monetaryValue = monetaryValue;
		}

		public Guid SweepUid {  get { return sweepUid; } }
		public Guid PrizeUid { get { return prizeUid; } }
		public string PrizeName { get { return prizeName; } }
		public string PrizeDescription { get { return prizeDescription; } }
		public decimal MonetaryValue { get { return monetaryValue; } }
	}
}
