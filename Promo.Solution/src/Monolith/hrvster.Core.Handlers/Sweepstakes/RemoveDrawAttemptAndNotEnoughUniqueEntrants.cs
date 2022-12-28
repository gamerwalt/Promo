using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class RemoveDrawAttemptAndNotEnoughUniqueEntrants : AsyncRequestHandler<RemoveDrawAttemptAndNotEnoughUniqueEntrantsRequest>
	{
		private readonly ILogger<RemoveDrawAttemptAndNotEnoughUniqueEntrants> _logger;
		private readonly ISweepsRepository _repository;

		public RemoveDrawAttemptAndNotEnoughUniqueEntrants(ILoggerFactory loggerFactory, ISweepsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<RemoveDrawAttemptAndNotEnoughUniqueEntrants>();
			_repository = repository;
		}

		protected override async Task Handle(RemoveDrawAttemptAndNotEnoughUniqueEntrantsRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Removing the draw attempt and not enough unique entrants request for the sweepstake {sweepName}", request.Sweep.Name);

			await _repository.ToggleSweepDrawAttemptedAsync(request.Sweep.SweepUid, false, cancellationToken);
			await _repository.ToggleSweepAsNotEnoughUniqueEntrantsAsync(request.Sweep.SweepUid, false, cancellationToken);
		}
	}
}
