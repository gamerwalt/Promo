using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Clients.Internal;
using Promo.Core;
using Promo.Core.Models;
using Promo.Harvester.Core.Notifications;
using Promo.Harvester.Core.RequestHandlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Harvester.Core
{
    /// <summary>
    /// This will process all hashtags tweets and retweets
    /// </summary>
    public class ProcessReceivedTweetFromHashtagStream : INotificationHandler<NewHashTagTweetReceived>
    {
        private readonly ICampaignsClient _campaignsClient;
		private readonly ISweepsClient _sweepsClient;
        private readonly IMediator _mediator;
        private ILogger<ProcessReceivedTweetFromHashtagStream> _logger;

        public ProcessReceivedTweetFromHashtagStream(
            ICampaignsClient campaignsClient,
			ISweepsClient sweepsClient,
            IMediator mediator,
            ILoggerFactory loggerFactory)
        {
            _campaignsClient = campaignsClient;
			_sweepsClient = sweepsClient;
            _mediator = mediator;
            _logger = loggerFactory.CreateLogger<ProcessReceivedTweetFromHashtagStream>();
        }

        //Ideally, we should just send this to a queue to handle this process
        public async Task Handle(NewHashTagTweetReceived notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started handling {action}", nameof(ProcessReceivedTweetFromHashtagStream));

            var campaign = await GetCampaignAsync(notification.Rule.CampaignUid);
			var sweep = await GetCampaignSweepsAsync(notification.Rule.CampaignUid);

            switch (campaign.CampaignType.CampaignTypeCode)
            {
                case CampaignTypeCode.AutoAll:
                    break;
				case CampaignTypeCode.AutoEnterSweepsFromHashtag:
					await EnterSweepsAsync(notification, campaign, sweep);
					break;
				case CampaignTypeCode.AutoEnterSweepsFromTweet:
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

            _logger.LogInformation("Finished handling {action}", nameof(ProcessReceivedTweetFromHashtagStream));
        }

		private async Task<Sweep> GetCampaignSweepsAsync(Guid campaignUid) 
			=> await _sweepsClient.GetCampaignSweeps(campaignUid);

		private async Task EnterSweepsAsync(NewHashTagTweetReceived notification, Campaign campaign, Sweep sweep)
		{
			if(campaign.ActiveInd && CampaignIsRunning(campaign) && SweepsIsRunningAndNotYetDrawn(sweep))
			{
				await _mediator.Send(new EnterSweeps(notification, campaign));

				_logger.LogInformation("{action} called for CampaignName: {campaignName}", nameof(EnterSweepsAsync), campaign.CampaignName);
			}
		}

		private bool SweepsIsRunningAndNotYetDrawn(Sweep sweep) 
			=> DateTime.UtcNow < sweep.EndDate && DateTime.UtcNow > sweep.StartDate && DateTime.UtcNow < sweep.DrawDate;

		private bool CampaignIsRunning(Campaign campaign) 
			=> DateTime.UtcNow < campaign.EndDate && DateTime.UtcNow > campaign.StartDate;

		private async Task<Campaign> GetCampaignAsync(Guid campaignUid) 
			=> await _campaignsClient.GetCampaignAsync(campaignUid);
	}
}
