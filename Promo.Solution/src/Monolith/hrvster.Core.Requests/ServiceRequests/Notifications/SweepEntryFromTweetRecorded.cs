using hrvster.Models;
using MediatR;
using Tweetinvi.Models;

namespace hrvster.Core.Requests.ServiceRequests.Notifications
{
	public class SweepEntryFromTweetRecorded : INotification
	{
		private Campaign _campaign;
		private Sweep _sweep;
		private ITweet _tweet;
		private UserIdentifierType _userIdentifierType;

		public SweepEntryFromTweetRecorded(Campaign campaign, Sweep sweep, ITweet tweet, UserIdentifierType userIdentifierType)
		{
			_campaign = campaign;
			_sweep = sweep;
			_tweet = tweet;
			_userIdentifierType = userIdentifierType;
		}

		public Campaign Campaign { get { return _campaign; } }
		public Sweep Sweep { get { return _sweep; } }
		public ITweet Tweet { get { return _tweet; } }
		public UserIdentifierType UserIdentifierType { get { return _userIdentifierType; } }
	}
}
