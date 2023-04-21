using hrvster.Models;
using hrvster.Models.Profiles;
using hrvster.Models.Twitter;
using MediatR;

namespace hrvster.Core.Requests.MatchGames.Harvester
{
	public class SendTwitterLinkToPlayGameRequest : IRequest
	{
		private string _url;
		private Rule _rule;
		private Tweet _tweet;
		private Profile _profile;

		public SendTwitterLinkToPlayGameRequest(
			string url,
			Rule rule,
			Tweet tweet,
			Profile profile)
		{
			_url = url;
			_rule = rule;
			_tweet = tweet;
			_profile = profile;
		}

		public string Url { get { return _url; } }
		public Rule Rule { get { return _rule; } }
		public Tweet Tweet { get { return _tweet; } }
		public Profile Profile { get { return _profile; } }
	}
}
