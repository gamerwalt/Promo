using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure.Core;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ActivitiesNotificationHandler
{
	public class CreatePromotionByWizardCreatedActivity : INotificationHandler<PromotionCreated>
	{
		private readonly ILogger<CreatePromotionByWizardCreatedActivity> _logger;
		private readonly IActivityRepository _repository;

		public CreatePromotionByWizardCreatedActivity(ILoggerFactory loggerFactory, IActivityRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreatePromotionByWizardCreatedActivity>();
			_repository = repository;
		}

		public async Task Handle(PromotionCreated notification, CancellationToken cancellationToken)
		{
			await _repository.AddActivityAsync(
				Guid.NewGuid(),
				notification.Campaign.Account.AccountUid,
				notification.Campaign.CampaignUid,
				ActivityType.CreatedPromotionByWizard, "campaigns");
		}
	}
}
