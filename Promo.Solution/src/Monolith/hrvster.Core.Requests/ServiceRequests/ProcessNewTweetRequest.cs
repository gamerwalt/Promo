using hrvster.Models.Harvester;
using hrvster.Models.Twitter;
using MediatR;
using Tweetinvi.Models;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class ProcessNewTweetRequest : IRequest
	{
		private Tweet _tweet;
		private string _json;
		private IncomingTweet _incomingTweet;

		public ProcessNewTweetRequest(Tweet tweet, string json, IncomingTweet incomingTweet)
		{
			_tweet = tweet;
			_json = json;
			_incomingTweet = incomingTweet;
		}

		public Tweet Tweet { get { return _tweet; } }

		public string Json { get { return _json; } }

		public IncomingTweet IncomingTweet { get { return _incomingTweet; } }
	}
}
