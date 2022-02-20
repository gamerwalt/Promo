using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.MessageContracts;
using Promo.Membership.Core.Notifications;
using Promo.Membership.Infrastructure;
using Promo.Membership.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Core
{
	public class PublishCampaignUpdated : INotificationHandler<CampaignUpdated>
	{
		private readonly ILogger<PublishCampaignUpdated> _logger;
		private readonly ICampaignRepository _repository;
		private readonly IRequestClient<ICampaignUpdated> _submitUpdateCampaignClient;

		public PublishCampaignUpdated(ILoggerFactory loggerFactory, ICampaignRepository repository, IRequestClient<ICampaignUpdated> submitUpdateCampaignClient)
		{
			_logger = loggerFactory.CreateLogger<PublishCampaignUpdated>();
			_repository = repository;
			_submitUpdateCampaignClient = submitUpdateCampaignClient;
		}

		public async Task Handle(CampaignUpdated notification, CancellationToken cancellationToken)
		{
			var campaign = await _repository.GetCampaignByUidAsync(notification.CampaignUid);

			if(campaign == null)
			{
				_logger.LogError("Unable to find the campaign with Uid: {campaignUid} while performing action {action}",
					notification.CampaignUid,
					nameof(PublishCampaignUpdated));

				throw new InvalidCampaignException($"Campaign for {notification.CampaignUid} not found.");
			}

			var campaignUpdated = new
			{
				CampaignUid = campaign.CampaignUid,
				CampaignName = campaign.CampaignName,
				CampaignTypeCode = campaign.CampaignType.CampaignTypeCode,
				ActiveInd = campaign.ActiveInd,
				StartDate = campaign.StartDate,
				EndDate = campaign.EndDate
			};

			var (accepted, rejected) = await _submitUpdateCampaignClient.GetResponse<ICampaignUpdatedAccepted, ICampaignUpdatedRejected>(campaignUpdated);

			if (accepted.IsCompletedSuccessfully)
				_logger.LogInformation("Campaign with Uid {campaignUid} queued successfully after update.",
					campaign.CampaignUid);
			else
				_logger.LogError("Unable to queue the campaign with Uid {campaignUid} for consumers. Reason: {reason}",
					campaign.CampaignUid,
					rejected.Exception.Message);

		}
	}
}
