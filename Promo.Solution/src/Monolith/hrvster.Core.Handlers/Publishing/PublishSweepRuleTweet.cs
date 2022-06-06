using hrvster.Core.Clients.Common;
using hrvster.Core.Requests.Notifications.Publishing;
using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Extensions;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Publishing
{
	public class PublishSweepRuleTweet : AsyncRequestHandler<PublishSweepRuleTweetRequest>
	{
		private readonly ILogger<PublishSweepRuleTweet> _logger;
		private readonly Func<PublisherType, Clients.Publishers.IPublisher> _publisherResolver;
		private readonly IMediator _mediator;

		public PublishSweepRuleTweet(
			ILoggerFactory loggerFactory,
			Func<PublisherType, Clients.Publishers.IPublisher> publisherResolver,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<PublishSweepRuleTweet>();
			_publisherResolver = publisherResolver;
			_mediator = mediator;
		}

		protected async override Task Handle(PublishSweepRuleTweetRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling {action} for the following payload {payload}",
				nameof(PublishSweepRuleTweet),
				request.AsJson());

			var twitterPublisher = _publisherResolver(PublisherType.Twitter);

			var message = new Message
			{
				WatchTypeId = request.WatchTypeId,
				Hashtag = request.WatchValue,
				SweepUid = request.Sweep.SweepUid,
				SweepName = request.Sweep.Name,
				Profile = request.Profile,
				Text = request.TweetText,
				IsScheduled = !request.PostNow,
				ScheduledDate = request.ScheduledDate,
				Media = request.MediaLocation
			};

			var tweetId = await twitterPublisher.PublishAsync(message);

			await _mediator.Publish(new SweepRuleTweetPublished(tweetId, message));
		}
	}
}
