using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.Notifications;
using Promo.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.Handlers
{
	public class UpdateSweepHandler : INotificationHandler<SweepUpdated>
	{
		private readonly ILogger<UpdateSweepHandler> _logger;
		private readonly IServiceSweepsRepository _repository;
		private readonly IDatabase _database;

		public UpdateSweepHandler(ILoggerFactory loggerFactory, IServiceSweepsRepository repository, IDatabase database)
		{
			_logger = loggerFactory.CreateLogger<UpdateSweepHandler>();
			_repository = repository;
			_database = database;
		}

		public async Task Handle(SweepUpdated notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Saving sweep updated {notification.Sweep.SweepName} to the Harvester database");
			await _repository.UpdateAsync(notification.Sweep, _database.GetHarvesterConnectionString());
		}
	}
}
