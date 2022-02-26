using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.Notifications;
using Promo.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.Handlers
{
	public class UpdateCampaignHandler : INotificationHandler<CampaignUpdated>
	{
		private readonly ILogger<UpdateCampaignHandler> _logger;
		private readonly IServiceCampaignsRepository _repository;
		private readonly IDatabase _database;

		public UpdateCampaignHandler(ILoggerFactory loggerFactory, IServiceCampaignsRepository repository, IDatabase database)
		{
			_logger = loggerFactory.CreateLogger<UpdateCampaignHandler>();
			_repository = repository;
			_database = database;
		}
		public async Task Handle(CampaignUpdated notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Saving campaign updated {notification.Campaign.CampaignUid} to the Games database");
			await _repository.UpdateAsync(notification.Campaign, _database.GetGamesConnectionstring());

			_logger.LogInformation($"Saving campaign updated {notification.Campaign.CampaignUid} to the Harvester database");
			await _repository.UpdateAsync(notification.Campaign, _database.GetHarvesterConnectionString());
		}
	}
}
