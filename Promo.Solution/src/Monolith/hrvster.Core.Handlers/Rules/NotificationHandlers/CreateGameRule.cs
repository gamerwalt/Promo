using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Harvesters.Twitter;
using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Requests.Rules.Notifications;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Core;
using hrvster.Infrastructure.MatchGames;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Sweepstakes;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Rules.NotificationHandlers
{
	public class CreateGameRule : INotificationHandler<RulePublished>
	{
		private readonly ILogger<CreateGameRule> _logger;
		private readonly IWatchTypesRepository _watchTypesRepository;
		private readonly ISweepRulesRepository _sweepRulesRepository;
		private readonly IMatchGameRulesRepository _matchGameRulesRepository;
		private readonly IHarvesterService _harvesterService;
		private readonly IAccountsService _accountsService;
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly ISweepRulesPageRepository _sweepRulesPageRepository;
		private readonly IMatchRulePageRepository _matchRulePageRepository;

		public CreateGameRule(
			ILoggerFactory loggerFactory,
			IWatchTypesRepository watchTypesRepository,
			ISweepRulesRepository sweepRulesRepository,
			IMatchGameRulesRepository matchGameRulesRepository,
			IHarvesterService harvesterService,
			IAccountsService accountsService,
			IAccountSubscriptionService accountSubscriptionService,
			ISweepRulesPageRepository sweepRulesPageRepository,
			IMatchRulePageRepository matchRulePageRepository)
		{
			_logger = loggerFactory.CreateLogger<CreateGameRule>();
			_watchTypesRepository = watchTypesRepository;
			_sweepRulesRepository = sweepRulesRepository;
			_matchGameRulesRepository = matchGameRulesRepository;
			_harvesterService = harvesterService;
			_accountsService = accountsService;
			_accountSubscriptionService = accountSubscriptionService;
			_sweepRulesPageRepository = sweepRulesPageRepository;
			_matchRulePageRepository = matchRulePageRepository;
		}

		public async Task Handle(RulePublished notification, CancellationToken cancellationToken)
		{
			var accountSubscription
				= await _accountSubscriptionService.GetAccountSubscriptionAsync(notification.Profile.Account.AccountUid);

			if (accountSubscription == null)
			{
				_logger.LogWarning("Account with uid {accountUid} is not subscribed.", notification.Profile.Account.AccountUid);

				return;
			}

			var ruleCount = await GetGameRuleCountsForAsync(notification.Profile.Account.AccountUid);

			if (ruleCount >= accountSubscription.Plan.GetHashtagCount())
			{
				_logger.LogWarning("Account with uid {accountUid} has a total of {activeRulesCount} active hashtags and their current max is {maxHashtags}", notification.Profile.Account.AccountUid,
				ruleCount,
						accountSubscription.Plan.HashtagsPerMonth);

				return;
			}

			var watchTypes = await _watchTypesRepository.GetWatchTypesAsync();
			var watchTypeCodeSelected = watchTypes.SingleOrDefault(c => c.WatchTypeId.Equals(notification.Message.WatchTypeId)).WatchTypeCode;

			await CreateGameRuleAsync(notification, watchTypeCodeSelected);

			await _harvesterService.RestartAsync(cancellationToken);
		}

		private async Task CreateGameRuleAsync(RulePublished notification, string watchTypeCodeSelected)
		{
			switch (notification.GameType)
			{
				case GameTypesEnum.Sweeps:
					await CreateSweepRuleAsync(notification, watchTypeCodeSelected);
					break;
				case GameTypesEnum.MatchGames:
					await CreateMatchRuleAsync(notification, watchTypeCodeSelected);
					break;
			}

		}

		private async Task CreateMatchRuleAsync(RulePublished notification, string watchTypeCodeSelected)
		{
			await _matchGameRulesRepository.CreateAsync(new Rule
			{
				RuleUid = notification.RuleUid,
				Game = new Games.Match.Models.MatchGame
				{
					Uid = notification.GameUid,
					Id = notification.GameId
				},
				WatchTypeCode = Enum.Parse<WatchTypeCode>(watchTypeCodeSelected),
				WatchValue = notification.Message.Hashtag,
				ExternalId = notification.ExternalId,
				ActiveInd = true,
				MustBeFollowing = notification.MustBeFollowing,
				ScheduledInd = false,
				NotifyWinners = true,
				ExternalClientType = notification.ExternalClientType,
				UrlLink = notification.UrlLink
			}, default);

			var rule = await _matchGameRulesRepository.GetAsync(notification.RuleUid);

			await _matchRulePageRepository.CreateAsync(new MatchRulePage
			{
				Uid = Guid.NewGuid(),
				PageId = notification.PageId,
				MatchRuleId = rule.RuleId
			});
		}

		private async Task CreateSweepRuleAsync(RulePublished notification, string watchTypeCodeSelected)
		{
			await _sweepRulesRepository.CreateRuleAsync(new Rule
			{
				RuleUid = notification.RuleUid,
				Sweep = new Sweep
				{
					SweepUid = notification.Message.SweepUid == Guid.Empty ? notification.GameUid : notification.Message.SweepUid,
				},
				WatchTypeCode = (WatchTypeCode)Enum.Parse(typeof(WatchTypeCode), watchTypeCodeSelected),
				WatchValue = notification.Message.Hashtag,
				ExternalId = notification.ExternalId,
				ActiveInd = true,
				MustBeFollowing = notification.MustBeFollowing,
				ScheduledInd = false,
				ExternalClientType = notification.ExternalClientType,
				UrlLink = notification.UrlLink
			});

			var rule = await _sweepRulesRepository.GetAsync(notification.RuleUid);

			await _sweepRulesPageRepository.CreateAsync(new SweepRulesPage
			{
				Uid = Guid.NewGuid(),
				PageId = notification.PageId,
				SweepRuleId = rule.RuleId
			});
		}

		private async Task<int> GetGameRuleCountsForAsync(Guid userUid)
		{
			var rules = await _accountsService.GetAllRulesAsync(userUid);

			if (rules == null) return 0;

			return rules.GetActiveRules().Count();
		}
	}
}
