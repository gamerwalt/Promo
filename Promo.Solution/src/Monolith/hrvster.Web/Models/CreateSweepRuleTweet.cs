using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class CreateSweepRuleTweet
	{
		public List<SelectListItem> WatchTypes { get; set; }
		public int WatchTypeId { get; set; }
		public string WatchValue { get; set; }
		public string TweetText { get; set; }
		public int SweepstakeId { get; internal set; }
		public Guid SweepstakeUid { get; internal set; }
		public string SweepstakeName { get; internal set; }
		public string ReturnAction { get; internal set; }
		public string ReturnController { get; internal set; }
		public IFormFile Media1 { get; set; }
		public IFormFile Media2 { get; set; }
		public IFormFile Media3 { get; set; }
		public IFormFile Media4 { get; set; }
		public bool PostNow { get; set; }
		public DateTime ScheduledDate { get; set; }
	}
}
