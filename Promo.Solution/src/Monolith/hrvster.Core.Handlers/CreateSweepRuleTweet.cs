using hrvster.ApplicationServices.Harvesters.Twitter;
using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Requests.Notifications.Publishing;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Core;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.Notifications.Handlers
{
	public class CreateSweepRuleTweet : INotificationHandler<SweepRuleTweetPublished>
	{
		private readonly ILogger<CreateSweepRuleTweet> _logger;
		private readonly IWatchTypesRepository _watchTypesRepository;
		private readonly ISweepRulesRepository _repository;
		private readonly IHarvesterService _harvesterService;
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly IToastNotification _notificationService;

		public CreateSweepRuleTweet(
			ILoggerFactory loggerFactory,
			IWatchTypesRepository watchTypesRepository,
			ISweepRulesRepository repository,
			IHarvesterService harvesterService,
			IAccountSubscriptionService accountSubscriptionService,
			IToastNotification notificationService)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepRuleTweet>();
			_watchTypesRepository = watchTypesRepository;
			_repository = repository;
			_harvesterService = harvesterService;
			_accountSubscriptionService = accountSubscriptionService;
			_notificationService = notificationService;
		}

		public async Task Handle(SweepRuleTweetPublished notification, CancellationToken cancellationToken)
		{
			var accountSubscription = await _accountSubscriptionService.GetAccountSubscriptionAsync(notification.Message.Profile.Account.AccountUid);

			if (accountSubscription == null)
			{
				_logger.LogWarning("Account with uid {accountUid} is not subscribed.", notification.Message.Profile.Account.AccountUid);

				return;
			}

			var rules = await _repository.GetSweepRuleAsync(notification.Message.SweepUid);
			var activeRules = rules.Where(r => r.ActiveInd).ToList();

			if (accountSubscription.Plan.SweepstakesPerMonth != 0)
			{
				if (activeRules.Count >= accountSubscription.Plan.HashtagsPerMonth)
				{
					_logger.LogWarning("Account with uid {accountUid} has a total of {activeRulesCount} active hashtags and their current max is {maxHashtags}",
						notification.Message.Profile.Account.AccountUid,
						activeRules.Count,
						accountSubscription.Plan.HashtagsPerMonth);

					_notificationService.AddErrorToastMessage($"You cannot create more hashtags.");
					_notificationService.AddInfoToastMessage($"You can only have {accountSubscription.Plan.HashtagsPerMonth} active hashtags per month.");

					return;
				}
			}

			var ruleUid = Guid.NewGuid();

			var watchTypes = await _watchTypesRepository.GetWatchTypesAsync();
			var watchTypeCodeSelected = watchTypes.SingleOrDefault(c => c.WatchTypeId.Equals(notification.Message.WatchTypeId)).WatchTypeCode;

			await _repository.CreateRuleAsync(new Rule
			{
				RuleUid = ruleUid,
				Sweep = new Sweep
				{
					SweepUid = notification.Message.SweepUid,
				},
				WatchTypeCode = (WatchTypeCode)Enum.Parse(typeof(WatchTypeCode), watchTypeCodeSelected),
				WatchValue = notification.Message.Hashtag,
				ExternalId = long.Parse(notification.TweetId),
				ActiveInd = true,
				MustBeFollowing = notification.MustBeFollowing,
				ScheduledInd = false
			});

			await _harvesterService.RestartAsync(cancellationToken);
		}
	}
}
