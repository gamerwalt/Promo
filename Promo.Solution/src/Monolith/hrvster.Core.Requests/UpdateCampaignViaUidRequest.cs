using hrvster.Core.Requests.ApiRequests;
using hrvster.Models;
using MediatR;
using System;

namespace hrvster.Core.Requests
{
	public class UpdateCampaignViaUidRequest : IRequest<Campaign>
	{
		private Guid _campaignUid;
		private Guid _accountUid;
		private string _timezoneId;
		private UpdateCampaignRequest _request;
		private bool _isDaylight;

		public UpdateCampaignViaUidRequest(Guid campaignUid, Guid accountUid, UpdateCampaignRequest request, string timezoneId, bool isDaylight)
		{
			_campaignUid = campaignUid;
			_request = request;
			_accountUid = accountUid;
			_timezoneId = timezoneId;
			_isDaylight = isDaylight;
		}

		public Guid AccountUid { get { return _accountUid; } }

		public Guid CampaignUid { get { return _campaignUid; } }

		public UpdateCampaignRequest Request { get { return _request; } }
		public string Timezone { get { return _timezoneId; } }
		public bool IsDaylight { get { return _isDaylight; } }
	}
}
