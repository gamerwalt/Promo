using hrvster.Models.Scheduling;
using MediatR;
using System.Collections.Generic;

namespace hrvster.Services.Requests
{
	public class PublishScheduledPostsRequest : IRequest
	{
		private IEnumerable<ScheduledPost> _scheduledTweets;

		public PublishScheduledPostsRequest(IEnumerable<ScheduledPost> scheduledTweets)
		{
			_scheduledTweets = scheduledTweets;
		}

		public IEnumerable<ScheduledPost> ScheduledSweepRuleTweets
		{
			get { return _scheduledTweets; }
		}
	}
}
