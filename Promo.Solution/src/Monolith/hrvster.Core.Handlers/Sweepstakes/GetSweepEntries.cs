using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class GetSweepEntries : IRequestHandler<GetSweepEntriesRequest, IEnumerable<SweepEntry>>
	{
		private readonly ILogger<GetSweepEntries> _logger;
		private readonly ISweepsEntriesRepository _repository;

		public GetSweepEntries(ILoggerFactory loggerFactory, ISweepsEntriesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetSweepEntries>();
			_repository = repository;
		}

		public async Task<IEnumerable<SweepEntry>> Handle(GetSweepEntriesRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Halding {className} to get sweep entries for sweep with uid {sweepUid}",
				nameof(GetSweepEntries),
				request.SweepUid);

			return await _repository.GetSweepEntriesAsync(request.SweepUid);
		}
	}
}
