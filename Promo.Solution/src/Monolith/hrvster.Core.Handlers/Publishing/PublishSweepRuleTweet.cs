using hrvster.Core.Clients.Common;
using hrvster.Core.Requests.Notifications.Publishing;
using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Extensions;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Core;
using hrvster.Infrastructure.Scheduling;
using hrvster.Models;
using hrvster.Models.Scheduling;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Publishing
{
	public class PublishSweepRuleTweet : IRequestHandler<PublishSweepRuleTweetRequest, bool>
	{
		private readonly ILogger<PublishSweepRuleTweet> _logger;
		private readonly Func<PublisherType, Clients.Publishers.IPublisher> _publisherResolver;
		private readonly IMediator _mediator;
		private readonly ISweepTweetSchedulerRepository _repository;
		private readonly IWatchTypesRepository _watchTypesRepository;

		public PublishSweepRuleTweet(
			ILoggerFactory loggerFactory,
			Func<PublisherType, Clients.Publishers.IPublisher> publisherResolver,
			IMediator mediator,
			ISweepTweetSchedulerRepository repository,
			IWatchTypesRepository watchTypesRepository)
		{
			_logger = loggerFactory.CreateLogger<PublishSweepRuleTweet>();
			_publisherResolver = publisherResolver;
			_mediator = mediator;
			_repository = repository;
			_watchTypesRepository = watchTypesRepository;
		}

		public async Task<bool> Handle(PublishSweepRuleTweetRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling {action} for the following payload {payload}",
				nameof(PublishSweepRuleTweet),
				request.AsJson());

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

			if(!request.PostNow)
			{
				return await SaveScheduledTweetAsync(message, request.MustBeFollowing);
			}

			var twitterPublisher = _publisherResolver(PublisherType.Twitter);
			var tweetId = await twitterPublisher.PublishAsync(message);

			if (string.IsNullOrWhiteSpace(tweetId))
			{
				_logger.LogError("Unable to send the tweet {text} for sweep name and uid {name}, {uid} for {profileName} and {profileUid}",
					message.Text,
					message.SweepName,
					message.SweepUid,
					message.Profile.ScreenName,
					message.Profile.ProfileUid);

				return false;
			}

			await _mediator.Publish(new SweepRuleTweetPublished(tweetId, message, request.MustBeFollowing));

			return true;
		}

		private async Task<bool> SaveScheduledTweetAsync(Message message, bool mustBeFollowing)
		{
			try
			{
				await _repository.BeginTransaction();

				var ruleUid = await SaveRule(message, mustBeFollowing);
				var uid = await SaveScheduledTweet(ruleUid, message);
				await SaveScheduledTweetImages(uid, message);

				await _repository.CommitTransaction();

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occurred trying to save the Scheduled Tweet. Exception Message: {message}",
					ex.Message);

				await _repository.RollbackTransaction();
			}

			return false;
		}

		private async Task SaveScheduledTweetImages(Guid scheduledUid, Message message)
		{
			foreach(var media in message.Media)
			{
				await _repository.CreateScheduledSweepRuleImage(scheduledUid, new ScheduledSweepRuleTweetImage
				{
					ImageLocation = media
				});
			}
		}

		private async Task<Guid> SaveScheduledTweet(Guid ruleUid, Message message)
		{
			var scheduledUid = Guid.NewGuid();

			await _repository.CreateScheduledSweepRuleTweetAsync(new ScheduledSweepRuleTweet
			{
				ScheduledUid = scheduledUid,
				Rule = new Rule
				{
					RuleUid = ruleUid
				},
				ScheduledTime = message.ScheduledDate,
				ScheduledTimeUtc = message.ScheduledDate.ToUniversalTime(),
				Status = ScheduledStatus.New,
				Text = message.Text
			});

			return scheduledUid;
		}

		private async Task<Guid> SaveRule(Message message, bool mustBeFollowing)
		{
			var watchTypes = await _watchTypesRepository.GetWatchTypesAsync();
			var watchTypeCodeSelected = watchTypes.SingleOrDefault(c => c.WatchTypeId.Equals(message.WatchTypeId)).WatchTypeCode;

			var ruleUid = Guid.NewGuid();

			await _repository.CreateRuleAsync(new Rule
			{
				RuleUid = ruleUid,
				Sweep = new Sweep
				{
					SweepUid = message.SweepUid,
				},
				WatchTypeCode = (WatchTypeCode)Enum.Parse(typeof(WatchTypeCode), watchTypeCodeSelected),
				WatchValue = message.Hashtag,
				ExternalId = 0,
				ActiveInd = true,
				MustBeFollowing = mustBeFollowing,
				ScheduledInd = true
			});

			return ruleUid;
		}
	}
}
