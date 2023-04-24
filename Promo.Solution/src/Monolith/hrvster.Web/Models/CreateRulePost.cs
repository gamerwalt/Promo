using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class CreateRulePost
	{
		public List<SelectListItem> WatchTypes { get; set; }
		public int WatchTypeId { get; set; }
		public string WatchValue { get; set; }
		public string Post { get; set; }
		public string GameType { get; set; }
		public int GameId { get; set; }
		public Guid GameUid { get; set; }
		public string GameName { get; set; }
		public string ActionName { get; set; }
		public string ControllerName { get; set; }
		public IFormFile Media1 { get; set; }
		public IFormFile Media2 { get; set; }
		public IFormFile Media3 { get; set; }
		public IFormFile Media4 { get; set; }
		public bool PostNow { get; set; }
		public DateTime ScheduledDate { get; set; }
		public bool MustBeFollowing { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime? DrawDate { get; set; }
		public string PageId { get; set; }
	}
}
