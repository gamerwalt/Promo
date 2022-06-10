using hrvster.Models.Scheduling;
using MediatR;
using System.Collections.Generic;

namespace hrvster.Services.Requests
{
	public class PublishScheduledTweetsRequest : IRequest
	{
		private IEnumerable<ScheduledSweepRuleTweet> _scheduledTweets;

		public PublishScheduledTweetsRequest(IEnumerable<ScheduledSweepRuleTweet> scheduledTweets)
		{
			_scheduledTweets = scheduledTweets;
		}

		public IEnumerable<ScheduledSweepRuleTweet> ScheduledSweepRuleTweets
		{
			get { return _scheduledTweets; }
		}
	}
}
