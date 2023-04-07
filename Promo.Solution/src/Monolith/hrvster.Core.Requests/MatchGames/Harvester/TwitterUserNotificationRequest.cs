using hrvster.Models;
using hrvster.Models.Profiles;
using MediatR;
using Tweetinvi.Models;

namespace hrvster.Core.Requests.MatchGames.Harvester
{
	public class TwitterUserNotificationRequest : IRequest
	{
		private ITweet _tweet;
		private string _message;
		private Profile _profile;

		public TwitterUserNotificationRequest(ITweet tweet, Profile profile, string message)
		{
			_tweet = tweet;
			_message = message;
			_profile = profile;
		}

		public ITweet Tweet { get { return _tweet; } }
		public string Message { get { return _message; } }
		public Profile Profile { get { return _profile; } }
	}
}
