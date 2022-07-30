using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace hrvster.Core.Requests.WebRequests
{
	public class GetSweepEntriesByCampaignRequest : IRequest<IEnumerable<SweepEntryChart>>
	{
		private Guid _campaignUid;
		private Guid _userUid;

		public GetSweepEntriesByCampaignRequest(Guid campaignUid, Guid userUid)
		{
			_campaignUid = campaignUid;
			_userUid = userUid;
		}

		public Guid CampaignUid
		{
			get { return _campaignUid; }
		}

		public Guid UserUid
		{
			get { return _userUid; }
		}
	}
}
