using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using hrvster.Models.Company;
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
		private readonly IAccountCompanyRepository _accountCompanyRepository;
		private readonly IMailRepository _mailRepository;

		public StoreAccountInDatabase(
			ILoggerFactory loggerFactory, 
			IAccountsRepository repository, 
			IAccountCompanyRepository accountCompanyRepository,
			IMailRepository mailRepository)
		{
			_logger = loggerFactory.CreateLogger<StoreAccountInDatabase>();
			_repository = repository;
			_accountCompanyRepository = accountCompanyRepository;
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

			_logger.LogInformation("Storing the company information for {companyName} and {subdomain} into the database.",
				notification.CompanyName,
				notification.SubdomainName);

			await _accountCompanyRepository.CreateAsync(new AccountCompany
			{
				Account = new Account { AccountUid = notification.UserUid },
				Name = notification.CompanyName,
				Subdomain = notification.SubdomainName,
				IsActive = true,
				Uid = Guid.NewGuid()
			}, cancellationToken);

			_logger.LogInformation("Storing the email required for the verification link for the account {accountUid} and email {emailAddress}",
				notification.UserUid,
				notification.EmailAddress);

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
