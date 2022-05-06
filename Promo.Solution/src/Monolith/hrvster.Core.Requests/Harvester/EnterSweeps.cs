using hrvster.Core.Requests.Notifications.Harvester;
using hrvster.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.Harvester
{
	public class EnterSweeps : IRequest
	{
		private NewHashTagTweetReceived notification;
		private Campaign campaign;
		private Sweep sweep;

		public EnterSweeps(NewHashTagTweetReceived notification, Campaign campaign, Sweep sweep)
		{
			this.notification = notification;
			this.campaign = campaign;
			this.sweep = sweep;
		}

		public Sweep Sweep
		{
			get { return sweep; }
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
		//private readonly ISweepsClient _sweepsClient;
		private readonly IMediator _mediator;

		public EnterSweepsHandler(IMediator mediator)//(ISweepsClient sweepsClient, IMediator mediator)
		{
			//_sweepsClient = sweepsClient;
			_mediator = mediator;
		}

		protected async override Task Handle(EnterSweeps request, CancellationToken cancellationToken)
		{
			//await _sweepsClient.CreateSweepsEntryAsync(
			//	entryCount: 1,
			//	sweepUid: request.Sweep.SweepUid,
			//	campaignUid: request.Campaign.CampaignUid,
			//	screenName: $"@{request.TweetReceived.Tweet.CreatedBy.ScreenName}",
			//	userIdentifierType: UserIdentifierType.TwitterHandle.ToString(),
			//	acceptedTerms: true);

			await _mediator.Publish(
				new SweepEntryRecorded(
					request.Campaign.CampaignUid,
					request.Sweep.SweepUid,
					request.TweetReceived.Tweet.CreatedBy.ScreenName,
					request.TweetReceived.Tweet.CreatedBy.IdStr,
					request.TweetReceived.Tweet.IdStr,
					UserIdentifierType.TwitterHandle.ToString()
					)
				);
		}
	}
}
