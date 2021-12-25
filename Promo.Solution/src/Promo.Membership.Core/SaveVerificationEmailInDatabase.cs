using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Promo.Clients.Internal;
using Promo.Core;
using Promo.Core.Authentication;
using Promo.Core.Authentication.ConfigurationOptions;
using Promo.Membership.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Core
{
    public class SaveVerificationEmailInDatabase : INotificationHandler<AccountCreatedViaFirebase>
    {
        private readonly IMailClient _mailClient;
        private readonly IConfiguration _config;
        private readonly IServiceAuthenticate _serviceAuthenticate;

        public SaveVerificationEmailInDatabase(
            IMailClient mailClient, 
            IConfiguration config,
            IServiceAuthenticate serviceAuthenticate)
        {
            _mailClient = mailClient;
            _config = config;
            _serviceAuthenticate = serviceAuthenticate;
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

            var email = new Promo.Core.Models.Email
            {
                EmailUid = Guid.NewGuid(),
                EmailAddress = notification.CreatedUser.Email,
                Status = EmailStatus.New,
                Template = EmailTemplate.VerificationEmail,
                Sent = false,
                Parameters = new Dictionary<string, string>
                {
                    { EmailParameters.VerificationLink, emailLink },
                    { EmailParameters.Email, notification.CreatedUser.Email },
                    { EmailParameters.DisplayName, notification.CreatedUser.DisplayName }
                }
            };

            var authModel = await _serviceAuthenticate.Authenticate();

            await _mailClient.CreateAsync(email, authModel.IdToken);

            FirebaseApp.DefaultInstance.Delete();
        }
    }
}
