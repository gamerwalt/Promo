using FirebaseAdmin.Auth;
using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Account
{
	public class UpdateAccountDetails : AsyncRequestHandler<UpdateAccountDetailsRequest>
	{
		private readonly ILogger<UpdateAccountDetails> _logger;
		private readonly IAccountsRepository _repository;
		private readonly IMailRepository _mailRepository;
		private readonly IAccountUpdateHistoryRepository _accountUpdateHistoryRepository;
		private readonly FirebaseAuth _firebaseAuth;
		private readonly HarvesterSettingsOptions _harvesterSettings;

		public UpdateAccountDetails(
			ILoggerFactory loggerFactory,
			IAccountsRepository repository,
			IMailRepository mailRepository,
			IAccountUpdateHistoryRepository accountUpdateHistoryRepository,
			FirebaseAuth firebaseAuth,
			HarvesterSettingsOptions harvesterSettings)
		{
			_logger = loggerFactory.CreateLogger<UpdateAccountDetails>();
			_repository = repository;
			_mailRepository = mailRepository;
			_accountUpdateHistoryRepository = accountUpdateHistoryRepository;
			_firebaseAuth = firebaseAuth;
			_harvesterSettings = harvesterSettings;
		}

		protected override async Task Handle(UpdateAccountDetailsRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Request received to update account details for {accountUid}", request.AccountUid);

			var account = await _repository.GetAsync(request.AccountUid);
			if (account != null)
			{
				await UpdateAccountDetailsAsync(account, request);
			}
		}

		private async Task UpdateAccountDetailsAsync(Models.Account account, UpdateAccountDetailsRequest request)
		{
			//update with firebase
			_logger.LogInformation("Updating the google account for the account with uid {uid}", account.AccountUid);

			await _firebaseAuth.UpdateUserAsync(new UserRecordArgs
			{
				Uid = request.AccountUid.ToString(),
				Email = request.EmailAddress,
				PhoneNumber = request.PhoneNumber,
				DisplayName = account.DisplayName
			});

			//if email was updated
			//Send an email to inform old email address of change
			if (account.EmailAddress != request.EmailAddress)
			{
				_logger.LogInformation("Email address is changing for account with uid {accountUid}", account.AccountUid);
				_logger.LogInformation("Sending an email to nofity old email address for account with uid {accountUid}", account.AccountUid);

				await _mailRepository.CreateAsync(new Models.Email
				{
					EmailUid = Guid.NewGuid(),
					EmailAddress = account.EmailAddress,
					Status = EmailStatus.New,
					Parameters = new Dictionary<string, string>()
					{
						{ "DisplayName", account.DisplayName },
						{ "OldEmailAddress", account.EmailAddress },
						{ "NewEmailAddress", request.EmailAddress },
						{ "DashboardLink", new Uri(_harvesterSettings.HrvsterUrl).GenerateUrl(UrlTypes.DashboardLink).ToString() },
						{ "SupportLink", new Uri("https://support.hrvster.com").ToString() }
					},
					Template = EmailTemplate.EmailAddressUpdateRequest
				});

				await _accountUpdateHistoryRepository.CreateRecordAsync(
					Guid.NewGuid(),
					account.AccountId,
					account.EmailAddress,
					request.EmailAddress);
			}

			//if password was updated
			//send a text message to inform old phone about change
			if (account.PhoneNumber != request.PhoneNumber)
			{
				//Send notification
				_logger.LogInformation("Phone number is changing for account with uid {accountUid}", account.AccountUid);

				await _accountUpdateHistoryRepository.CreateRecordAsync(
					Guid.NewGuid(),
					account.AccountId,
					account.PhoneNumber,
					request.PhoneNumber);
			}

			//once successful, we update our own details
			_logger.LogInformation("Updating our systems for the account with uid {uid}", account.AccountUid);

			await _repository.UpdateAsync(request.AccountUid, request.EmailAddress, request.PhoneNumber);
		}
	}
}
