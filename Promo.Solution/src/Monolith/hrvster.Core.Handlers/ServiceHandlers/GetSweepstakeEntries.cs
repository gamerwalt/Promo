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
	public class GetSweepstakeEntries : IRequestHandler<GetSweepstakeEntriesRequest, IEnumerable<SweepEntry>>
	{
		private readonly ILogger<GetSweepstakeEntries> _logger;
		private readonly ISweepsEntriesRepository _repository;

		public GetSweepstakeEntries(ILoggerFactory loggerFactory, ISweepsEntriesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetSweepstakeEntries>();
			_repository = repository;
		}

		public async Task<IEnumerable<SweepEntry>> Handle(GetSweepstakeEntriesRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Retriving the sweep entries for the sweepstakes {sweepName}", request.Sweep.Name);

			return await _repository.GetSweepEntriesAsync(request.Sweep.SweepUid);
		}
	}
}
