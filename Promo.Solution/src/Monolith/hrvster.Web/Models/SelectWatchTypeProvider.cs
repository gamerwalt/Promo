using hrvster.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class SelectWatchTypeProvider
	{
		public IEnumerable<SelectListItem> Providers { get; set; }
		public int WatchTypeProviderId { get; set; }
		public Campaign Campaign { get; set; }
		public Sweep Sweep { get; set; }
	}
}
