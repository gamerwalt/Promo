using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.Notifications.Handlers
{
	public class StoreAccountInDatabase : INotificationHandler<AccountCreatedViaFirebase>
	{
		private readonly ILogger<StoreAccountInDatabase> _logger;
		private readonly IAccountsRepository _repository;
		private readonly IMailRepository _mailRepository;

		public StoreAccountInDatabase(ILoggerFactory loggerFactory, IAccountsRepository repository, IMailRepository mailRepository)
		{
			_logger = loggerFactory.CreateLogger<StoreAccountInDatabase>();
			_repository = repository;
			_mailRepository = mailRepository;
		}

		public async Task Handle(AccountCreatedViaFirebase notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Storing account for {accountUid} {emailAddress} into the database.",
				notification.UserUid,
				notification.EmailAddress);

			await _repository.CreateAsync(new Account
			{
				AccountUid = notification.UserUid,
				EmailAddress = notification.EmailAddress,
				EmailVerified = false,
				PhoneNumber = notification.PhoneNumber,
				DisplayName = notification.UserName,
				PhoneNumberVerified = false,
				Pin = notification.Pin,
				TermsAccepted = notification.TermsAccepted,
				VerificationLink = notification.VerificationLink
			});

			await _mailRepository.CreateAsync(new Email
			{
				EmailAddress = notification.EmailAddress,
				EmailUid = Guid.NewGuid(),
				Parameters = new Dictionary<string, string>()
				{
					{ "DisplayName", notification.UserName },
					{ "DashboardLink", "https://app.hrvster/dashboard" },
					{ "VerificationLink", notification.VerificationLink }
				},
				Template = EmailTemplate.VerificationEmail,
				Status = EmailStatus.New
			});
		}
	}
}
