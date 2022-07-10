using hrvster.Models;
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

		public TweetReceived(Rule rule, Guid harvestedUid, Guid newUserUid, ITweet tweet)
		{
			_rule = rule;
			_harvestedUid = harvestedUid;
			_userUid = newUserUid;
			_tweet = tweet;
		}

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
