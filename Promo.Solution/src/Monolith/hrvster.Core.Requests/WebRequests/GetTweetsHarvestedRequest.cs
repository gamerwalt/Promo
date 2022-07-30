using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace hrvster.Core.Requests.WebRequests
{
	public class GetTweetsHarvestedRequest : IRequest<IEnumerable<HarvestedTweet>>
	{
		private Guid _userUid;
		private Guid _campaignUid;

		public GetTweetsHarvestedRequest(Guid userUid, Guid campaignUid)
		{
			_userUid = userUid;
			_campaignUid = campaignUid;
		}

		public Guid UserUid
		{
			get { return _userUid; }
		}

		public Guid CampaignUid
		{
			get { return _campaignUid; }
		}
	}
}
