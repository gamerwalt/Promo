using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Promo.Core.ConfigurationOptions;
using Promo.Core.CoreRequestNotifications;
using Promo.Core.Repositories.MySqlRepositories;
using Promo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Core
{
    public class SaveVerificationEmailInDatabase : INotificationHandler<AccountCreatedViaFirebase>
    {
        private readonly IMailRepository _repository;
        private readonly IConfiguration _config;

        public SaveVerificationEmailInDatabase(IMailRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        public async Task Handle(AccountCreatedViaFirebase notification, CancellationToken cancellationToken)
        {
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

            var emailLink = await FirebaseAuth.DefaultInstance.GenerateEmailVerificationLinkAsync(notification.CreatedUser.Email);

            var email = new Email
            {
                EmailUid = Guid.NewGuid(),
                EmailAddress = notification.CreatedUser.Email,
                Status = EmailStatus.New,
                Template = EmailTemplate.VerificationEmail,
                Sent = false,
                Parameters = new Dictionary<string, string>
                {
                    { "VerificationLink", emailLink },
                    { "Email", notification.CreatedUser.Email },
                    { "DisplayName", notification.CreatedUser.DisplayName }
                }
            };

            await _repository.CreateAsync(email);

            FirebaseApp.DefaultInstance.Delete();
        }
    }
}
