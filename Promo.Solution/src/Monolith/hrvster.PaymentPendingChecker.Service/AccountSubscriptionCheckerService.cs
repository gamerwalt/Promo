using hrvster.ApplicationServices.Subscription;
using hrvster.Infrastructure;
using hrvster.Models;

namespace hrvster.PaymentPendingChecker.Service
{
	public interface IAccountSubscriptionCheckerService
	{
		Task UnsubscribePaymentPendingSubscriptionsAsync(int pendingHours, CancellationToken token);
	}

	public class AccountSubscriptionCheckerService : IAccountSubscriptionCheckerService
	{
		private readonly ILogger<AccountSubscriptionCheckerService> _logger;
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly IMailRepository _repository;

		public AccountSubscriptionCheckerService(
			ILoggerFactory loggerFactory,
			IAccountSubscriptionService accountSubscriptionService,
			IMailRepository repository)
		{
			_logger = loggerFactory.CreateLogger<AccountSubscriptionCheckerService>();
			_accountSubscriptionService = accountSubscriptionService;
			_repository = repository;
		}

		public async Task UnsubscribePaymentPendingSubscriptionsAsync(int pendingHours, CancellationToken token)
		{
			var status = SubscriptionStatus.PendingPayment;
			var newStatus = SubscriptionStatus.UnSubscribed;
			var paymentPendingAccounts = await _accountSubscriptionService.GetAccountSubscriptionsByStatysAsync(status, pendingHours);

			_logger.LogInformation("Retrieved {count} accounts with pending payment status", paymentPendingAccounts.Count());

			_logger.LogInformation("Started sending email notifications and updating account subscription status");

			foreach (var account in paymentPendingAccounts)
			{
				_logger.LogInformation("Sending account subscription update email to {email} for account {accountUid}",
					account.Account.EmailAddress,
					account.Account.AccountUid);

				await SendEmailNotificationToUser(account.Account, status, newStatus, pendingHours);

				_logger.LogInformation("Setting account subscription status to {newStatus} for account {accountUid}",
					newStatus,
					account.Account.AccountUid);

				await SetAccountSubscriptionStatus(account.SubscriptionUid, newStatus);
			}

			_logger.LogInformation("Finished sending email notifications and updating account subscription status");
		}

		private Task SetAccountSubscriptionStatus(Guid subscriptionUid, SubscriptionStatus newStatus)
			=> _accountSubscriptionService.SetAccountSubscriptionStatusAsync(subscriptionUid, newStatus);

		private async Task SendEmailNotificationToUser(
			Account account,
			SubscriptionStatus status,
			SubscriptionStatus newStatus,
			int pendingHours)
		{
			await _repository.CreateAsync(new Models.Email
			{
				Template = Models.EmailTemplate.AccountSubscriptionUpdateEmail,
				EmailAddress = account.EmailAddress,
				EmailUid = Guid.NewGuid(),
				Status = Models.EmailStatus.New,
				Parameters = new Dictionary<string, string>()
				{
					{ EmailParameters.EmailAddress, account.EmailAddress },
					{ EmailParameters.CurrentAccountSubscriptionStatus, status.ToString() },
					{ EmailParameters.NewAccountSubscriptionStatus, newStatus.ToString() },
					{ EmailParameters.PendingHours, pendingHours.ToString() },
				}
			});
		}
	}
}
