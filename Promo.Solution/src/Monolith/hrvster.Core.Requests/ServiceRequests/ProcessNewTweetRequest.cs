using hrvster.Models.Harvester;
using MediatR;
using Tweetinvi.Models;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class ProcessNewTweetRequest : IRequest
	{
		private ITweet _tweet;
		private string _json;
		private IncomingTweet _incomingTweet;

		public ProcessNewTweetRequest(ITweet tweet, string json, IncomingTweet incomingTweet)
		{
			_tweet = tweet;
			_json = json;
			_incomingTweet = incomingTweet;
		}

		public ITweet Tweet { get { return _tweet; } }

		public string Json { get { return _json; } }

		public IncomingTweet IncomingTweet { get { return _incomingTweet; } }
	}
}
