using MediatR;
using Promo.Rules.Infrastructure;
using Promo.Rules.Infrastructure.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Rules.Core.RequestHandlers
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
        private readonly IRulesRepository _repository;

        public GetRuleHandler(IRulesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Rule> Handle(GetRule request, CancellationToken cancellationToken)
        {
            return await _repository.GetAsync(request.RuleUid);
        }
    }
}
