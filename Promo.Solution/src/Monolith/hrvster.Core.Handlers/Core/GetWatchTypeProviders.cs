using hrvster.Core.Requests.WebRequests.Core;
using hrvster.Infrastructure.Core;
using hrvster.Models.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Core
{
	public class GetWatchTypeProviders : IRequestHandler<GetWatchTypeProvidersRequest, IEnumerable<WatchTypeProvider>>
	{
		private readonly ILogger<GetWatchTypeProviders> _logger;
		private readonly IWatchTypeProvidersRepository _repository;

		public GetWatchTypeProviders(ILoggerFactory loggerFactory, IWatchTypeProvidersRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetWatchTypeProviders>();
			_repository = repository;
		}

		public async Task<IEnumerable<WatchTypeProvider>> Handle(GetWatchTypeProvidersRequest request, CancellationToken cancellationToken)
		{
			return await _repository.GetWatchTypeProviders();
		}
	}
}
