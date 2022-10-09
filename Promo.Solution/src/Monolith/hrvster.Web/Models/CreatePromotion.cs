using hrvster.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hrvster.Web.Models
{
	public class CreatePromotion
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public List<SelectListItem> CampaignTypes { get; set; }
		public Guid CampaignTypeUid { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
