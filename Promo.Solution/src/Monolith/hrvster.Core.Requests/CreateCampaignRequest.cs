using hrvster.Core.Requests.ApiRequests;
using MediatR;
using System;
using System.Linq;
using System.Security.Claims;

namespace hrvster.Core.Requests
{
	public class CreateCampaignRequest : IRequest<Guid>
	{
		private NewCampaignRequest request;
		private ClaimsPrincipal user;
		private Guid accountUid;
		private string timezoneId;
		private bool isDaylight;

		public CreateCampaignRequest(NewCampaignRequest request, ClaimsPrincipal user, string timezoneId, bool isDaylight)
		{
			this.request = request;
			this.user = user;
			this.timezoneId = timezoneId;
			this.isDaylight = isDaylight;
		}

		public CreateCampaignRequest(NewCampaignRequest newCampaignRequest, Guid userUid, string timezoneId)
		{
			this.request = newCampaignRequest;
			this.accountUid = userUid;
			this.timezoneId = timezoneId;
			this.isDaylight = isDaylight;
		}

		public NewCampaignRequest NewCampaign
		{
			get { return request; }
		}

		public string TimezoneId { get { return timezoneId; } }

		public Guid AccountUid
		{
			get {
				return
					user == null
					? this.accountUid
					: Guid.Parse(user.Claims.FirstOrDefault(c => c.Type.Equals("user_id", StringComparison.OrdinalIgnoreCase)).Value);
			}
		}
		public bool IsDaylight { get { return isDaylight; } }
	}
}
