using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class CreateSweepRequest : IRequest
	{
		private string _name;
		private DateTime _startDate;
		private DateTime _endDate;
		private DateTime _drawDate;
		private Guid _campaignUid;
		private Guid _accountUid;
		private Guid _sweepUid;
		private string _timezoneId;

		public CreateSweepRequest(string name, DateTime startDate, DateTime endDate, DateTime drawDate, Guid campaignUid, Guid accountUid, string timezoneId)
		{
			_name = name;
			_startDate = startDate;
			_endDate = endDate;
			_drawDate = drawDate;
			_campaignUid = campaignUid;
			_accountUid = accountUid;
			_timezoneId = timezoneId;
		}

		public CreateSweepRequest(Guid sweepUid, string name, DateTime startDate, DateTime endDate, DateTime drawDate, Guid campaignUid, Guid userUid, string timezoneId)
		{
			_name = name;
			_startDate = startDate;
			_endDate = endDate;
			_drawDate = drawDate;
			_campaignUid = campaignUid;
			_accountUid = userUid;
			_sweepUid = sweepUid;
			_timezoneId = timezoneId;
		}

		public string Name { get { return _name; } }
		public DateTime StartDate { get { return _startDate; } }
		public DateTime EndDate { get { return _endDate; } }
		public DateTime DrawDate { get { return _drawDate; } }
		public Guid CampaignUid { get { return _campaignUid; } }
		public Guid AccountUid { get { return _accountUid; } }
		public Guid SweepUid { get { return _sweepUid; } }
		public string TimeZoneId { get { return _timezoneId;} }
	}
}
