using hrvster.Models.WebModels;
using System;

namespace hrvster.Models.Extensions
{
	public static class CampaignAndPromotionExtensions
	{
		public static bool IsActive(this Campaign campaign)
		{
			return DateTime.UtcNow >= campaign.StartDateUtc && DateTime.UtcNow <= campaign.EndDateUtc;
		}

		public static bool WillBeInProgress(this Campaign campaign)
		{
			return DateTime.UtcNow <= campaign.StartDateUtc;
		}

		public static bool IsActive(this Promotion promotion)
		{
			return DateTime.UtcNow >= promotion.StartDateUtc &&
				DateTime.UtcNow <= promotion.EndDateUtc;
		}

		public static bool WillBeInProgress(this Promotion promotion)
		{
			return DateTime.UtcNow <= promotion.StartDateUtc;
		}
	}
}
