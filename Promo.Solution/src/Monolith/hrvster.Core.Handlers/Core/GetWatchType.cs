using AutoMapper;
using hrvster.Core.Requests.WebRequests.Core;
using hrvster.Infrastructure.Core;
using hrvster.Models.WebModels;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Core
{
	public class GetWatchType : IRequestHandler<GetWatchTypeRequest, IEnumerable<WatchType>>
	{
		private readonly ILogger<GetWatchType> _logger;
		private readonly IWatchTypesRepository _repostiory;
		private readonly IMapper _mapper;

		public GetWatchType(
			ILoggerFactory loggerFactory,
			IWatchTypesRepository repository,
			IMapper mapper)
		{
			_logger = loggerFactory.CreateLogger<GetWatchType>();
			_repostiory = repository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<WatchType>> Handle(GetWatchTypeRequest request, CancellationToken cancellationToken)
		{
			var watchTypes = await _repostiory.GetWatchTypesAsync();

			return _mapper.Map<IEnumerable<WatchType>>(watchTypes);
		}
	}
}
