using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.NotificationHandlers
{
	public class SaveToGamesDatabase : INotificationHandler<AccountCreated>
	{
		private readonly ILogger<SaveToGamesDatabase> _logger;

		public SaveToGamesDatabase(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<SaveToGamesDatabase>();
		}

		public async Task Handle(AccountCreated notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Saving account created {notification.Account.AccountUid} to the Games service");
		}
	}
}
