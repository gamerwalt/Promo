using hrvster.Models;
using hrvster.Models.WebModels;
using System;

namespace hrvster.Extensions.ModelExtensions
{
	public static class CampaignAndPromotionExtensions
	{
		public static bool IsActive(this Campaign campaign)
		{
			return DateTime.UtcNow >= campaign.StartDate && DateTime.UtcNow <= campaign.EndDate ||
				DateTime.UtcNow <= campaign.StartDate;
		}

		public static bool IsActive(this Promotion promotion)
		{
			return DateTime.UtcNow >= promotion.StartDate && DateTime.UtcNow <= promotion.EndDate ||
				DateTime.UtcNow <= promotion.StartDate;
		}
	}
}
