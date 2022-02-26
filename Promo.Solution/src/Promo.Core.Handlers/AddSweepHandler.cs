using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.Notifications;
using Promo.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.Handlers
{
	public class AddSweepHandler : INotificationHandler<SweepCreated>
	{
		private readonly ILogger<AddSweepHandler> _logger;
		private readonly IServiceSweepsRepository _repository;
		private readonly IDatabase _database;

		public AddSweepHandler(ILoggerFactory loggerFactory, IServiceSweepsRepository repository, IDatabase database)
		{
			_logger = loggerFactory.CreateLogger<AddSweepHandler>();
			_repository = repository;
			_database = database;
		}

		public async Task Handle(SweepCreated notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Saving sweep created {notification.Sweep.SweepName} to the Harvester database");
			await _repository.CreateAsync(notification.Sweep, _database.GetHarvesterConnectionString());
		}
	}
}
