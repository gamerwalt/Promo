using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure.Core;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ActivitiesNotificationHandler
{
	public class CreateSweepUpdatedActivity : INotificationHandler<SweepstakeUpdated>
	{
		private readonly ILogger<CreateSweepUpdatedActivity> _logger;
		private readonly IActivityRepository _repository;

		public CreateSweepUpdatedActivity(ILoggerFactory loggerFactory, IActivityRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepUpdatedActivity>();
			_repository = repository;
		}

		public async Task Handle(SweepstakeUpdated notification, CancellationToken cancellationToken)
		{
			await _repository.AddActivityAsync(
				Guid.NewGuid(),
				notification.Sweepstake.Account.AccountUid,
				notification.Sweepstake.SweepUid,
				ActivityType.UpdatedSweepstakes, "sweep");
		}
	}
}
