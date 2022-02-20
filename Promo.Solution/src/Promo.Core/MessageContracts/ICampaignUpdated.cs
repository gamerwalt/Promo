using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Core.MessageContracts
{
	public interface ICampaignUpdated
	{
		Guid CampaignUid { get; }
		string CampaignName { get; }
		CampaignTypeCode CampaignTypeCode { get; }
		bool ActiveInd { get; }
		DateTime StartDate { get; }
		DateTime EndDate { get; }
	}

	public interface ICampaignUpdatedAccepted
	{
		Guid CampaignUid { get; }
	}

	public interface ICampaignUpdatedRejected
	{
		string Reason { get; }
	}
}
