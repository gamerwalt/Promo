using hrvster.Core.Handlers.Account;
using hrvster.Infrastructure.Core;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ActivitiesNotificationHandler
{
	public class CreateAccountLoggedInActivity : INotificationHandler<AccountLoggedIn>
	{
		private readonly ILogger<CreateAccountLoggedInActivity> _logger;
		private readonly IActivityRepository _repository;

		public CreateAccountLoggedInActivity(ILoggerFactory loggerFactory, IActivityRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateAccountLoggedInActivity>();
			_repository = repository;
		}

		public async Task Handle(AccountLoggedIn notification, CancellationToken cancellationToken)
		{
			await _repository.AddActivityAsync(
					Guid.NewGuid(),
					notification.Request.AccountUid,
					notification.Request.AccountUid,
					ActivityType.LoggedIn, "accounts");
		}
	}
}
