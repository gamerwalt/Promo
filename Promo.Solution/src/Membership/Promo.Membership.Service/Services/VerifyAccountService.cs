using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using hrvster.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Promo.Clients.Internal;
using Promo.Core;
using Promo.Core.Authentication;
using Promo.Core.Authentication.ConfigurationOptions;
using Promo.Membership.Infrastructure;
using Promo.Membership.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Service.Services
{
    public interface IVerifyAccountService
    {
        Task VerifyAnyUnverifiedAccounts(CancellationToken token);
    }

    public class VerifyAccountService : IVerifyAccountService
    {
        private ILogger<VerifyAccountService> _logger;
        private readonly IAccountsRepository _repository;
        private readonly IMailClient _mailClient;
        private readonly IConfiguration _config;
        private readonly IServiceAuthenticate _serviceAuthenticate;
		private readonly HarvesterSettingsOptions _harvesterSettings;

        public VerifyAccountService(
            ILogger<VerifyAccountService> logger,
            IAccountsRepository repository,
            IMailClient mailclient,
            IConfiguration config,
            IServiceAuthenticate serviceAuthenticate,
			HarvesterSettingsOptions harvesterSettings)
        {
            _logger = logger;
            _repository = repository;
            _config = config;
            _mailClient = mailclient;
            _serviceAuthenticate = serviceAuthenticate;
			_harvesterSettings = harvesterSettings;
        }

        public async Task VerifyAnyUnverifiedAccounts(CancellationToken token)
        {
            _logger.LogInformation($"{nameof(VerifyAnyUnverifiedAccounts)} processing started.");

            var authModel = await _serviceAuthenticate.Authenticate();

            if (authModel == null)
                throw new Exception($"Unable to authenticate service at {nameof(VerifyAccountService)}");

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

            var unverifiedAccounts = await _repository.GetUnverifiedAccountsAsync(token);
            if (unverifiedAccounts != null || unverifiedAccounts.Count() != 0)
            {
                _logger.LogInformation($"There are {unverifiedAccounts.Count()} accounts that needs to be checked for verification.");
                foreach (var account in unverifiedAccounts)
                {
                    _logger.LogInformation($"Verifying account with uid: {account.AccountUid}");
                    try
                    {
                        var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(account.AccountUid.ToString(), token);
                        if (userRecord.EmailVerified)
                        {
                            await _repository.VerifyAccountEmailAsync(account.AccountUid);
                            await SaveWelcomeEmailAsync(account, authModel.IdToken);
                        }
                    }
                    catch (FirebaseAuthException fbEx)
                    {
                        _logger.LogError($"A FirebaseAuthException occurred for the user with uid: {account.AccountUid}");
                        _logger.LogError(fbEx.Message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
            }

            FirebaseApp.DefaultInstance.Delete();

            _logger.LogInformation($"{nameof(VerifyAnyUnverifiedAccounts)} processing completed.");
        }

        private async Task SaveWelcomeEmailAsync(Account account, string accessToken)
        {
            await _mailClient.CreateAsync(new Core.Models.Email
            {
                EmailAddress = account.EmailAddress,
                EmailUid = Guid.NewGuid(),
                Parameters = new Dictionary<string, string>()
                {
                    { "DisplayName", account.DisplayName },
                    { "DashboardLink", _harvesterSettings.hrvsterUrl }
                },
                Status = EmailStatus.New,
                Template = EmailTemplate.WelcomeEmail
            }, accessToken);
        }
    }
}
