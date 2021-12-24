using MediatR;
using Promo.Campaign.Core.Notifications;
using Promo.Campaign.Infrastructure;
using Promo.Campaign.Infrastructure.Exceptions;
using Promo.Clients.Internal;
using Promo.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Campaign.Core
{
    public class HandleCampaignCreatedService : INotificationHandler<CampaignCreated>
    {
        private readonly ICampaignTypesRepository _campaignTypesRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly ISweepsClient _sweepsClient;

        public HandleCampaignCreatedService(
            ICampaignTypesRepository campaignTypesRepository,
            ICampaignRepository campaignRepository,
            ISweepsClient sweepsClient)
        {
            _campaignTypesRepository = campaignTypesRepository;
            _campaignRepository = campaignRepository;
            _sweepsClient = sweepsClient;
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
                throw new InvalidCampaignTypeException($"An error occurred while trying to handle the {nameof(CampaignCreated)} for the campaign name {notification.Request.Name}.");

            var campaignType = await _campaignTypesRepository.GetCampaignTypeByUidAsync(notification.Request.CampaignTypeUid);

            if (campaignType == null)
                throw new InvalidCampaignTypeException($"An error occurred while trying to handle the {nameof(CampaignCreated)} for the campaign name {notification.Request.Name}.");

            switch (campaignType.CampaignTypeCode)
            {
                case CampaignTypeCode.AutoAll:
                    break;
                case CampaignTypeCode.AutoEnterSweepsFromHashtag:
                case CampaignTypeCode.AutoEnterSweepsFromTweet:
                    await CreateSweepsForCampaignAsync(notification.CampaignUid);
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

        private async Task CreateSweepsForCampaignAsync(Guid campaignUid)
        {
            var campaign = await _campaignRepository.GetCampaignByUidAsync(campaignUid);
            if (campaign == null)
                throw new InvalidCampaignException($"An error occurred while trying to run the method {nameof(CreateSweepsForCampaignAsync)} to create a sweeps for the campaignUid {campaignUid}");

            await _sweepsClient.CreateSweepsForCampaignAsync(campaign.CampaignUid, campaign.StartDate, campaign.EndDate);
        }
    }
}
