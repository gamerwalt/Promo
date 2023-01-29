using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Clients.Common;
using hrvster.Core.Requests.Notifications.Publishing;
using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Core;
using hrvster.Infrastructure.Extensions;
using hrvster.Infrastructure.Scheduling;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Profiles;
using hrvster.Models.Scheduling;
using MediatR;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Publishing
{
	public class PublishSweepRuleTweet : IRequestHandler<PublishSweepRuleTweetRequest, bool>
	{
		private readonly ILogger<PublishSweepRuleTweet> _logger;
		private readonly Func<ExternalClientType, Clients.Publishers.IExternalClient> _publisherResolver;
		private readonly IMediator _mediator;
		private readonly ISweepRulesRepository _sweepRulesRepository;
		private readonly ISweepTweetSchedulerRepository _repository;
		private readonly IWatchTypesRepository _watchTypesRepository;
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly IToastNotification _notificationService;

		public PublishSweepRuleTweet(
			ILoggerFactory loggerFactory,
			Func<ExternalClientType, Clients.Publishers.IExternalClient> publisherResolver,
			IMediator mediator,
			ISweepRulesRepository sweepRulesRepository,
			ISweepTweetSchedulerRepository repository,
			IWatchTypesRepository watchTypesRepository,
			IAccountSubscriptionService accountSubscriptionService,
			IToastNotification notificationService)
		{
			_logger = loggerFactory.CreateLogger<PublishSweepRuleTweet>();
			_publisherResolver = publisherResolver;
			_mediator = mediator;
			_sweepRulesRepository = sweepRulesRepository;
			_repository = repository;
			_watchTypesRepository = watchTypesRepository;
			_accountSubscriptionService = accountSubscriptionService;
			_notificationService = notificationService;
		}

		public async Task<bool> Handle(PublishSweepRuleTweetRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling {action} for the following payload {payload}",
				nameof(PublishSweepRuleTweet),
				request.AsJson());

			if (!await CanPublishRequestAsync(request))
			{
				return false;
			}

			var message = BuildMessage(request);

			return request.PostNow
				? await PublishMessageAsync(message, request.MustBeFollowing)
				: await SaveScheduledTweetAsync(message, request.MustBeFollowing, request.TimeZoneId, request.IsDaylight);
		}

		private async Task<bool> PublishMessageAsync(Message message, bool following)
		{
			var twitterPublisher = _publisherResolver(ExternalClientType.Twitter);

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

			await _mediator.Publish(new SweepRuleTweetPublished(tweetId, message, following));

			return true;
		}

		private static Message BuildMessage(PublishSweepRuleTweetRequest request)
		{
			return new Message
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
		}

		private async Task<bool> CanPublishRequestAsync(PublishSweepRuleTweetRequest request)
		{
			var accountSubscription = await _accountSubscriptionService.GetAccountSubscriptionAsync(request.Profile.Account.AccountUid);

			if (accountSubscription == null)
			{
				_logger.LogWarning("Account with uid {accountUid} is not subscribed.", request.Profile.Account.AccountUid);

				return false;
			}

			var rules = await _sweepRulesRepository.GetSweepRuleAsync(request.Sweep.SweepUid);
			var activeRules = rules.Where(r => (r.ActiveInd && r.Sweep.IsActive()) || r.Sweep.WillBeInProgress()).ToList();

			if (accountSubscription.Plan.SweepstakesPerMonth != 0)
			{
				if (activeRules.Count >= accountSubscription.Plan.HashtagsPerMonth)
				{
					_logger.LogWarning("Account with uid {accountUid} has a total of {activeRulesCount} active hashtags and their current max is {maxHashtags}",
						request.Profile.Account.AccountUid,
						activeRules.Count,
						accountSubscription.Plan.HashtagsPerMonth);

					_notificationService.AddErrorToastMessage($"You cannot create more hashtags.");
					_notificationService.AddInfoToastMessage($"You can only have {accountSubscription.Plan.HashtagsPerMonth} active hashtags per month.");

					return false;
				}
			}

			return true;
		}

		private async Task<bool> SaveScheduledTweetAsync(Message message, bool mustBeFollowing, string timeZoneId, bool isDaylight)
		{
			try
			{
				await _repository.BeginTransaction();

				var ruleUid = await SaveRule(message, mustBeFollowing);
				var uid = await SaveScheduledTweet(ruleUid, message, timeZoneId, isDaylight);
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
			if (message.Media != null)
			{
				foreach (var media in message.Media)
				{
					await _repository.CreateScheduledSweepRuleImage(scheduledUid, new ScheduledSweepRuleTweetImage
					{
						ImageLocation = media
					});
				}
			}
		}

		private async Task<Guid> SaveScheduledTweet(Guid ruleUid, Message message, string timeZoneId, bool isDaylight)
		{
			var scheduledUid = Guid.NewGuid();

			await _repository.CreateScheduledSweepRuleTweetAsync(new ScheduledSweepRuleTweet
			{
				ScheduledUid = scheduledUid,
				Profile = new Profile
				{
					ProfileUid = message.Profile.ProfileUid
				},
				Rule = new Rule
				{
					RuleUid = ruleUid
				},
				ScheduledTime = message.ScheduledDate,
				ScheduledTimeUtc = message.ScheduledDate.ToUniversalTime(),
				Status = ScheduledStatus.New,
				Text = message.Text
			}, timeZoneId, isDaylight);

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
