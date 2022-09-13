using hrvster.Models;
using hrvster.Models.Harvester;
using MediatR;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class TweetProcessingStopped : INotification
	{
		private RateLimiter _rateLimiter;
		private ProcessNewTweetRequest _request;
		private IncomingTweet _incomingTweet;

		public TweetProcessingStopped(
			RateLimiter rateLimiter, 
			ProcessNewTweetRequest request,
			IncomingTweet incomingTweet)
		{
			_rateLimiter = rateLimiter;
			_request = request;
			_incomingTweet = incomingTweet;
		}

		public RateLimiter RateLimiter { get { return _rateLimiter; } }
		public ProcessNewTweetRequest Request { get { return _request; } }
		public IncomingTweet IncomingTweet { get { return _incomingTweet; } }
	}
}
