using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.Handlers
{
	public class SaveAccountToNotificationDatabase : INotificationHandler<AccountCreated>
	{
		private readonly ILogger<SaveAccountToNotificationDatabase> _logger;

		public SaveAccountToNotificationDatabase(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<SaveAccountToNotificationDatabase>();
		}

		public async Task Handle(AccountCreated notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Saving account created {notification.Account.AccountUid} to the Notification service");
		}
	}
}
