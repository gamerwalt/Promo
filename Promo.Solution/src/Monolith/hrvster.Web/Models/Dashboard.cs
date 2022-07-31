using hrvster.Models.WebModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class Dashboard
	{
		public Account Account { get; set; }
		public IEnumerable<SelectListItem> Promotions { get; set; }
		public List<hrvster.Models.Campaign> Campaigns { get; internal set; }
	}
}
