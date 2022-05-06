using hrvster.Models;
using MediatR;
using System;
using Tweetinvi.Models;

namespace hrvster.Core.Requests.Notifications.Harvester
{
	public class NewHashTagTweetReceived : INotification
	{
		private ITweet tweet;
		private Rule rule;
		private Guid harvestedUid;
		private Guid userUid;

		public NewHashTagTweetReceived(Rule rule, Guid harvestedUid, Guid userUid, ITweet tweet)
		{
			this.tweet = tweet;
			this.rule = rule;
			this.harvestedUid = harvestedUid;
			this.userUid = userUid;
		}

		public ITweet Tweet
		{
			get { return tweet; }
		}

		public Rule Rule
		{
			get { return rule; }
		}

		public Guid HarvestedUid
		{
			get { return harvestedUid; }
		}

		public Guid UserUid
		{
			get { return userUid; }
		}
	}
}
