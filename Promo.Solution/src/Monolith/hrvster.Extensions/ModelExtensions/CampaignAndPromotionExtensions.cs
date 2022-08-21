using hrvster.Models;
using hrvster.Models.WebModels;
using System;

namespace hrvster.Extensions.ModelExtensions
{
	public static class CampaignAndPromotionExtensions
	{
		public static bool IsActive(this Campaign campaign)
		{
			return DateTime.UtcNow >= campaign.StartDate.ToUniversalTime() && DateTime.UtcNow <= campaign.EndDate.ToUniversalTime();
		}

		public static bool WillBeInProgress(this Campaign campaign)
		{
			return DateTime.UtcNow <= campaign.StartDate.ToUniversalTime();
		}

		public static bool IsActive(this Promotion promotion)
		{
			return DateTime.UtcNow >= promotion.StartDate.ToUniversalTime() && 
				DateTime.UtcNow <= promotion.EndDate.ToUniversalTime();
		}

		public static bool WillBeInProgress(this Promotion promotion)
		{
			return DateTime.UtcNow <= promotion.StartDate.ToUniversalTime();
		}
	}
}
