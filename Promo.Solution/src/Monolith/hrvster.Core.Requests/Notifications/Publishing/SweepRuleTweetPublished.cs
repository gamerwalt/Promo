using hrvster.Core.Clients.Common;
using MediatR;

namespace hrvster.Core.Requests.Notifications.Publishing
{
	public class SweepRuleTweetPublished : INotification
	{
		private Message _message;
		private string _tweetId;
		private bool _mustBeFollowing;

		public SweepRuleTweetPublished(string tweetId, Message message, bool mustBeFollowing)
		{
			_message = message;
			_tweetId = tweetId;
			_mustBeFollowing = mustBeFollowing;
		}

		public Message Message
		{
			get { return _message; }
		}

		public string TweetId
		{
			get { return _tweetId; }
		}

		public bool MustBeFollowing
		{
			get { return _mustBeFollowing; }
		}
	}
}
