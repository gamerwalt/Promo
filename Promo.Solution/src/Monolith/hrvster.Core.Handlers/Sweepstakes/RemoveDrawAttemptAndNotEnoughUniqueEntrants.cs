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
		private readonly ISweepNotificationRepository _sweepNotificationRepository;

		public RemoveDrawAttemptAndNotEnoughUniqueEntrants(
			ILoggerFactory loggerFactory,
			ISweepNotificationRepository sweepNotificationRepository)
		{
			_logger = loggerFactory.CreateLogger<RemoveDrawAttemptAndNotEnoughUniqueEntrants>();
			_sweepNotificationRepository = sweepNotificationRepository;
		}

		protected override async Task Handle(RemoveDrawAttemptAndNotEnoughUniqueEntrantsRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Removing the draw attempt and not enough unique entrants request for the sweepstake {sweepName}", request.Sweep.Name);

			await _sweepNotificationRepository.ToggleDrawAttemptedAsync(request.Sweep.SweepId, cancellationToken);
			await _sweepNotificationRepository.ToggleNotEnoughUniqueEntrantsAsync(request.Sweep.SweepId, cancellationToken);
		}
	}
}
