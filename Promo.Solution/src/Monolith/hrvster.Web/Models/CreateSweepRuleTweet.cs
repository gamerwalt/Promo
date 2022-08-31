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
		public int SweepstakeId { get; set; }
		public Guid SweepstakeUid { get; set; }
		public string SweepstakeName { get; set; }
		public string ActionName { get; set; }
		public string ControllerName { get; set; }
		public IFormFile Media1 { get; set; }
		public IFormFile Media2 { get; set; }
		public IFormFile Media3 { get; set; }
		public IFormFile Media4 { get; set; }
		public bool PostNow { get; set; }
		public DateTime ScheduledDate { get; set; }
		public bool MustBeFollowing { get; set; }
		public DateTime SweepstakeStartDate { get; set; }
		public DateTime SweepstakeEndDate { get; set; }
		public DateTime? SweepstakeDrawDate { get; set; }
	}
}
