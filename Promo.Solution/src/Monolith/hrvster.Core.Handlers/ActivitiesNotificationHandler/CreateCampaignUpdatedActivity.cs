using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure.Core;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ActivitiesNotificationHandler
{
	public class CreateCampaignUpdatedActivity : INotificationHandler<CampaignUpdated>
	{
		private readonly ILogger<CreateCampaignUpdatedActivity> _logger;
		private readonly IActivityRepository _repository;

		public CreateCampaignUpdatedActivity(ILoggerFactory loggerFactory, IActivityRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateCampaignUpdatedActivity>();
			_repository = repository;
		}

		public async Task Handle(CampaignUpdated notification, CancellationToken cancellationToken)
		{
			await _repository.AddActivityAsync(
				Guid.NewGuid(),
				notification.AccountUid,
				notification.CampaignUid,
				ActivityType.CreatedPromotion, "campaigns");
		}
	}
}
