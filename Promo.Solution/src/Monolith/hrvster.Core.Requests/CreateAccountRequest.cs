using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using hrvster.Core.Requests.ApiRequests;
using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure.Authentication.ConfigurationOptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class CreateAccountRequest : IRequest
	{
		private NewAccountRequest _newAccountRequest;
		private Guid _userUid;

		public CreateAccountRequest(NewAccountRequest newAccount, Guid userUid)
		{
			_newAccountRequest = newAccount;
			_userUid = userUid;
		}

		public NewAccountRequest NewAccountRequest => _newAccountRequest;
		public Guid UserUid => _userUid;
	}

	public class CreateAccountRequestHandler : AsyncRequestHandler<CreateAccountRequest>
	{
		private readonly ILogger<CreateAccountRequestHandler> _logger;
		private readonly IMediator _mediator;
		private readonly IConfiguration _config;

		public CreateAccountRequestHandler(ILoggerFactory loggerFactory, IMediator mediator, IConfiguration config)
		{
			_logger = loggerFactory.CreateLogger<CreateAccountRequestHandler>();
			_mediator = mediator;
			_config = config;
		}

		protected override async Task Handle(CreateAccountRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Creating account for {accountUid}.", request.UserUid);

			var args = new UserRecordArgs
			{
				Uid = request.UserUid.ToString(),
				Email = request.NewAccountRequest.EmailAddress,
				EmailVerified = false,
				PhoneNumber = request.NewAccountRequest.PhoneNumber,
				Password = request.NewAccountRequest.Password,
				DisplayName = request.NewAccountRequest.DisplayName,
				Disabled = false
			};

			var createdUser = await ConfigureAndCreateUser(args);

			await _mediator.Publish(new AccountCreatedViaFirebase(
				createdUser,
				request.NewAccountRequest.Pin,
				request.NewAccountRequest.TermsAccepted)
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

			_logger.LogInformation("Account for {accountUid} created in Firebase.", args.Uid);

			FirebaseApp.DefaultInstance.Delete();

			return createdUser;
		}
	}
}
