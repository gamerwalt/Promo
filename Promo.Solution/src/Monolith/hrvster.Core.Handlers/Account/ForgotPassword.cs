using FirebaseAdmin.Auth;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.WebRequests;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Account
{
	public class ForgotPassword : AsyncRequestHandler<ForgotPasswordRequest>
	{
		private readonly ILogger<ForgotPassword> _logger;
		private readonly FirebaseAuth _firebaseAuth;
		private readonly IMediator _mediator;

		public ForgotPassword(
			ILoggerFactory loggerFactory, 
			FirebaseAuth firebaseAuth,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<ForgotPassword>();
			_firebaseAuth = firebaseAuth;
			_mediator = mediator;
		}

		protected override async Task Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Running Forgot Password for {content}", request.ToJsonString());

			var resetPasswordLink = await _firebaseAuth.GeneratePasswordResetLinkAsync(request.EmailAddress);

			await _mediator.Publish(new PasswordResetLinkReceived(request.EmailAddress, resetPasswordLink));
		}
	}
}
