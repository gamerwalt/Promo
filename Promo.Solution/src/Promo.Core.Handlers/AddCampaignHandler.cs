using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.Notifications;
using Promo.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.Handlers
{
	public class AddCampaignHandler : INotificationHandler<CampaignCreated>
	{
		private readonly ILogger<AddCampaignHandler> _logger;
		private readonly IServiceCampaignsRepository _repository;
		private readonly IDatabase _database;

		public AddCampaignHandler(ILoggerFactory loggerFactory, IServiceCampaignsRepository repository, IDatabase database)
		{
			_logger = loggerFactory.CreateLogger<AddCampaignHandler>();
			_repository = repository;
			_database = database;
		}

		public async Task Handle(CampaignCreated notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Saving campaign created {notification.Campaign.CampaignUid} to the Games database");
			await _repository.CreateAsync(notification.Campaign, _database.GetGamesConnectionstring());

			_logger.LogInformation($"Saving campaign created {notification.Campaign.CampaignUid} to the Harvester database");
			await _repository.CreateAsync(notification.Campaign, _database.GetHarvesterConnectionString());
		}
	}
}
