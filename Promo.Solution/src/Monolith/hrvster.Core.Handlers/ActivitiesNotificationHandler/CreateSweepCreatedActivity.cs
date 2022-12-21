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
	public class CreateSweepCreatedActivity : INotificationHandler<SweepCreated>
	{
		private readonly ILogger<CreateSweepCreatedActivity> _logger;
		private readonly IActivityRepository _repository;

		public CreateSweepCreatedActivity(ILoggerFactory loggerFactory, IActivityRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepCreatedActivity>();
			_repository = repository;
		}

		public async Task Handle(SweepCreated notification, CancellationToken cancellationToken)
		{
			await _repository.AddActivityAsync(
				Guid.NewGuid(),
				notification.Sweep.Account.AccountUid,
				notification.Sweep.SweepUid,
				ActivityType.CreatedSweepstakes, "sweep");
		}
	}
}
