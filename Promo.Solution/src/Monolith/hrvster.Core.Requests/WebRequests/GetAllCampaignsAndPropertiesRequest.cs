using hrvster.Models;
using hrvster.Models.WebModels;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;

namespace hrvster.Core.Requests.WebRequests
{
	public class GetAllCampaignsAndPropertiesRequest : IRequest<IEnumerable<Promotion>>
	{
		private Guid _accountUid;

		public GetAllCampaignsAndPropertiesRequest(Guid accountUid)
		{
			_accountUid = accountUid;
		}

		public Guid AccountUid { get { return _accountUid; } }
	}
}
