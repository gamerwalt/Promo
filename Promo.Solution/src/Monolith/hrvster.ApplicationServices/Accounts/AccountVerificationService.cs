using FirebaseAdmin.Auth;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Accounts
{
	public interface IAccountVerificationService
	{
		Task VerifyUnverifiedAccountsAsync(CancellationToken token);
	}

	public class AccountVerificationService : IAccountVerificationService
	{
		private readonly ILogger<AccountVerificationService> _logger;
		private readonly IAccountsRepository _accountsRepository;
		private readonly IMailRepository _mailRepository;
		private readonly FirebaseAuth _firebaseAuth;
		private readonly HarvesterSettingsOptions _harvesterSettings;

		public AccountVerificationService(
			ILoggerFactory loggerFactory,
			IAccountsRepository accountsRepository,
			IMailRepository mailRepository,
			FirebaseAuth firebaseAuth,
			HarvesterSettingsOptions harvesterSettings)
		{
			_logger = loggerFactory.CreateLogger<AccountVerificationService>();
			_accountsRepository = accountsRepository;
			_mailRepository = mailRepository;
			_firebaseAuth = firebaseAuth;
			_harvesterSettings = harvesterSettings;
		}

		public async Task VerifyUnverifiedAccountsAsync(CancellationToken token)
		{
			_logger.LogInformation("Retrieving unverified accounts.");

			var unverifiedAccounts = await _accountsRepository.GetUnverifiedAccountsAsync(token);

			_logger.LogInformation("Retrieved {total} unverified accounts.", unverifiedAccounts.ToList().Count);

			if (unverifiedAccounts != null)
			{
				foreach (var account in unverifiedAccounts)
				{
					_logger.LogInformation("Verifing account with uid {accountUid}", account.AccountUid);

					try
					{
						var userRecord = await _firebaseAuth.GetUserAsync(account.AccountUid.ToString());
						if (userRecord.EmailVerified)
						{
							await _accountsRepository.VerifyAccountEmailAsync(account.AccountUid);

							await SaveWelcomeEmailAsync(account);
						}
					}
					catch (FirebaseAuthException fbx)
					{
						_logger.LogWarning("An error occurred while trying to fetch the details of account {accountUid} from firebase. Firebase Message {message}",
							account.AccountUid,
							fbx.Message);
					}
					catch (Exception ex)
					{
						_logger.LogWarning("An error occurred while trying to fetch the details of account {accountUid} from firebase. System Message {message}",
							account.AccountUid,
							ex.Message);
					}
				}
			}
		}

		private async Task SaveWelcomeEmailAsync(Account account)
		{
			await _mailRepository.CreateAsync(new Email
			{
				EmailAddress = account.EmailAddress,
				EmailUid = Guid.NewGuid(),
				Parameters = new Dictionary<string, string>()
				{
					{ "DisplayName", account.DisplayName },
					{ "DashboardLink", new Uri(_harvesterSettings.HrvsterUrl).GenerateUrl(UrlTypes.DashboardLink).ToString() }
				},
				Status = EmailStatus.New,
				Template = EmailTemplate.WelcomeEmail
			});
		}
	}
}
