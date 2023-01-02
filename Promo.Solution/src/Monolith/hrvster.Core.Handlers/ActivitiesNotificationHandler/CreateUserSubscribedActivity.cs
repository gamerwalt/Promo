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
	public class CreateUserSubscribedActivity : INotificationHandler<UserSubscribed>
	{
		private readonly ILogger<CreateUserSubscribedActivity> _logger;
		private readonly IActivityRepository _repository;

		public CreateUserSubscribedActivity(ILoggerFactory loggerFactory, IActivityRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateUserSubscribedActivity>();
			_repository = repository;
		}

		public async Task Handle(UserSubscribed notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("User with uid {accountUid} has been subscribed to the plan {planName}",
				notification.AccountUid,
				notification.Plan.PlanName);

			await _repository.AddActivityAsync(
				Guid.NewGuid(),
				notification.AccountUid,
				notification.Plan.PlanUid,
				ActivityType.Subscribed, "account_subscriptions");
		}
	}
}
