using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Promo.Core.ApiRequests;
using Promo.Core.ConfigurationOptions;
using Promo.Core.CoreRequestNotifications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.CoreRequestHandlers
{
    public class CreateAccountRequest : IRequest
    {
        private ApiRequests.CreateAccountRequest _newAccount;
        private Guid _userUid;

        public CreateAccountRequest(ApiRequests.CreateAccountRequest newAccount, Guid userUid)
        {
            _newAccount = newAccount;
            _userUid = userUid;
        }

        public ApiRequests.CreateAccountRequest NewAccount => _newAccount;
        public Guid UserUid => _userUid;
    }

    public class CreateAccountRequestHandler : AsyncRequestHandler<CreateAccountRequest>
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;

        public CreateAccountRequestHandler(IMediator mediator, IConfiguration config)
        {
            _mediator = mediator;
            _config = config;
        }

        protected override async Task Handle(CreateAccountRequest request, CancellationToken cancellationToken)
        {
            var args = new UserRecordArgs
            {
                Uid = request.UserUid.ToString(),
                Email = request.NewAccount.EmailAddress,
                EmailVerified = false,
                PhoneNumber = request.NewAccount.PhoneNumber,
                Password = request.NewAccount.Password,
                DisplayName = request.NewAccount.DisplayName,
                Disabled = false
            };

            var createdUser = await ConfigureAndCreateUser(args);

            await _mediator.Publish(new AccountCreatedViaFirebase(
                createdUser, 
                request.NewAccount.Pin, 
                request.NewAccount.TermsAccepted)
            );
        }

        private async Task<UserRecord> ConfigureAndCreateUser(UserRecordArgs args)
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

            var createdUser = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);

            FirebaseApp.DefaultInstance.Delete();

            return createdUser;
        }
    }
}
