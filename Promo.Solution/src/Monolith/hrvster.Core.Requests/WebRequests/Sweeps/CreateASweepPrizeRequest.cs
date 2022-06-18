using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class CreateASweepPrizeRequest : IRequest
	{
		private Guid _sweepUid;
		private int _position;
		private string _prizeName;
		private string _prizeDescription;
		private decimal _monetaryValue;

		public CreateASweepPrizeRequest(Guid sweepUid, int position, string prizeName, string prizeDescription, decimal monetaryValue)
		{
			_sweepUid = sweepUid;
			_position = position;
			_prizeName = prizeName;
			_prizeDescription = prizeDescription;
			_monetaryValue = monetaryValue;
		}

		public Guid SweepUid { get { return _sweepUid; } }
		public int Position { get { return _position; } }
		public string PrizeName { get { return _prizeName; } }
		public string PrizeDescription { get { return _prizeDescription; } }
		public decimal MonetaryValue { get { return _monetaryValue; } }
	}
}
