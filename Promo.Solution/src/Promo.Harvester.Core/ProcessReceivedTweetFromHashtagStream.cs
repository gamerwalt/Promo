using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Clients.Internal;
using Promo.Core;
using Promo.Core.Models;
using Promo.Harvester.Core.Notifications;
using Promo.Harvester.Core.RequestHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
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
			var sweeps = await GetCampaignSweepsAsync(notification.Rule.CampaignUid);

			var sweepStakesToEnter = sweeps.FirstOrDefault(c => notification.Rule.SweepUid.Equals(c.SweepUid));

			if (campaign == null || sweepStakesToEnter == null)
				throw new Exception("Campaign or Sweeps is null");

            switch (campaign.CampaignType.CampaignTypeCode)
            {
                case CampaignTypeCode.AutoAll:
                    break;
				case CampaignTypeCode.AutoEnterSweepsFromHashtag:
					await EnterSweepsAsync(notification, campaign, sweepStakesToEnter);
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

		private async Task<IEnumerable<Sweep>> GetCampaignSweepsAsync(Guid campaignUid) 
			=> await _sweepsClient.GetCampaignSweepsAsync(campaignUid);

		private async Task EnterSweepsAsync(NewHashTagTweetReceived notification, Campaign campaign, Sweep sweep)
		{
			if(campaign.ActiveInd && CampaignIsRunning(campaign) && SweepsIsRunningAndNotYetDrawn(sweep))
			{
				_logger.LogInformation("{action} called for CampaignName: {campaignName}", nameof(EnterSweepsAsync), campaign.CampaignName);

				await _mediator.Send(new EnterSweeps(notification, campaign, sweep));

				return;
			}

			_logger.LogInformation("Could not enter sweep for Campaign : {campaignName}", 
				campaign.CampaignName);
			
			_logger.LogInformation("Campaign Details - Start Date: {startDate}, End Date: {endDate}", 
				campaign.StartDate.ToShortDateString(), 
				campaign.EndDate.ToShortDateString());
			
			_logger.LogInformation("Sweepstakes Details - Start Date: {startDate}, End Date: {endDate}, Draw Date: {drawDate}",
				sweep.StartDate.ToShortDateString(),
				sweep.EndDate.ToShortDateString(),
				sweep.DrawDate.GetValueOrDefault().ToShortDateString());

			//Todo: DO we want to inform the user that the sweeps is not active yet?
			await _mediator.Publish(new UnableToEnterSweeps(notification, campaign, sweep));
		}

		private bool SweepsIsRunningAndNotYetDrawn(Sweep sweep) 
			=> DateTime.UtcNow < sweep.EndDate && DateTime.UtcNow > sweep.StartDate && DateTime.UtcNow < sweep.DrawDate;

		private bool CampaignIsRunning(Campaign campaign) 
			=> DateTime.UtcNow < campaign.EndDate && DateTime.UtcNow > campaign.StartDate;

		private async Task<Campaign> GetCampaignAsync(Guid campaignUid) 
			=> await _campaignsClient.GetCampaignAsync(campaignUid);
	}
}
