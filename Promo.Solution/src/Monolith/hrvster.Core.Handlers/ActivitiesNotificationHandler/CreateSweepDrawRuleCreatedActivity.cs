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
	public class CreateSweepDrawRuleCreatedActivity : INotificationHandler<SweepDrawRuleCreated>
	{
		private readonly ILogger<CreateSweepDrawRuleCreatedActivity> _logger;
		private readonly IActivityRepository _repository;

		public CreateSweepDrawRuleCreatedActivity(
			ILoggerFactory loggerFactory, IActivityRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepDrawRuleCreatedActivity>();
			_repository = repository;
		}

		public async Task Handle(SweepDrawRuleCreated notification, CancellationToken cancellationToken)
		{
			await _repository.AddActivityAsync(
				Guid.NewGuid(),
				notification.AccountUid,
				notification.SweepstakeDrawRule.SweepDrawRuleUid,
				ActivityType.AddedSweepstakesDrawRule, "sweep");
		}
	}
}
