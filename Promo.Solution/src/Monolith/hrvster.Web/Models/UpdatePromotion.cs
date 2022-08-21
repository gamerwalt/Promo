using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class UpdatePromotion
	{
		public Guid CampaignUid { get; set; }
		public string CampaignName { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public List<SelectListItem> CampaignTypes { get; set; }
		public Guid CampaignTypeUid { get; set; }
	}
}
