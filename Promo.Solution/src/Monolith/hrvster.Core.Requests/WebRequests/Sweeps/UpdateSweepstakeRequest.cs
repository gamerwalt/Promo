using hrvster.Models;
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
		private Sweep _sweep;
		private string _timezoneId;

		public UpdateSweepstakeRequest(Models.Sweep sweep, Guid sweepUid, string name, DateTime startDate, DateTime endDate, DateTime drawDate, string timeZoneId)
		{
			_sweep = sweep;
			_sweepUid = sweepUid;
			_name = name;
			_startDate = startDate;
			_endDate = endDate;
			_drawDate = drawDate;
			_timezoneId = timeZoneId;
		}
		public Sweep Sweepstake { get { return _sweep; } }
		public Guid SweepUid { get { return _sweepUid; } }
		public string Name { get { return _name; } }
		public DateTime StartDate { get { return _startDate; } }
		public DateTime EndDate { get { return _endDate; } }
		public DateTime DrawDate { get { return _drawDate; } }
		public string TimeZoneId { get { return _timezoneId;} }
	}
}
