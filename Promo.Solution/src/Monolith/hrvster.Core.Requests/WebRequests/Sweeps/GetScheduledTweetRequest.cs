using hrvster.Models;
using hrvster.Models.Scheduling;
using MediatR;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class GetScheduledTweetRequest : IRequest<ScheduledSweepRuleTweet>
	{
		private Rule _rule;

		public GetScheduledTweetRequest(Rule rule)
		{
			_rule = rule;
		}

		public Rule Rule { get { return _rule; } }
	}
}
