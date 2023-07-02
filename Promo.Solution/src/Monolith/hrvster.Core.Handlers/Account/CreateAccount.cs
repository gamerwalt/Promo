using FirebaseAdmin.Auth;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Account
{
	public class CreateAccount : AsyncRequestHandler<CreateAccountRequest>
	{
		private readonly ILogger<CreateAccount> _logger;
		private readonly IFirebaseAuthManager _firebaseAuth;
		private readonly IMediator _mediator;

		private const string defaultPin = "123456";

		public CreateAccount(
			ILoggerFactory loggerFactory,
			IFirebaseAuthManager firebaseAuth,
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

			_logger.LogInformation("User account for {emailAddress} created successfully to Google Firebase as {createdUser}",
				request.EmailAddress,
				createdUser.ToJsonString());

			_logger.LogInformation("Generating verification link for {emailAddress}.", 
				request.EmailAddress);

			var verificationLink = await _firebaseAuth.GenerateEmailVerificationLinkAsync(request.EmailAddress);

			await _mediator.Publish(
				new AccountCreatedViaFirebase(
					request.EmailAddress, 
					request.UserUid, 
					request.PhoneNumber, 
					request.Username, 
					request.FirstName,
					request.LastName,
					defaultPin, 
					request.AcceptedTerms, 
					verificationLink,
					request.CompanyName,
					request.SubdomainName));
		}
	}
}
