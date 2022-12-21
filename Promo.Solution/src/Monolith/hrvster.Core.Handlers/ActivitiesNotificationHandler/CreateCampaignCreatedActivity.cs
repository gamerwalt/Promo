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
	public class CreateCampaignCreatedActivity : INotificationHandler<CampaignCreated>
	{
		private readonly ILogger<CreateCampaignCreatedActivity> _logger;
		private readonly IActivityRepository _repository;

		public CreateCampaignCreatedActivity(ILoggerFactory loggerFactory, IActivityRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateCampaignCreatedActivity>();
			_repository = repository;
		}

		public async Task Handle(CampaignCreated notification, CancellationToken cancellationToken)
		{
			await _repository.AddActivityAsync(
				Guid.NewGuid(),
				notification.AccountUid,
				notification.CampaignUid,
				ActivityType.CreatedPromotion, "campaigns");
		}
	}
}
