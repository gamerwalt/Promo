using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Promotions
{
	public class CreatePromotionRequest : IRequest
	{
		private Guid _promotionUid;
		private string _name;
		private string _description;
		private Guid _campaignTypeUid;
		private DateTime _startDate;
		private DateTime _endDate;
		private Guid _accountUid;

		public CreatePromotionRequest(
			Guid promotionUid,
			string name,
			string description,
			Guid campaignTypeUid,
			DateTime startDate,
			DateTime endDate,
			Guid userUid)
		{
			_promotionUid = promotionUid;
			_name = name;
			_description = description;
			_campaignTypeUid = campaignTypeUid;
			_startDate = startDate;
			_endDate = endDate;
			_accountUid = userUid;
		}

		public Guid PromotionUid { get { return _promotionUid; } }
		public string Name { get { return _name; } }
		public string Description { get { return _description; } }
		public Guid CampaignTypeUid { get { return _campaignTypeUid; } }
		public DateTime StartDate { get { return _startDate; } }
		public DateTime EndDate { get { return _endDate; } }
		public Guid AccountUid { get { return _accountUid; } }
	}
}
