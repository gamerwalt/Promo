using System;

namespace Promo.Core.MessageContracts
{
	public interface ISweepCreated
	{
		Guid SweepUid { get; }
		Guid CampaignUid { get; }
		Guid AccountUid { get; }
		string SweepName { get; }
		string RulesUrl { get; }
		DateTime StartDate { get; }
		DateTime EndDate { get; }
		DateTime DrawDate { get; }
	}

	public interface ISweepCreatedAccepted
	{
		Guid SweepUid { get; }
	}

	public interface ISweepCreatedRejected
	{
		Guid SweepUid { get; }
		string Reason { get; }
	}
}
