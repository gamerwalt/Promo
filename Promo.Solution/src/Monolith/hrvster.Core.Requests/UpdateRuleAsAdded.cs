using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class UpdateRuleAsAdded : IRequest<Rule>
	{
		private Guid ruleUid;

		public UpdateRuleAsAdded(Guid ruleUid)
		{
			this.ruleUid = ruleUid;
		}

		public Guid RuleUid
		{
			get { return ruleUid; }
		}
	}

	public class UpdateRuleAsAddedRequestHandler : IRequestHandler<UpdateRuleAsAdded, Rule>
	{
		private readonly ISweepRulesRepository _repository;

		public UpdateRuleAsAddedRequestHandler(ISweepRulesRepository repository)
		{
			_repository = repository;
		}

		public async Task<Rule> Handle(UpdateRuleAsAdded request, CancellationToken cancellationToken)
		{
			await _repository.UpdateRuleAsAddedAsync(request.RuleUid);

			return await _repository.GetAsync(request.RuleUid);
		}
	}
}
