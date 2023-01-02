using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Account
{
	public class UpdateAccount : AsyncRequestHandler<UpdateAccountRequest>
	{
		private readonly ILogger<UpdateAccount> _logger;
		private readonly IAccountsRepository _repository;
		private readonly IFirebaseAuthManager _firebaseAuthManager;
		private readonly IToastNotification _notificationService;
		private readonly ISessionManager _sessionManager;

		public UpdateAccount(
			ILoggerFactory loggerFactory,
			IAccountsRepository repository,
			IFirebaseAuthManager firebaseAuthManager,
			IToastNotification notificationService,
			ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<UpdateAccount>();
			_repository = repository;
			_firebaseAuthManager = firebaseAuthManager;
			_notificationService = notificationService;
			_sessionManager = sessionManager;
		}


		protected override async Task Handle(UpdateAccountRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Updating the user with uid {uid}.", request.AccountUid);

			try
			{
				var userRecord = await _firebaseAuthManager.GetUserAsync(request.AccountUid, cancellationToken);

				if (userRecord == null)
				{
					_logger.LogWarning("Unable to locate user account with Uid from google.", request.AccountUid);

					_notificationService.AddWarningToastMessage("Unable to locate your account. Please let support know.");

					return;
				}

				_logger.LogInformation("User account with uid {Uid} located.", request.AccountUid);

				var account = await _repository.GetAsync(request.AccountUid);

				if (account == null)
				{
					_logger.LogWarning("Unable to locate user account with Uid from our systems.", request.AccountUid);

					_notificationService.AddWarningToastMessage("Unable to locate your account. Please let support know.");

					return;
				}

				_logger.LogInformation("Updating google account for uid {Uid}.", request.AccountUid);
				await _firebaseAuthManager.UpdateUserAsync(new FirebaseAdmin.Auth.UserRecordArgs
				{
					DisplayName = request.DisplayName,
					Email = account.EmailAddress,
					Uid = request.AccountUid.ToString()
				});

				_logger.LogInformation("Updating account for uid {Uid} on our systems.", request.AccountUid);

				await _repository.UpdateAsync(request.AccountUid, request.FirstName, request.LastName, request.DisplayName);

				userRecord = await _firebaseAuthManager.GetUserAsync(request.AccountUid, cancellationToken);
				account = await _repository.GetAsync(request.AccountUid);
				await _sessionManager.UpdateDisplayNameAsync(account.DisplayName);
			}
			catch (Exception ex)
			{
				_logger.LogInformation("There was an error updating the user with uid {uid}. Exception Message: {message}",
					request.AccountUid,
					ex.Message);
			}
		}
	}
}
