using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.Notifications;
using Promo.Infrastructure.Games;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.Handlers
{
	public class AddAccountToGamesDatabase : INotificationHandler<AccountCreated>
	{
		private readonly ILogger<AddAccountToGamesDatabase> _logger;
		private readonly IGamesAccountsRepository _repository;

		public AddAccountToGamesDatabase(ILoggerFactory loggerFactory, IGamesAccountsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<AddAccountToGamesDatabase>();
			_repository = repository;
		}

		public async Task Handle(AccountCreated notification, CancellationToken cancellationToken)
		{
			await _repository.CreateAsync(notification.Account);
		}
	}
}
