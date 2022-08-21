using hrvster.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class CreateSweepRule
	{
		public int SweepstakeId { get; set; }
		public Guid SweepstakeUid { get; set; }
		public string SweepstakeName { get; set; }
		public List<SelectListItem> WatchTypes { get; set; }
		public int WatchTypeId { get; set; }
		public string WatchValue { get; set; }
		public string TweetId { get; set; }
		public bool Active { get; set; }
		public string ReturnAction { get; set; }
		public string ReturnController { get; set; }
		public hrvster.Models.Campaign Campaign { get; set; }
	}
}
