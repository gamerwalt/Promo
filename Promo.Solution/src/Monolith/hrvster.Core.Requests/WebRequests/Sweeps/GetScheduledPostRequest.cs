using hrvster.Models;
using hrvster.Models.Scheduling;
using MediatR;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class GetScheduledPostRequest : IRequest<ScheduledPost>
	{
		private Rule _rule;

		public GetScheduledPostRequest(Rule rule)
		{
			_rule = rule;
		}

		public Rule Rule { get { return _rule; } }
	}
}
