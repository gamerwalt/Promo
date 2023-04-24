using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class SelectPageToPost
	{
		public Guid GameUid { get; set; }
		public string GameName { get; set; }
		public string PageId { get; set; }
		public Guid PageUid { get; set; }
		public List<SelectListItem> Pages { get; set; }
		public string ControllerName { get; set; }
		public string ActionName { get; set; }
		public string GameType { get; set; }
	}
}
