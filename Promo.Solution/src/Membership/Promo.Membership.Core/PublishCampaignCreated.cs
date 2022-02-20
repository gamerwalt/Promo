using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core;
using Promo.Core.MessageContracts;
using Promo.Membership.Core.Notifications;
using Promo.Membership.Infrastructure;
using Promo.Membership.Infrastructure.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Core
{
	public class PublishCampaignCreated : INotificationHandler<CampaignCreated>
	{
		private readonly ILogger<PublishCampaignCreated> _logger;
		private readonly ICampaignTypesRepository _campaignTypesRepository;
		private readonly ICampaignRepository _campaignRepository;
		private readonly IRequestClient<ICampaignCreated> _submitNewCampaignClient;

		public PublishCampaignCreated(
			ILoggerFactory loggerFactory,
			ICampaignTypesRepository campaignTypesRepository,
			ICampaignRepository campaignRepository,
			IRequestClient<ICampaignCreated> submitNewCampaignClient)
		{
			_logger = loggerFactory.CreateLogger<PublishCampaignCreated>();
			_campaignTypesRepository = campaignTypesRepository;
			_campaignRepository = campaignRepository;
			_submitNewCampaignClient = submitNewCampaignClient;
		}

		/// <summary>
		/// Idea here is to get the notificaiton
		/// Determine what type of campaign
		/// And do the necessary step for the campaign type
		/// if sweeps, call to create a new sweeps
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task Handle(CampaignCreated notification, CancellationToken cancellationToken)
		{
			if (notification.Request.CampaignTypeUid == Guid.Empty)
			{
				_logger.LogError("{action}. An error occurred while trying to publish to MassTransit the campaign {campaignName}.",
					nameof(Core.PublishCampaignCreated),
					notification.Request.Name);
				throw new InvalidCampaignTypeException($"An error occurred while trying to handle the {nameof(CampaignCreated)} for the campaign name {notification.Request.Name}.");
			}

			var campaignType = await GetCampaignTypeAsync(notification);

			switch (campaignType.CampaignTypeCode)
			{
				case CampaignTypeCode.AutoAll:
					break;
				case CampaignTypeCode.AutoEnterSweepsFromHashtag:
				case CampaignTypeCode.AutoEnterSweepsFromTweet:
					await PublishCampaignAsync(notification.CampaignUid, notification.AccountUid);
					break;
				case CampaignTypeCode.AutoReplyDM:
					break;
				case CampaignTypeCode.AutoReplyHashtag:
					break;
				case CampaignTypeCode.AutoReplyLikedTweet:
					break;
				case CampaignTypeCode.AutoReplyTweet:
					break;
			}
		}

		private async Task<Infrastructure.Models.CampaignType> GetCampaignTypeAsync(CampaignCreated notification)
		{
			var campaignType = await _campaignTypesRepository.GetCampaignTypeByUidAsync(notification.Request.CampaignTypeUid);

			if (campaignType == null)
			{
				_logger.LogError("{action} CampaignType is null for {campaignName}.",
					nameof(Core.PublishCampaignCreated),
					notification.Request.Name);
				throw new InvalidCampaignTypeException($"An error occurred while trying to handle the {nameof(CampaignCreated)} for the campaign name {notification.Request.Name}.");
			}

			return campaignType;
		}

		private async Task PublishCampaignAsync(Guid campaignUid, Guid accountUid)
		{
			var campaign = await GetCampaignAsync(campaignUid, accountUid);
			var campaignCreated = new
			{
				CampaignUid = campaignUid,
				CampaignName = campaign.CampaignName,
				AccountUid = accountUid,
				CampaignTypeCode = campaign.CampaignType.CampaignTypeCode,
				ActiveInd = campaign.ActiveInd,
				StartDate = campaign.StartDate,
				EndDate = campaign.EndDate
			};
			var (accepted, rejected) = await _submitNewCampaignClient.GetResponse<ICampaignCreatedAccepted, ICampaignCreatedRejected>(campaignCreated);

			if (accepted.IsCompletedSuccessfully)
				_logger.LogInformation("Campaign with Uid {campaignUid} queued successfully.",
					campaignUid);
			else
				_logger.LogError("Unable to queue the campaign with Uid {campaignUid} for consumers. Reason: {reason}",
					campaignUid,
					rejected.Exception.Message);
		}

		private async Task<Infrastructure.Models.Campaign> GetCampaignAsync(Guid campaignUid, Guid accountUid)
		{
			var campaign = await _campaignRepository.GetCampaignByUidAsync(campaignUid, accountUid);

			if (campaign == null)
			{
				_logger.LogError("{action}. An error occurred while trying to publish the campaign with Uid: {campaignUid} for Account with Uid: {accountUid}.",
					nameof(PublishCampaignAsync),
					campaignUid,
					accountUid);
				throw new InvalidCampaignException($"An error occurred while trying to run the method {nameof(PublishCampaignCreated)} to create a sweeps for the campaignUid {campaignUid}");
			}

			return campaign;
		}
	}
}
