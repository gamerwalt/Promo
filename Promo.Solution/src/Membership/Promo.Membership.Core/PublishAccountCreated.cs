using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.MessageContracts;
using Promo.Membership.Core.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Core
{

	public class PublishAccountCreated : INotificationHandler<AccountCreatedViaFirebase>
	{
		private readonly ILogger<PublishAccountCreated> _logger;
		private readonly IRequestClient<IAccountCreated> _submitNewAccountClient;

		public PublishAccountCreated(
			ILoggerFactory loggerFactory,
			IRequestClient<IAccountCreated> submitNewAccountClient
			)
		{
			_logger = loggerFactory.CreateLogger<PublishAccountCreated>();
			_submitNewAccountClient = submitNewAccountClient;
		}

		public async Task Handle(AccountCreatedViaFirebase notification, CancellationToken cancellationToken)
		{
			var (accepted, rejected) = await _submitNewAccountClient.GetResponse<IAccountCreatedAccepted, IAccountCreatedRejected>(new
			{
				AccountUid = notification.CreatedUser.Uid,
				EmailAddress = notification.CreatedUser.Email,
				PhoneNumber = notification.CreatedUser.PhoneNumber,
				DisplayName = notification.CreatedUser.DisplayName
			});

			if (accepted.IsCompletedSuccessfully)
				_logger.LogInformation("Account with Uid {uid} and email {email} added to queue for consumers.",
					notification.CreatedUser.Uid,
					notification.CreatedUser.Email);
			else
				_logger.LogError("Unable to send the account with Uid {uid} and email {email} to the queue for consumers.",
					notification.CreatedUser.Uid,
					notification.CreatedUser.Email);
		}
	}
}
