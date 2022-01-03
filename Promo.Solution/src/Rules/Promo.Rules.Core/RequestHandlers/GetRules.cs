using MediatR;
using Promo.Rules.Infrastructure;
using Promo.Rules.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Rules.Core.RequestHandlers
{
    public class GetRules : IRequest<IEnumerable<Rule>>
    {
        private string watchTypeCode;

        public GetRules(string watchTypeCode)
        {
            this.watchTypeCode = watchTypeCode;
        }

        public string WatchTypeCode
        {
            get { return watchTypeCode; }
        }
    }

    public class GetRulesHandler : IRequestHandler<GetRules, IEnumerable<Rule>>
    {
        private readonly IRulesRepository _repository;

        public GetRulesHandler(IRulesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Rule>> Handle(GetRules request, CancellationToken cancellationToken)
        {
            return await _repository.GetRules(request.WatchTypeCode);
        }
    }
}
