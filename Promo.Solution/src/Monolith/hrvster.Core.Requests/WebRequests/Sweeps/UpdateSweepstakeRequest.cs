using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class UpdateSweepstakeRequest : IRequest
	{
		private Guid _sweepUid;
		private string _name;
		private DateTime _startDate;
		private DateTime _endDate;
		private DateTime _drawDate;

		public UpdateSweepstakeRequest(Guid sweepUid, string name, DateTime startDate, DateTime endDate, DateTime drawDate)
		{
			_sweepUid = sweepUid;
			_name = name;
			_startDate = startDate;
			_endDate = endDate;
			_drawDate = drawDate;
		}
		public Guid SweepUid { get { return _sweepUid; } }
		public string Name { get { return _name; } }
		public DateTime StartDate { get { return _startDate; } }
		public DateTime EndDate { get { return _endDate; } }
		public DateTime DrawDate { get { return _drawDate; } }
	}
}
