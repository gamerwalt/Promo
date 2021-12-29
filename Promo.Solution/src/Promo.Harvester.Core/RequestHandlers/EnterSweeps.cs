using MediatR;
using Promo.Clients.Internal;
using Promo.Core;
using Promo.Core.Models;
using Promo.Harvester.Core.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Harvester.Core.RequestHandlers
{
    public class EnterSweeps : IRequest
    {
        private NewHashTagTweetReceived notification;
        private Campaign campaign;

        public EnterSweeps(NewHashTagTweetReceived notification, Campaign campaign)
        {
            this.notification = notification;
            this.campaign = campaign;
        }

        public NewHashTagTweetReceived TweetReceived
        {
            get { return notification; }
        }

        public Campaign Campaign
        {
            get { return campaign; }
        }
    }

    public class EnterSweepsHandler : AsyncRequestHandler<EnterSweeps>
    {
        private readonly ISweepsClient _sweepsClient;
        private readonly IMediator _mediator;

        public EnterSweepsHandler(ISweepsClient sweepsClient, IMediator mediator)
        {
            _sweepsClient = sweepsClient;
            _mediator = mediator;
        }

        protected async override Task Handle(EnterSweeps request, CancellationToken cancellationToken)
        {
            await _sweepsClient.CreateSweepsEntryAsync(
                entryCount: 1,
                campaignUid: request.Campaign.CampaignUid,
                screenName: $"@{request.TweetReceived.Tweet.CreatedBy.ScreenName}",
                userIdentifierType: UserIdentifierType.TwitterHandle.ToString(),
                acceptedTerms: true);

            await _mediator.Publish(
                new SweepEntryRecorded(
                    request.Campaign.CampaignUid, 
                    request.TweetReceived.Tweet.CreatedBy.ScreenName, 
                    request.TweetReceived.Tweet.CreatedBy.IdStr,
                    UserIdentifierType.TwitterHandle.ToString()
                    )
                );
        }
    }
}
