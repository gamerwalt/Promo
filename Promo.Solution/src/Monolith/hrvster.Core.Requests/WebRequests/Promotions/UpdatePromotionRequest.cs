using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Promotions
{
	public class UpdatePromotionRequest : IRequest
	{
		private Guid _campaignUid;
		private string _campaignName;
		private string _description;
		private Guid _campaignTypeUid;
		private DateTime _startDate;
		private DateTime _endDate;
		private string _timezone;
		private bool _isDaylight;

		public UpdatePromotionRequest(
			Guid campaignUid,
			string campaignName,
			string description,
			Guid campaignTypeUid,
			DateTime startDate,
			DateTime endDate,
			string timezone,
			bool isDaylight)
		{
			_campaignUid = campaignUid;
			_campaignName = campaignName;
			_description = description;
			_campaignTypeUid = campaignTypeUid;
			_startDate = startDate;
			_endDate = endDate;
			_timezone = timezone;
			_isDaylight = isDaylight;
		}

		public Guid CampaignUid { get { return _campaignUid; } }
		public string CampaignName { get { return _campaignName; } }
		public string Description { get { return _description; } }
		public Guid CampaignTypeUid { get { return _campaignTypeUid; } }
		public DateTime StartDate { get { return _startDate; } }
		public DateTime EndDate { get { return _endDate; } }
		public string TimeZone { get { return _timezone;} }
		public bool IsDaylight { get { return _isDaylight;} }
	}
}
