using hrvster.Models.Profiles;
using MediatR;
using Tweetinvi.Models;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class NotifyUserOfNotFollowingProfileRequest : IRequest
	{
		private ITweet _tweet;
		private Profile _profile;

		public NotifyUserOfNotFollowingProfileRequest(ITweet tweet, Profile profile)
		{
			_tweet = tweet;
			_profile = profile;
		}

		public ITweet Tweet { get { return _tweet; } }
		public Profile Profile { get { return _profile; } }
	}
}
