using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.Notifications;
using Promo.Infrastructure.Games;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.Handlers
{
	public class AddCampaignToGamesDatabase : INotificationHandler<CampaignCreated>
	{
		private readonly ILogger<AddCampaignToGamesDatabase> _logger;
		private readonly IGamesCampaignRepository _repository;

		public AddCampaignToGamesDatabase(ILoggerFactory loggerFactory, IGamesCampaignRepository repository)
		{
			_logger = loggerFactory.CreateLogger<AddCampaignToGamesDatabase>();
			_repository = repository;
		}

		public async Task Handle(CampaignCreated notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Saving campaign created {notification.Campaign.CampaignUid} to the Games database");

			await _repository.CreateAsync(notification.Campaign);
		}
	}
}
