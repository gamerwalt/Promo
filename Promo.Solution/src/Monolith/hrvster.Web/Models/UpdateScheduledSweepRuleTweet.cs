using hrvster.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class UpdateScheduledSweepRuleTweet
	{
		public int RuleId { get; set; }
		public Guid ScheduledUid { get; set; }
		public string Text { get; set; }
		public DateTime ScheduledTime { get; set; }
		public IFormFile Media1 { get; set; }
		public int PreviousMediaId1 { get; set; }
		public IFormFile Media2 { get; set; }
		public int PreviousMediaId2 { get; set; }
		public IFormFile Media3 { get; set; }
		public int PreviousMediaId3 { get; set; }
		public IFormFile Media4 { get; set; }
		public int PreviousMediaId4 { get; set; }
		public Sweep Sweepstake { get; internal set; }
		public Rule Rule { get; internal set; }
		public int ScheduledId { get; internal set; }
		public ScheduledStatus Status { get; internal set; }
		public List<string> Images { get; internal set; }
		public DateTime ScheduledTimeUtc { get; internal set; }
		public string ActionName { get; internal set; }
		public string ControllerName { get; internal set; }
	}
}
