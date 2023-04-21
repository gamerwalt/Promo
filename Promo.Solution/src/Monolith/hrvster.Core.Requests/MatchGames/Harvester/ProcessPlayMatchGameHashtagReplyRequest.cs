using hrvster.Models;
using hrvster.Models.Harvester;
using hrvster.Models.Twitter;
using MediatR;
using System;
using System.Collections.Generic;

namespace hrvster.Core.Requests.MatchGames.Harvester
{
	public class ProcessPlayMatchGameHashtagReplyRequest : IRequest
	{
		private IncomingTweet _incomingTweet;
		private IEnumerable<BlacklistedUser> _blacklisted;
		private Tweet _tweet;
		private Rule _rule;
		private Guid _userUid;
		private Guid _harvestedUid;

		public ProcessPlayMatchGameHashtagReplyRequest(
			IncomingTweet incomingTweet,
			IEnumerable<BlacklistedUser> blacklisted,
			Tweet tweet,
			Rule rule,
			Guid userUid,
			Guid harvestedUid)
		{
			_incomingTweet = incomingTweet;
			_blacklisted = blacklisted;
			_tweet = tweet;
			_rule = rule;
			_userUid = userUid;
			_harvestedUid = harvestedUid;
		}

		public IncomingTweet IncomingTweet { get { return _incomingTweet; } }
		public IEnumerable<BlacklistedUser> BlacklistedUsers { get { return _blacklisted; } }
		public Tweet Tweet { get { return _tweet; } }
		public Rule Rule { get { return _rule; } }
		public Guid UserUid { get { return _userUid; } }
		public Guid HarvestedUid { get { return _harvestedUid; } }
	}
}
