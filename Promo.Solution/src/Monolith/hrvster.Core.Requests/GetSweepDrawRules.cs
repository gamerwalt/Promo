using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetSweepDrawRules : IRequest<IEnumerable<DrawRule>>
	{
		private List<Guid> sweepUids;

		public GetSweepDrawRules(List<Guid> sweepUids)
		{
			this.sweepUids = sweepUids;
		}

		public List<Guid> SweepUids
		{
			get { return sweepUids; }
		}
	}

	public class GetSweepDrawRulesHandler : IRequestHandler<GetSweepDrawRules, IEnumerable<DrawRule>>
	{
		private readonly ISweepDrawRuleRepository _repository;

		public GetSweepDrawRulesHandler(ISweepDrawRuleRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<DrawRule>> Handle(GetSweepDrawRules request, CancellationToken cancellationToken)
		{
			var drawRuleList = new List<DrawRule>();

			foreach (var sweepUid in request.SweepUids)
			{
				drawRuleList.Add(await _repository.GetSweepDrawRuleAsync(sweepUid));
			}

			return drawRuleList;
		}
	}
}
