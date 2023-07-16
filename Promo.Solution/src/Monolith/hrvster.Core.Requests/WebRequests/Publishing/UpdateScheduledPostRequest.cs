using hrvster.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace hrvster.Core.Requests.WebRequests.Publishing
{
	public class UpdateScheduledPostRequest : IRequest
	{
		private Guid userUid;
		private Rule rule;
		private Sweep sweep;
		private Guid scheduledUid;
		private string text;
		private DateTime scheduledTime;
		private int previousMediaId1;
		private IFormFile media1;
		private int previousMediaId2;
		private IFormFile media2;
		private int previousMediaId3;
		private IFormFile media3;
		private int previousMediaId4;
		private IFormFile media4;
		private string _timeZoneId;
		private bool _isDaylight;

		public UpdateScheduledPostRequest(
			Guid userUid,
			Rule rule, 
			Sweep sweep, 
			Guid scheduledUid, 
			string text, 
			DateTime scheduledTime, 
			int previousMediaId1, 
			IFormFile media1, 
			int previousMediaId2, 
			IFormFile media2, 
			int previousMediaId3, 
			IFormFile media3, 
			int previousMediaId4, 
			IFormFile media4,
			string timeZoneId,
			bool isDaylight)
		{
			this.userUid = userUid;
			this.rule = rule;
			this.sweep = sweep;
			this.scheduledUid = scheduledUid;
			this.text = text;
			this.scheduledTime = scheduledTime;
			this.previousMediaId1 = previousMediaId1;
			this.media1 = media1;
			this.previousMediaId2 = previousMediaId2;
			this.media2 = media2;
			this.previousMediaId3 = previousMediaId3;
			this.media3 = media3;
			this.previousMediaId4 = previousMediaId4;
			this.media4 = media4;
			_timeZoneId = timeZoneId;
			_isDaylight = isDaylight;
		}

		public string TimeZoneId { get { return _timeZoneId; } }
		public Guid UserUid { get { return userUid; } }
		public Rule Rule { get { return rule; } }
		public Sweep Sweepstake { get { return sweep; } }
		public Guid ScheduledUid { get { return scheduledUid; } }
		public string Text { get { return text; } }
		public DateTime ScheduledTime { get { return scheduledTime; } }
		public int PreviousMediaId1 { get { return previousMediaId1; } }
		public int PreviousMediaId2 { get { return previousMediaId2; } }
		public int PreviousMediaId3 { get { return previousMediaId3; } }
		public int PreviousMediaId4 { get { return previousMediaId4; } }
		public IFormFile Media1 { get { return media1; } }
		public IFormFile Media2 { get { return media2; } }
		public IFormFile Media3 { get { return media3; } }
		public IFormFile Media4 { get { return media4; } }
		public bool IsDaylight { get { return _isDaylight; } }
	}
}
