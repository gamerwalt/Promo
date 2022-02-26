using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetCampaignTypesRequest : IRequest<IEnumerable<CampaignType>>
	{
	}

	public class GetCampaignTypesRequestHandler : IRequestHandler<GetCampaignTypesRequest, IEnumerable<CampaignType>>
	{
		private readonly ICampaignTypesRepository _repository;

		public GetCampaignTypesRequestHandler(ICampaignTypesRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<CampaignType>> Handle(GetCampaignTypesRequest request, CancellationToken cancellationToken)
		{
			return await _repository.GetAllAsync();
		}
	}
}
