using MediatR;
using Promo.Rules.Infrastructure;
using Promo.Rules.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Rules.Core.RequestHandlers
{
    public class GetRulesForHashTags : IRequest<IEnumerable<Rule>>
    {
        private List<string> hashTags;

        public GetRulesForHashTags(List<string> hashTags)
        {
            this.hashTags = hashTags;
        }

        public List<string> Hashtags
        {
            get { return hashTags; }
        }
    }

    public class GetRulesForHashTagsRequestHandler : IRequestHandler<GetRulesForHashTags, IEnumerable<Rule>>
    {
        private readonly IRulesRepository _repository;

        public GetRulesForHashTagsRequestHandler(IRulesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Rule>> Handle(GetRulesForHashTags request, CancellationToken cancellationToken)
        {
            var rules = new List<Rule>();

            foreach (var hashtag in request.Hashtags)
            {
                var rulesReceived = await _repository.GetRuleByHashTagAsync($"#{hashtag}");

                rules.AddRange(rulesReceived);
            }

            return rules;
        }
    }
}
