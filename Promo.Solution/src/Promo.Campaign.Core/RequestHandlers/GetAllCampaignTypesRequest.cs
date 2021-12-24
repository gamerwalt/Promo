using MediatR;
using Promo.Campaign.Infrastructure;
using Promo.Campaign.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Campaign.Core.RequestHandlers
{
    public class GetAllCampaignTypesRequest : IRequest<IEnumerable<CampaignType>>
    {
    }

    public class GetAllCampaignTypesRequestHandler : IRequestHandler<GetAllCampaignTypesRequest, IEnumerable<CampaignType>>
    {
        private readonly ICampaignTypesRepository _repository;

        public GetAllCampaignTypesRequestHandler(ICampaignTypesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CampaignType>> Handle(GetAllCampaignTypesRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
