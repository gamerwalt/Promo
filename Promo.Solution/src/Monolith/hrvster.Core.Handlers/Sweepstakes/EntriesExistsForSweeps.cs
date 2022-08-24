using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class EntriesExistsForSweeps : IRequestHandler<EntriesExistsForSweepsRequest, bool>
	{
		private readonly ILogger<EntriesExistsForSweeps> _logger;
		private readonly ISweepsEntriesRepository _repository;

		public EntriesExistsForSweeps(ILoggerFactory loggerFactory, ISweepsEntriesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<EntriesExistsForSweeps>();
			_repository = repository;
		}

		public async Task<bool> Handle(EntriesExistsForSweepsRequest request, CancellationToken cancellationToken)
		{
			var result = await _repository.GetSweepEntriesAsync(request.SweepUid);

			return result.Any();
		}
	}
}
