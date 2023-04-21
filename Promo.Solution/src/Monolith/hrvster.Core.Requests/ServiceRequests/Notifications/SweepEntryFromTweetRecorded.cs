using hrvster.Models;
using hrvster.Models.Harvester;
using hrvster.Models.Twitter;
using MediatR;

namespace hrvster.Core.Requests.ServiceRequests.Notifications
{
	public class SweepEntryFromTweetRecorded : INotification
	{
		private Campaign _campaign;
		private Sweep _sweep;
		private Tweet _tweet;
		private UserIdentifierType _userIdentifierType;
		private IncomingTweet _incomingTweet;

		public SweepEntryFromTweetRecorded(
			IncomingTweet incomingTweet,
			Campaign campaign,
			Sweep sweep,
			Tweet tweet,
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
		public Tweet Tweet { get { return _tweet; } }
		public UserIdentifierType UserIdentifierType { get { return _userIdentifierType; } }
	}
}
