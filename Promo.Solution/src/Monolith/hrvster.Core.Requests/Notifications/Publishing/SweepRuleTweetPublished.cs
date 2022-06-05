using hrvster.Core.Clients.Common;
using MediatR;

namespace hrvster.Core.Requests.Notifications.Publishing
{
	public class SweepRuleTweetPublished : INotification
	{
		private Message _message;
		private string _tweetId;

		public SweepRuleTweetPublished(string tweetId, Message message)
		{
			_message = message;
			_tweetId = tweetId;
		}

		public Message Message
		{
			get { return _message; }
		}

		public string TweetId
		{
			get { return _tweetId; }
		}
	}
}
