using hrvster.Models;
using hrvster.Models.Harvester;
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
		private IncomingTweet _incomingTweet;

		public SweepEntryFromTweetRecorded(
			IncomingTweet incomingTweet,
			Campaign campaign, 
			Sweep sweep, 
			ITweet tweet, 
			UserIdentifierType userIdentifierType)
		{
			_incomingTweet = incomingTweet;
			_campaign = campaign;
			_sweep = sweep;
			_tweet = tweet;
			_userIdentifierType = userIdentifierType;
		}

		public IncomingTweet IncomingTweet { get { return _incomingTweet; } }
		public Campaign Campaign { get { return _campaign; } }
		public Sweep Sweep { get { return _sweep; } }
		public ITweet Tweet { get { return _tweet; } }
		public UserIdentifierType UserIdentifierType { get { return _userIdentifierType; } }
	}
}
