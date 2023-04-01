using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.MatchGames;
using hrvster.ApplicationServices.Publishing;
using hrvster.ApplicationServices.Subscription;
using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Files;
using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Games.Match.Models;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Profiles;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;

namespace hrvster.ApplicationServices
{
	public interface IGameService
	{
		Task CreateRuleTweetAsync(HrvsterUser hrvsterUser, string gameType, Guid gameUid, int gameId, int watchTypeId, string watchValue, string tweetText, bool postNow, DateTime scheduledDate, IFormFile media1, IFormFile media2, IFormFile media3, IFormFile media4, bool following);
		Task<GameDetail> GetGameDetailsAsync(string gameType, Guid uid);
		Task<IEnumerable<Rule>> GetRulesAsync(GameTypesEnum gameType, CancellationToken cancellationToken);
		IEnumerable<Rule> GetRulesMatching(string gameType, string watchValue);
	}

	public class GameService : IGameService
	{
		private readonly ILogger<GameService> _logger;
		private readonly ISweepstakesService _sweepService;
		private readonly IMatchGamesService _matchGameService;
		private readonly IMediator _mediator;
		private readonly IPublishService _publishService;
		private readonly IAccountsService _accountsService;
		private readonly IAccountSubscriptionService _accountSubscriptionService;

		public GameService(
			ILoggerFactory loggerFactory,
			ISweepstakesService sweepstakesService,
			IMatchGamesService matchGamesService,
			IMediator mediator,
			IPublishService publishService,
			IAccountsService accountsService,
			IAccountSubscriptionService accountSubscriptionService)
		{
			_logger = loggerFactory.CreateLogger<GameService>();
			_sweepService = sweepstakesService;
			_matchGameService = matchGamesService;
			_mediator = mediator;
			_publishService = publishService;
			_accountsService = accountsService;
			_accountSubscriptionService = accountSubscriptionService;
		}

		public async Task<IEnumerable<Rule>> GetRulesAsync(GameTypesEnum gameType, CancellationToken cancellationToken)
		{
			var rules = new List<Rule>();

			switch(gameType)
			{
				case GameTypesEnum.MatchGames:
					rules.AddRange(await GetMatchGameRulesAsync(cancellationToken));
					break;
			}

			return rules;
		}

		private async Task<IEnumerable<Rule>> GetMatchGameRulesAsync(CancellationToken cancellationToken)
			=> await _matchGameService.GetRulesAsync(cancellationToken);

		public async Task<GameDetail> GetGameDetailsAsync(string gameType, Guid uid)
		{
			switch (Enum.Parse<GameTypesEnum>(gameType))
			{
				case GameTypesEnum.Sweeps:
					return await GetSweepGameDetailsAsync(uid);
				case GameTypesEnum.MatchGames:
					return await GetMatchGameDetailsAsync(uid);
				default:
					return await GetSweepGameDetailsAsync(uid);
			}
		}

		public IEnumerable<Rule> GetRulesMatching(string gameType, string watchValue)
		{
			switch(Enum.Parse<GameTypesEnum>(gameType))
			{
				case GameTypesEnum.Sweeps:
					return GetRulesMatchingForSweeps(watchValue);
				case GameTypesEnum.MatchGames:
					return GetRulesMatchingForMatchGames(watchValue);
				default:
					return GetRulesMatchingForSweeps(watchValue);
			}
		}

		private IEnumerable<Rule> GetRulesMatchingForMatchGames(string watchValue)
			=> _matchGameService.GetRulesMatching(watchValue);

		private IEnumerable<Rule> GetRulesMatchingForSweeps(string watchValue)
			=> _sweepService.GetRulesMatching(watchValue);

		private async Task<GameDetail> GetMatchGameDetailsAsync(Guid uid)
		{
			var gameDetail = default(GameDetail);

			_logger.LogInformation("Loading the game detail for match game with uid {MatchGameUid}", uid);

			var matchGame = await _matchGameService.GetMatchGameAsync(uid, default);

			if (matchGame != null)
			{
				return new GameDetail
				{
					Name = matchGame.Name,
					StartDate = matchGame.StartDate,
					EndDate = matchGame.EndDate,
					Id = matchGame.Id,
					Uid = uid
				};
			}

			_logger.LogWarning("Could not load the match game with Uid {MatchGameUid}", uid);

			return gameDetail;
		}

