using hrvster.Models.Harvester;
using hrvster.Models.Profiles;
using hrvster.Models.Twitter;
using MediatR;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class NotifyUserOfNotFollowingProfileRequest : IRequest
	{
		private Tweet _tweet;
		private Profile _profile;
		private IncomingTweet _incomingTweet;

		public NotifyUserOfNotFollowingProfileRequest(
			Tweet tweet,
			Profile profile,
			IncomingTweet incomingTweet)
		{
			_tweet = tweet;
			_profile = profile;
			_incomingTweet = incomingTweet;
		}

		public Tweet Tweet { get { return _tweet; } }
		public Profile Profile { get { return _profile; } }
		public IncomingTweet IncomingTweet { get { return _incomingTweet; } }
	}
}
