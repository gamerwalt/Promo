using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.MessageContracts;
using Promo.Core.Notifications;
using Promo.Infrastructure.Games;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.Handlers
{
	public class UpdateCampaignInGamesDatabase : INotificationHandler<CampaignUpdated>
	{
		private readonly ILogger<UpdateCampaignInGamesDatabase> _logger;
		private readonly IGamesCampaignRepository _repository;

		public UpdateCampaignInGamesDatabase(ILoggerFactory loggerFactory, IGamesCampaignRepository repository)
		{
			_logger = loggerFactory.CreateLogger<UpdateCampaignInGamesDatabase>();
			_repository = repository;
		}
		public async Task Handle(CampaignUpdated notification, CancellationToken cancellationToken)
		{
			await _repository.UpdateAsync(notification.Campaign);
		}
	}
}
