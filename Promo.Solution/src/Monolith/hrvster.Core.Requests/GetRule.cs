using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetRule : IRequest<Rule>
	{
		private Guid ruleUid;

		public GetRule(Guid ruleUid)
		{
			this.ruleUid = ruleUid;
		}

		public Guid RuleUid
		{
			get { return ruleUid; }
		}
	}

	public class GetRuleHandler : IRequestHandler<GetRule, Rule>
	{
		private readonly ISweepRulesRepository _repository;

		public GetRuleHandler(ISweepRulesRepository repository)
		{
			_repository = repository;
		}

		public async Task<Rule> Handle(GetRule request, CancellationToken cancellationToken)
		{
			return await _repository.GetAsync(request.RuleUid);
		}
	}
}
