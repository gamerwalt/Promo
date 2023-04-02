using hrvster.Models;
using hrvster.Models.Profiles;
using MediatR;
using System;
using Tweetinvi.Models;

namespace hrvster.Core.Requests.MatchGames.Harvester
{
	public class SendTwitterLinkToPlayGameRequest : IRequest
	{
		private string _url;
		private Rule _rule;
		private ITweet _tweet;
		private Profile _profile;

		public SendTwitterLinkToPlayGameRequest(
			string url, 
			Rule rule, 
			ITweet tweet, 
			Profile profile)
		{
			_url = url;
			_rule = rule;
			_tweet = tweet;
			_profile = profile;
		}

		public string Url { get { return _url; } }
		public Rule Rule { get { return _rule; } }
		public ITweet Tweet { get { return _tweet; } }
		public Profile Profile { get { return _profile; } }
	}
}
