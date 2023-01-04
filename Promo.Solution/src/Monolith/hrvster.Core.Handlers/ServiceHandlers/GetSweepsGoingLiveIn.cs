using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class GetSweepsGoingLiveIn : IRequestHandler<GetSweepsGoingLiveInRequest, IEnumerable<Sweep>>
	{
		private readonly ILogger<GetSweepsGoingLiveIn> _logger;
		private readonly ISweepsRepository _repository;

		public GetSweepsGoingLiveIn(ILoggerFactory loggerFactory, ISweepsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetSweepsGoingLiveIn>();
			_repository = repository;
		}

		public async Task<IEnumerable<Sweep>> Handle(GetSweepsGoingLiveInRequest request, CancellationToken cancellationToken)
		{
			return await _repository.GetAllGoingLiveInHoursAsync(request.HourValue, cancellationToken);
		}
	}
}
