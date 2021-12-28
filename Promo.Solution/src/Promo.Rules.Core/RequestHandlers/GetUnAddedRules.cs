using MediatR;
using Promo.Rules.Infrastructure;
using Promo.Rules.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Rules.Core.RequestHandlers
{
    public class GetUnAddedRules : IRequest<IEnumerable<Rule>>
    {
    }

    public class GetUnAddedRulesHandler : IRequestHandler<GetUnAddedRules, IEnumerable<Rule>>
    {
        private readonly IRulesRepository _repository;

        public GetUnAddedRulesHandler(IRulesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Rule>> Handle(GetUnAddedRules request, CancellationToken cancellationToken)
        {
            return await _repository.GetUnAddedRulesAsync();
        }
    }
}
