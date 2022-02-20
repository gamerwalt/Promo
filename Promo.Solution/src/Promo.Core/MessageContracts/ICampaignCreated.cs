using System;

namespace Promo.Core.MessageContracts
{
	public interface ICampaignCreated
	{
		Guid CampaignUid { get; }
		Guid AccountUid { get; }
		string CampaignName { get; }
		CampaignTypeCode CampaignTypeCode { get; }
		bool ActiveInd { get; }
		DateTime StartDate { get; }
		DateTime EndDate { get; }
	}

	public interface ICampaignCreatedAccepted
	{
		Guid CampaignUid { get; }
	}

	public interface ICampaignCreatedRejected
	{
		string Reason { get; }
	}
}
