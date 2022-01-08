using MediatR;
using Promo.Rules.Infrastructure;
using Promo.Rules.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Rules.Core.RequestHandlers
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
		private readonly IDrawRuleRepository _repository;

		public GetSweepDrawRulesHandler(IDrawRuleRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<DrawRule>> Handle(GetSweepDrawRules request, CancellationToken cancellationToken)
		{
			var drawRuleList = new List<DrawRule>();

			foreach(var sweepUid in request.SweepUids)
			{
				drawRuleList.Add(await _repository.GetSweepDrawRuleAsync(sweepUid));
			}

			return drawRuleList;
		}
	}
}
