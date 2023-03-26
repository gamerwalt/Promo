using hrvster.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class SelectWatchTypeProvider
	{
		public IEnumerable<SelectListItem> Providers { get; set; }
		public string ProviderCode { get; set; }
		public hrvster.Models.Campaign Campaign { get; set; }
		public Sweep Sweep { get; set; }
		public Guid Uid { get; set; }
		public string ReturnController { get; set; }
		public string ReturnAction { get; set; }
		public string GameType { get; internal set; }
	}
}
