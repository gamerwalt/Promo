using hrvster.Models.Harvester;
using hrvster.Models.Profiles;
using MediatR;
using Tweetinvi.Models;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class NotifyUserOfNotFollowingProfileRequest : IRequest
	{
		private ITweet _tweet;
		private Profile _profile;
		private IncomingTweet _incomingTweet;

		public NotifyUserOfNotFollowingProfileRequest(
			ITweet tweet, 
			Profile profile,
			IncomingTweet incomingTweet)
		{
			_tweet = tweet;
			_profile = profile;
			_incomingTweet = incomingTweet;
		}

		public ITweet Tweet { get { return _tweet; } }
		public Profile Profile { get { return _profile; } }
		public IncomingTweet IncomingTweet { get { return _incomingTweet; } }
	}
}
