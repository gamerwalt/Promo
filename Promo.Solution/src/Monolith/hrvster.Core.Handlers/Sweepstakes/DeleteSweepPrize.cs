using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class DeleteSweepPrize : AsyncRequestHandler<DeleteSweepPrizeRequest>
	{
		private readonly ILogger<DeleteSweepPrize> _logger;
		private readonly ISweepPrizesRepository _repository;

		public DeleteSweepPrize(ILoggerFactory loggerFactory, ISweepPrizesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<DeleteSweepPrize>();
			_repository = repository;
		}

		protected override async Task Handle(DeleteSweepPrizeRequest request, CancellationToken cancellationToken)
			=> await _repository.DeleteSweepPrizeAsync(request.PrizeUid);
	}
}
