using MediatR;
using Promo.Rules.Infrastructure;
using Promo.Rules.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Rules.Core.RequestHandlers
{
    public class GetWatchTypeCampaignTypes : IRequest<IEnumerable<WatchTypeCampaignType>>
    {
    }

    public class GetWatchTypeCampaignTypesHandler : IRequestHandler<GetWatchTypeCampaignTypes, IEnumerable<WatchTypeCampaignType>>
    {
        private readonly IWatchTypeCampaignTypesRepository _repository;
        public GetWatchTypeCampaignTypesHandler(IWatchTypeCampaignTypesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<WatchTypeCampaignType>> Handle(GetWatchTypeCampaignTypes request, CancellationToken cancellationToken)
        {
            return await _repository.GetWatchTypeCampaignTypesAsync();
        }
    }
}
