using hrvster.Models;
using hrvster.Models.Harvester;
using MediatR;
using System;
using Tweetinvi.Models;

namespace hrvster.Core.Requests.ServiceRequests.Notifications
{
	public class TweetReceived : INotification
	{
		private Rule _rule;
		private Guid _harvestedUid;
		private Guid _userUid;
		private ITweet _tweet;
		private IncomingTweet _incomingTweet;

		public TweetReceived(IncomingTweet incomingTweet, Rule rule, Guid harvestedUid, Guid newUserUid, ITweet tweet)
		{
			_rule = rule;
			_harvestedUid = harvestedUid;
			_userUid = newUserUid;
			_tweet = tweet;
			_incomingTweet = incomingTweet;
		}

		public IncomingTweet IncomingTweet { get { return _incomingTweet; } }

		public Rule Rule
		{
			get { return _rule; }
		}

		public Guid HarvestedUid
		{
			get { return _harvestedUid; }
		}

		public Guid UserUid
		{
			get { return _userUid; }
		}

		public ITweet Tweet
		{
			get { return _tweet; }
		}
	}
}
