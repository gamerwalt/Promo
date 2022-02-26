using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.Notifications;
using Promo.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.Handlers
{
	public class AddAccountHandler : INotificationHandler<AccountCreated>
	{
		private readonly ILogger<AddAccountHandler> _logger;
		private readonly IServiceAccountsRepository _repository;
		private readonly IDatabase _database;

		public AddAccountHandler(ILoggerFactory loggerFactory, IServiceAccountsRepository repository, IDatabase database)
		{
			_logger = loggerFactory.CreateLogger<AddAccountHandler>();
			_repository = repository;
			_database = database;
		}

		public async Task Handle(AccountCreated notification, CancellationToken cancellationToken)
		{
			var gamesConnectionString = _database.GetGamesConnectionstring();
			var harvesterConnectionString = _database.GetHarvesterConnectionString();

			_logger.LogInformation($"Saving the account created {notification.Account.AccountUid} to the Games database");
			await _repository.CreateAsync(notification.Account, gamesConnectionString);

			_logger.LogInformation($"Saving the account created {notification.Account.AccountUid} to the harvester database");
			await _repository.CreateAsync(notification.Account, harvesterConnectionString);
		}
	}
}
