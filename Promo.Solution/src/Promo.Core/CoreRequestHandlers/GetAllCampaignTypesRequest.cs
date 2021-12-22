using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.Repositories.MySqlRepositories;
using Promo.Models.Campaigns;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.CoreRequestHandlers
{
    public class GetAllCampaignTypesRequest : IRequest<IEnumerable<CampaignType>>
    {
    }

    public class GetAllCampaignTypesRequestHandler : IRequestHandler<GetAllCampaignTypesRequest, IEnumerable<CampaignType>>
    {
        private readonly ILogger<GetAllCampaignTypesRequestHandler> _logger;
        private readonly ICampaignTypesRepository _repository;

        public GetAllCampaignTypesRequestHandler(
            ILogger<GetAllCampaignTypesRequestHandler> logger,
            ICampaignTypesRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<IEnumerable<CampaignType>> Handle(GetAllCampaignTypesRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
