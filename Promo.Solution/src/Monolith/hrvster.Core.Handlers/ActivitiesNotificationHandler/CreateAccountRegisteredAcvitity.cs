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
	public class CreateAccountRegisteredAcvitity : INotificationHandler<AccountCreatedViaFirebase>
	{
		private readonly ILogger<CreateAccountRegisteredAcvitity> _logger;
		private readonly IActivityRepository _repository;

		public CreateAccountRegisteredAcvitity(ILoggerFactory loggerFactory, IActivityRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateAccountRegisteredAcvitity>();
			_repository = repository;
		}

		public async Task Handle(AccountCreatedViaFirebase notification, CancellationToken cancellationToken)
		{
			await _repository.AddActivityAsync(
				Guid.NewGuid(),
				notification.UserUid,
				notification.UserUid,
				ActivityType.LoggedIn, "accounts");
		}
	}
}
