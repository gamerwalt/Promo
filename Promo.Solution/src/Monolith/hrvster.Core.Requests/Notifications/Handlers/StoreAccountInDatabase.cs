using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.Notifications.Handlers
{
	public class StoreAccountInDatabase : INotificationHandler<AccountCreatedViaFirebase>
	{
		private readonly ILogger<StoreAccountInDatabase> _logger;
		private readonly IAccountsRepository _repository;

		public StoreAccountInDatabase(ILoggerFactory loggerFactory, IAccountsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<StoreAccountInDatabase>();
			_repository = repository;
		}

		public async Task Handle(AccountCreatedViaFirebase notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Storing account for {accountUid} into the database.", notification.CreatedUser.Uid);

			await _repository.CreateAsync(new Account
			{
				AccountUid = Guid.Parse(notification.CreatedUser.Uid),
				EmailAddress = notification.CreatedUser.Email,
				EmailVerified = notification.CreatedUser.EmailVerified,
				PhoneNumber = notification.CreatedUser.PhoneNumber,
				DisplayName = notification.CreatedUser.DisplayName,
				PhoneNumberVerified = false,
				Pin = notification.Pin,
				TermsAccepted = notification.TermsAccepted
			});
		}
	}
}
