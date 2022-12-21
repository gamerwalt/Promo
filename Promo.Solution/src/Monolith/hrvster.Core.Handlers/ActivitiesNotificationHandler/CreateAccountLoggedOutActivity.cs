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
	public class CreateAccountLoggedOutActivity : INotificationHandler<AccountLoggedOut>
	{
		private readonly ILogger<CreateAccountLoggedOutActivity> _logger;
		private readonly IActivityRepository _repository;

		public CreateAccountLoggedOutActivity(ILoggerFactory loggerFactory, IActivityRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateAccountLoggedOutActivity>();
			_repository = repository;
		}

		public async Task Handle(AccountLoggedOut notification, CancellationToken cancellationToken)
		{
			await _repository.AddActivityAsync(
				Guid.NewGuid(),
				notification.AccountUid,
				notification.AccountUid,
				ActivityType.LoggedOut, "accounts");
		}
	}
}
