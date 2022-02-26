using System;

namespace Promo.Core.MessageContracts
{
	public interface ISweepUpdated
	{
		Guid SweepUid { get; }
		string SweepName { get; }
		string RulesUrl { get; }
		DateTime StartDate { get; }
		DateTime EndDate { get; }
		DateTime DrawDate { get; }
		bool DrawnIndicator { get; }
		DateTime DateDrawn { get; }
	}

	public interface ISweepUpdatedAccepted
	{
		Guid SweepUid { get; }
	}

	public interface ISweepUpdatedRejected
	{
		Guid SweepUid { get; }
		string Reason { get; }
	}
}
