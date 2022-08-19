using FirebaseAdmin.Auth;
using hrvster.Core.Requests.WebRequests;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Account
{
	public class VerifyEmail : AsyncRequestHandler<VerifyEmailRequest>
	{
		private readonly ILogger<VerifyEmail> _logger;
		private readonly FirebaseAuth _firebaseAuth;
		private readonly IMailRepository _mailRepository;

		public VerifyEmail(ILoggerFactory loggerFactory, FirebaseAuth firebaseAuth, IMailRepository mailRepository)
		{
			_logger = loggerFactory.CreateLogger<VerifyEmail>();
			_firebaseAuth = firebaseAuth;
			_mailRepository = mailRepository;
		}

		protected override async Task Handle(VerifyEmailRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Verify email request handling request: {content}", request.ToJsonString());

			var verificationLink = string.Empty;

			try
			{
				verificationLink = await _firebaseAuth.GenerateEmailVerificationLinkAsync(request.EmailAddress);
			}
			catch (Exception ex)
			{
				_logger.LogWarning("Firebase Auth threw an exception. Message: {message}, EmailAddress: {emailAddress}",
					ex.Message, request.EmailAddress);
			}
			
			if (!string.IsNullOrWhiteSpace(verificationLink))
			{
				_logger.LogDebug("Creating record in mail repository to send the verification email out to {emailAddress}", request.EmailAddress);

				await _mailRepository.CreateAsync(new Models.Email
				{
					EmailAddress = request.EmailAddress,
					EmailUid = Guid.NewGuid(),
					Parameters = new Dictionary<string, string>()
					{
						{ "DashboardLink", "https://app.hrvster/dashboard" },
						{ "VerificationLink", verificationLink }
					},
					Template = EmailTemplate.VerificationEmail,
					Status = EmailStatus.New
				});
			}
		}
	}
}
