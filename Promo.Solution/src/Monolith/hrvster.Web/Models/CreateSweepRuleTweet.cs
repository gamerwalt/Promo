using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class CreateSweepRuleTweet
	{
		public string WatchValue { get; set; }
		public string TweetText { get; set; }
		public int SweepstakeId { get; internal set; }
		public Guid SweepstakeUid { get; internal set; }
		public string SweepstakeName { get; internal set; }
		public string ReturnAction { get; internal set; }
		public string ReturnController { get; internal set; }
		public List<IFormFile> Medias { get; set; }
		public bool PostNow { get; set; }
		public DateTime ScheduledDate { get; set; }
	}
}
