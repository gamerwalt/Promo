using FirebaseAdmin.Auth;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.WebRequests.Accounts;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Account
{
	public class CreateAccount : AsyncRequestHandler<CreateAccountRequest>
	{
		private readonly ILogger<CreateAccount> _logger;
		private readonly FirebaseAuth _firebaseAuth;
		private readonly IMediator _mediator;

		private const string defaultPin = "123456";

		public CreateAccount(
			ILoggerFactory loggerFactory,
			FirebaseAuth firebaseAuth,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<CreateAccount>();
			_firebaseAuth = firebaseAuth;
			_mediator = mediator;
		}

		protected override async Task Handle(CreateAccountRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Creating an account for {emailAddress}.", request.EmailAddress);

			var args = new UserRecordArgs
			{
				Uid = request.UserUid.ToString(),
				Email = request.EmailAddress,
				EmailVerified = false,
				PhoneNumber = request.PhoneNumber,
				Password = request.Password,
				DisplayName = request.Username,
				Disabled = false
			};

			var createdUser = await _firebaseAuth.CreateUserAsync(args);

			_logger.LogInformation("User account for {emailAddress} created successfully to Google Firebase", request.EmailAddress);

			_logger.LogInformation("Generating verification link for {emailAddress}.", request.EmailAddress);

			var verificationLink = await _firebaseAuth.GenerateEmailVerificationLinkAsync(createdUser.Email);

			await _mediator.Publish(new AccountCreatedViaFirebase(createdUser, defaultPin, request.AcceptedTerms, verificationLink));
		}
	}
}
