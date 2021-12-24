using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Promo.Clients.Internal;
using Promo.Core;
using Promo.Core.Authentication.ConfigurationOptions;
using Promo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Email.Service.Services
{
    public interface IVerifyAccountService
    {
        Task VerifyAnyUnverifiedAccounts(CancellationToken token);
    }

    public class VerifyAccountService : IVerifyAccountService
    {
        private ILogger<VerifyAccountService> _logger;
        private readonly IAccountsClient _accountsClient;
        private readonly IMailRepository _mailRepository;
        private readonly IConfiguration _config;

        public VerifyAccountService(
            ILogger<VerifyAccountService> logger,
            IAccountsClient accountsClient,
            IMailRepository mailRepository,
            IConfiguration config)
        {
            _logger = logger;
            _accountsClient = accountsClient;
            _config = config;
            _mailRepository = mailRepository;
        }

        public async Task VerifyAnyUnverifiedAccounts(CancellationToken token)
        {
            _logger.LogInformation($"{nameof(VerifyAnyUnverifiedAccounts)} processing started.");

            if (FirebaseApp.DefaultInstance == null)
            {
                var firebaseConfig = new FirebaseConfig();
                _config.Bind(FirebaseConfig.Firebase, firebaseConfig);

                var firebaseConfigSerialized = JsonConvert.SerializeObject(firebaseConfig);
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(firebaseConfigSerialized),
                });
            }

            var unverifiedAccounts = await _accountsClient.GetUnverifiedAccountsAsync(token);
            if (unverifiedAccounts != null || unverifiedAccounts.Count() != 0)
            {
                _logger.LogInformation($"There are {unverifiedAccounts.Count()} accounts that needs to be checked for verification.");
                foreach (var account in unverifiedAccounts)
                {
                    _logger.LogInformation($"Verifying account with uid: {account.AccountUid}");
                    try
                    {
                        var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(account.AccountUid.ToString());
                        if (userRecord.EmailVerified)
                        {
                            await _accountsClient.VerifyAccountEmailAsync(account.AccountUid);
                            await SaveWelcomeEmailAsync(account);
                        }
                    }
                    catch (FirebaseAuthException fbEx)
                    {
                        _logger.LogError($"A FirebaseAuthException occurred for the user with uid: {account.AccountUid}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
            }

            _logger.LogInformation($"{nameof(VerifyAnyUnverifiedAccounts)} processing completed.");
        }

        private async Task SaveWelcomeEmailAsync(Account account)
        {
            await _mailRepository.CreateAsync(new Models.Email
            {
                EmailAddress = account.EmailAddress,
                EmailUid = Guid.NewGuid(),
                Parameters = new Dictionary<string, string>()
                {
                    { "DisplayName", account.DisplayName },
                    { "DashboardLink", "https://dashboardlink" }
                },
                Status = EmailStatus.New,
                Template = EmailTemplate.WelcomeEmail
            });
        }
    }
}