		private async Task<GameDetail> GetSweepGameDetailsAsync(Guid uid)
		{
			var gameDetail = default(GameDetail);

			_logger.LogInformation("Loading the game detail for sweepstake with uid {sweepUid}", uid);

			var sweepstake = await _sweepService.GetSweepstakeAsync(uid, default);

			if (sweepstake != null)
			{
				return new GameDetail
				{
					Name = sweepstake.Name,
					StartDate = sweepstake.StartDate,
					EndDate = sweepstake.EndDate,
					Id = sweepstake.SweepId,
					Uid = uid
				};
			}

			_logger.LogWarning("Could not load the Sweepstake with Uid {sweepUid}", uid);

			return gameDetail;
		}

		public async Task CreateRuleTweetAsync(
			HrvsterUser hrvsterUser,
			string gameType, 
			Guid gameUid, 
			int gameId, 
			int watchTypeId, 
			string watchValue, 
			string tweetText, 
			bool postNow, 
			DateTime scheduledDate, 
			IFormFile media1, 
			IFormFile media2, 
			IFormFile media3, 
			IFormFile media4, bool following)
		{
			//1. Publish the tweet
			//2. Create the rule with the ID
			//3. If it's immediate, we need to publish an event to add it

			var media = new List<IFormFile>();
			var mediaLocation = default(IEnumerable<string>);

			if (media1 != null && media1.Length > 0)
				media.Add(media1);
			if (media2 != null && media2.Length > 0)
				media.Add(media2);
			if (media3 != null && media3.Length > 0)
				media.Add(media3);
			if (media4 != null && media4.Length > 0)
				media.Add(media4);

			if (media.Count > 0)
				mediaLocation = await _mediator.Send(
					new UploadFileToFileServerRequest(media, hrvsterUser.UserUid, ProfileType.Twitter, hrvsterUser.Token));

			switch (Enum.Parse<GameTypesEnum>(gameType))
			{
				case GameTypesEnum.Sweeps:
					//await _mediator.Send(PublishSweepRuleTweetRequest())
					break;
				case GameTypesEnum.MatchGames:
					if(await CreateMatchRuleTweetAsync(
						gameUid,
						gameId,
						GameTypesEnum.MatchGames,
						media,
						mediaLocation,
						hrvsterUser, 
						watchValue,
						watchTypeId,
						tweetText, 
						postNow, 
						scheduledDate, 
						ExternalClientType.Twitter, following));
					//await _mediator.Send(PublishMatchGameRuleTweetRequest())
					break;
			}
		}

		private async Task<bool> CreateMatchRuleTweetAsync(
			Guid gameUid,
			int gameId,
			GameTypesEnum gameType,
			IList<IFormFile> media,
			IEnumerable<string> mediaLocation,
			HrvsterUser hrvsterUser, 
			string watchValue,
			int watchTypeId,
			string tweetText, 
			bool postNow, 
			DateTime scheduledDate, 
			ExternalClientType externalClientType,
			bool following)
		{
			var externalId = string.Empty;

			var profile = await _accountsService.GetProfileAsync(hrvsterUser.UserUid, ProfileType.Twitter);

			var accountSubscription = await _accountSubscriptionService.GetAccountSubscriptionAsync(profile.Account.AccountUid);

			if (accountSubscription == null)
			{
				_logger.LogWarning("Account with uid {accountUid} is not subscribed.", profile.Account.AccountUid);

				return false;
			}

			var ruleCount = await GetGameRuleCountsForAsync(hrvsterUser.UserUid);

			if(ruleCount >= accountSubscription.Plan.HashtagsPerMonth)
			{
				_logger.LogWarning("Account with uid {accountUid} has a total of {activeRulesCount} active hashtags and their current max is {maxHashtags}",
						profile.Account.AccountUid,
						ruleCount,
						accountSubscription.Plan.HashtagsPerMonth);

				return false;
			}

			if (postNow)
				return await _publishService.PublishAsync(
					profile, 
					gameId, 
					gameUid, 
					gameType, 
					media, 
					mediaLocation == null ? null : mediaLocation.ToList(), 
					hrvsterUser.Token, 
					hrvsterUser.UserUid, 
					watchValue, 
					watchTypeId, 
					tweetText, 
					externalClientType, 
					following);

			return await _publishService.SaveScheduledTweetToPublishAsync(hrvsterUser.UserUid, gameId, gameUid, profile, watchTypeId, watchValue, tweetText, scheduledDate, mediaLocation.ToList(), externalClientType, following);
		}

		private async Task<int> GetGameRuleCountsForAsync(Guid userUid)
		{
			var rules = await _accountsService.GetAllRulesAsync(userUid);

			if(rules == null)
			{
				return 0;
			}

			var activeRules = rules.Where(r => (r.ActiveInd && r.Game.IsActive()) || r.Game.WillBeInProgress()).ToList();

			return activeRules.Count();
		}
	}
}
