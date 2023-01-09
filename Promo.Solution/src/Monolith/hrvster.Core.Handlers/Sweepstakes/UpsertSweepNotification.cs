using hrvster.Core.Requests.Sweepstakes;
using hrvster.Infrastructure;
using hrvster.Models.Sweepstakes;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class UpsertSweepNotification : AsyncRequestHandler<UpsertSweepNotificationRequest>
	{
		private readonly ILogger<UpsertSweepNotification> _logger;
		private readonly ISweepNotificationRepository _repository;

		public UpsertSweepNotification(
			ILoggerFactory loggerFactory,
			ISweepNotificationRepository repository)
		{
			_logger = loggerFactory.CreateLogger<UpsertSweepNotification>();
			_repository = repository;
		}

		protected override async Task Handle(UpsertSweepNotificationRequest request, CancellationToken cancellationToken)
		{
			switch (request.NotificationType)
			{
				case SweepNotificationType.SweepsLive:
					await _repository.ToggleSweepsLiveAsync(request.Sweepstake.SweepId, cancellationToken);
					break;
				case SweepNotificationType.NotEnoughUniqueEntrants:
					await _repository.ToggleNotEnoughUniqueEntrantsAsync(request.Sweepstake.SweepId, cancellationToken);
					break;
				case SweepNotificationType.SweepsGoingLive:
					await _repository.ToggleSweepsGoingLiveAsync(request.Sweepstake.SweepId, cancellationToken);
					break;
				case SweepNotificationType.DrawAttempted:
					await _repository.ToggleDrawAttemptedAsync(request.Sweepstake.SweepId, cancellationToken);
					break;
				case SweepNotificationType.DrawHappened:
					await _repository.ToggleDrawHappenedAsync(request.Sweepstake.SweepId, cancellationToken);
					break;
			}
		}
	}
}
