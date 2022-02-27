using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
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
