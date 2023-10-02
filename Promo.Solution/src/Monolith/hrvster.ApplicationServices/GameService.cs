using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Extensions.LogExtensions;
using hrvster.ApplicationServices.MatchGames;
using hrvster.ApplicationServices.Publishing;
using hrvster.ApplicationServices.Subscription;
using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Core.Requests.WebRequests.Files;
using hrvster.Games.Match.Models;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Core;
using hrvster.Infrastructure.MatchGames;
using hrvster.Infrastructure.Scheduling;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Profiles;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace hrvster.ApplicationServices
{
	public interface IGameService
	{
		Task CreateRulePostAsync(Guid ruleUid, string urlLink, string externalClientType, HrvsterUser hrvsterUser, string gameType, Guid gameUid, int gameId, int watchTypeId, string watchValue, string post, bool postNow, DateTime scheduledDate, IFormFile media1, IFormFile media2, IFormFile media3, IFormFile media4, string pageId, bool following);
		Task<bool> GameExistsForAccountAsync(Guid accountUid, Guid gameUid);
		Task<bool> GameExistsForSubdomainAsync(string subdomain, Guid gameUid);
		Task<GameDetail> GetGameDetailsAsync(GameTypesEnum gameType, Guid uid);
		Task<GameDetail> GetGameDetailsAsync(string gameUid);
		Task<IEnumerable<Rule>> GetRulesAsync(GameTypesEnum gameType, CancellationToken cancellationToken);
		Task<IEnumerable<Rule>> GetRulesAsync(GameTypesEnum gameType, Guid gameUid, CancellationToken cancellationToken);
		IEnumerable<Rule> GetRulesMatching(GameTypesEnum gameType, string watchValue);
		Task<IEnumerable<Rule>> GetRulesMatchingAsync(GameTypesEnum gameType, string text, CancellationToken cancellationToken);
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
		private readonly IAccountCompanyService _accountCompanyService;
		private readonly IScheduledPostRepository _scheduledPostRepository;
		private readonly ISweepRulesRepository _sweepRulesRepository;
		private readonly IMatchGameRulesRepository _matchGameRulesRepository;
		private readonly IWatchTypesRepository _watchTypesRepository;

		public GameService(
			ILoggerFactory loggerFactory,
			ISweepstakesService sweepstakesService,
			IMatchGamesService matchGamesService,
			IMediator mediator,
			IPublishService publishService,
			IAccountsService accountsService,
			IAccountSubscriptionService accountSubscriptionService,
			IAccountCompanyService accountCompanyService,
			IScheduledPostRepository scheduledPostRepository,
			ISweepRulesRepository sweepRulesRepository,
			IMatchGameRulesRepository matchGameRulesRepository,
			IWatchTypesRepository watchTypesRepository)
		{
			_logger = loggerFactory.CreateLogger<GameService>();
			_sweepService = sweepstakesService;
			_matchGameService = matchGamesService;
			_mediator = mediator;
			_publishService = publishService;
			_accountsService = accountsService;
			_accountSubscriptionService = accountSubscriptionService;
			_accountCompanyService = accountCompanyService;
			_scheduledPostRepository = scheduledPostRepository;
			_sweepRulesRepository = sweepRulesRepository;
			_matchGameRulesRepository = matchGameRulesRepository;
			_watchTypesRepository = watchTypesRepository;
		}

		public async Task<IEnumerable<Rule>> GetRulesAsync(
			GameTypesEnum gameType,
			Guid gameUid,
			CancellationToken cancellationToken)
		{
			switch (gameType)
			{
				case GameTypesEnum.Sweeps:
					return await _sweepService.GetSweepRulesAsync(gameUid);
				case GameTypesEnum.MatchGames:
					return await _matchGameService.GetRulesAsync(gameUid, cancellationToken);
				default:
					return await _sweepService.GetSweepRulesAsync(gameUid);
			}
		}

		public async Task<IEnumerable<Rule>> GetRulesAsync(GameTypesEnum gameType, CancellationToken cancellationToken)
		{
			var rules = new List<Rule>();

			switch (gameType)
			{
				case GameTypesEnum.MatchGames:
					rules.AddRange(await GetMatchGameRulesAsync(cancellationToken));
					break;
			}

			return rules;
		}

		private async Task<IEnumerable<Rule>> GetMatchGameRulesAsync(CancellationToken cancellationToken)
			=> await _matchGameService.GetRulesAsync(cancellationToken);

		public async Task<GameDetail> GetGameDetailsAsync(GameTypesEnum gameType, Guid uid)
		{
			switch (gameType)
			{
				case GameTypesEnum.Sweeps:
					return await GetSweepGameDetailsAsync(uid);
				case GameTypesEnum.MatchGames:
					return await GetMatchGameDetailsAsync(uid);
				default:
					return await GetSweepGameDetailsAsync(uid);
			}
		}

		public async Task<GameDetail> GetGameDetailsAsync(string gameUid)
		{
			var gameDetails = await GetSweepGameDetailsAsync(Guid.Parse(gameUid));

			if (gameDetails != null) return gameDetails;

			gameDetails = await GetMatchGameDetailsAsync(Guid.Parse(gameUid));

			if (gameDetails != null) return gameDetails;

			return null;
		}

		public async Task<IEnumerable<Rule>> GetRulesMatchingAsync(
			GameTypesEnum gameType, string watchValue, CancellationToken cancellationToken)
		{
			switch (gameType)
			{
				case GameTypesEnum.Sweeps:
					return await GetRulesMatchingForSweepsAsync(watchValue);
				case GameTypesEnum.MatchGames:
					return await GetRulesMatchingForMatchGamesAsync(watchValue, cancellationToken);
				default:
					return await GetRulesMatchingForSweepsAsync(watchValue);
			}
		}

		private async Task<IEnumerable<Rule>> GetRulesMatchingForMatchGamesAsync(
			string watchValue, CancellationToken cancellationToken)
			=> await _matchGameService.GetRulesMatchingAsync(watchValue, cancellationToken);

		private async Task<IEnumerable<Rule>> GetRulesMatchingForSweepsAsync(string watchValue)
			=> await _sweepService.GetRulesMatchingAsync(watchValue);

		public IEnumerable<Rule> GetRulesMatching(GameTypesEnum gameType, string watchValue)
		{
			switch (gameType)
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
					StartDateUtc = matchGame.StartDateUtc,
					EndDate = matchGame.EndDate,
					EndDateUtc = matchGame.EndDateUtc,
					Id = matchGame.Id,
					Uid = uid,
					CampaignUid = matchGame.CampaignUid
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
					StartDateUtc = sweepstake.StartDateUtc,
					EndDate = sweepstake.EndDate,
					EndDateUtc = sweepstake.EndDateUtc,
					Id = sweepstake.SweepId,
					Uid = uid,
					CampaignUid = sweepstake.Campaign.CampaignUid
				};
			}

			_logger.LogWarning("Could not load the Sweepstake with Uid {sweepUid}", uid);

			return gameDetail;
		}

		public async Task CreateRulePostAsync(
			Guid ruleUid,
			string urlLink,
			string externalClientType,
			HrvsterUser hrvsterUser,
			string gameType,
			Guid gameUid,
			int gameId,
			int watchTypeId,
			string watchValue,
			string post,
			bool postNow,
			DateTime scheduledDate,
			IFormFile media1,
			IFormFile media2,
			IFormFile media3,
			IFormFile media4,
			string pageId,
			bool following)
		{
			//1. Publish the post
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
					await CreateSweepRulePostAsync(
						ruleUid,
						urlLink,
						externalClientType,
						gameUid,
						gameId,
						GameTypesEnum.Sweeps,
						media,
						mediaLocation,
						hrvsterUser,
						watchValue,
						watchTypeId,
						post,
						postNow,
						scheduledDate,
						following,
						pageId);
					break;
				case GameTypesEnum.MatchGames:
					await CreateMatchRulePostAsync(
						ruleUid,
						urlLink,
						externalClientType,
						gameUid,
						gameId,
						GameTypesEnum.MatchGames,
						media,
						mediaLocation,
						hrvsterUser,
						watchValue,
						watchTypeId,
						post,
						postNow,
						scheduledDate,
						following,
						pageId);
					break;
			}
		}

		private async Task<bool> CreateSweepRulePostAsync(
			Guid ruleUid,
			string urlLink,
			string externalClientType,
			Guid gameUid,
			int gameId,
			GameTypesEnum gameType,
			List<IFormFile> media,
			IEnumerable<string> mediaLocation,
			HrvsterUser hrvsterUser,
			string watchValue,
			int watchTypeId,
			string post,
			bool postNow,
			DateTime scheduledDate,
			bool following,
			string pageId)
		{
			var profile = await _accountsService.GetProfileAsync(hrvsterUser.UserUid, GetProfileType(externalClientType));

			if (profile == null)
			{
				_logger.LogError("Could not locate the profile for this account that matched with {profileType}",
					GetProfileType(externalClientType));

				return false;
			}

			var accountSubscription = await _accountSubscriptionService.GetAccountSubscriptionAsync(profile.Account.AccountUid);

			if (accountSubscription == null)
			{
				_logger.LogWarning("Account with uid {accountUid} is not subscribed.", profile.Account.AccountUid);

				return false;
			}

			var ruleCount = await GetGameRuleCountsForAsync(hrvsterUser.UserUid);

			if (ruleCount >= accountSubscription.Plan.GetHashtagCount())
			{
				_logger.LogWarning("Account with uid {accountUid} has a total of {activeRulesCount} active hashtags and their current max is {maxHashtags}",
						profile.Account.AccountUid,
						ruleCount,
						accountSubscription.Plan.HashtagsPerMonth);

				return false;
			}

			return await PostMessageAsync(ruleUid, urlLink, externalClientType, gameUid, gameId, gameType, media, mediaLocation, hrvsterUser, watchValue, watchTypeId, post, postNow, scheduledDate, following, profile, pageId);
		}

		private async Task<bool> CreateMatchRulePostAsync(
			Guid ruleUid,
			string urlLink,
			string externalClientType,
			Guid gameUid,
			int gameId,
			GameTypesEnum gameType,
			List<IFormFile> media,
			IEnumerable<string> mediaLocation,
			HrvsterUser hrvsterUser,
			string watchValue,
			int watchTypeId,
			string post,
			bool postNow,
			DateTime scheduledDate,
			bool following, string pageId)
		{
			var profile = await _accountsService.GetProfileAsync(hrvsterUser.UserUid, GetProfileType(externalClientType));

			var accountSubscription = await _accountSubscriptionService.GetAccountSubscriptionAsync(profile.Account.AccountUid);

			if (accountSubscription == null)
			{
				_logger.LogWarning("Account with uid {accountUid} is not subscribed.", profile.Account.AccountUid);

				return false;
			}

			var ruleCount = await GetGameRuleCountsForAsync(hrvsterUser.UserUid);

			if (ruleCount >= accountSubscription.Plan.GetHashtagCount())
			{
				_logger.LogWarning("Account with uid {accountUid} has a total of {activeRulesCount} active hashtags and their current max is {maxHashtags}",
						profile.Account.AccountUid,
						ruleCount,
						accountSubscription.Plan.HashtagsPerMonth);

				return false;
			}

			return await PostMessageAsync(ruleUid, urlLink, externalClientType, gameUid, gameId, gameType, media, mediaLocation, hrvsterUser, watchValue, watchTypeId, post, postNow, scheduledDate, following, profile, pageId);
		}

		private async Task<bool> PostMessageAsync(
			Guid ruleUid,
			string urlLink,
			string externalClientType,
			Guid gameUid,
			int gameId,
			GameTypesEnum gameType,
			List<IFormFile> media,
			IEnumerable<string> mediaLocation,
			HrvsterUser hrvsterUser,
			string watchValue,
			int watchTypeId,
			string post,
			bool postNow,
			DateTime scheduledDate,
			bool following,
			Profile profile,
			string pageId)
		{
			if (postNow)
				return await _publishService.PublishAsync(ruleUid, urlLink, profile, gameId, gameUid, gameType, media,
					mediaLocation == null ? null : mediaLocation.ToList(),
					hrvsterUser.Token, hrvsterUser.UserUid, watchValue, watchTypeId, post,
					Enum.Parse<ExternalClientType>(externalClientType), following, pageId);

			return await SaveScheduledPostToPublishAsync(
				ruleUid,
				urlLink,
				hrvsterUser.UserUid,
				gameId,
				gameUid,
				profile,
				watchTypeId,
				watchValue,
				post,
				scheduledDate,
				mediaLocation == null ? null : mediaLocation.ToList(),
				Enum.Parse<ExternalClientType>(externalClientType),
				following,
				pageId,
				gameType,
				hrvsterUser.WindowsTimeZone,
				hrvsterUser.IsDaylight);
		}

		private async Task<bool> SaveScheduledPostToPublishAsync(
			Guid ruleUid,
			string urlLink,
			Guid userUid,
			int gameId,
			Guid gameUid,
			Profile profile,
			int watchTypeId,
			string watchValue,
			string post,
			DateTime scheduledDate,
			List<string> mediaLocation,
			ExternalClientType externalClientType,
			bool following,
			string pageId,
			GameTypesEnum gameType,
			string timezoneId,
			bool isDaylight)
		{
			using (_logger.StartSaveScheduledPostToPublishAsyncScope(ruleUid, gameUid, profile, watchValue, gameType))
			{
				try
				{
					_logger.LogInformation("Beginning transaction to save the rule with a hashtag to schedule.");

					await _scheduledPostRepository.BeginTransactionAsync();

					_logger.LogInformation("Retrieving watch types.");
					var watchTypes = await _watchTypesRepository.GetWatchTypesAsync();
					var watchTypeCodeSelected = watchTypes.SingleOrDefault(c => c.WatchTypeId.Equals(watchTypeId)).WatchTypeCode;
					_logger.LogInformation("The watch type selected is {watchType}", watchTypeCodeSelected);

					_logger.LogInformation("Building new rule.");
					var rule = new Rule
					{
						ActiveInd = true,
						RuleUid = ruleUid,
						Sweep = new Sweep { SweepUid = gameUid, Uid = gameUid },
						WatchTypeCode = Enum.Parse<WatchTypeCode>(watchTypeCodeSelected),
						WatchValue = watchValue,
						MustBeFollowing = following,
						ExternalClientType = externalClientType,
						ScheduledInd = true,
						UrlLink = urlLink
					};

					_logger.LogInformation("Rule built to save {rule}", rule.ToJsonString());

					switch (gameType)
					{
						case GameTypesEnum.Sweeps:
							_logger.LogInformation("Creating a sweepstake rule.");
							await _sweepRulesRepository.CreateRuleAsync(rule).ContinueWith(_ =>
							{
								_logger.LogInformation("Finished the sweep rule creation.");
							});
							break;
						case GameTypesEnum.MatchGames:
							_logger.LogInformation("Creating a match game rule.");
							await _matchGameRulesRepository.CreateAsync(rule, default).ContinueWith(_ =>
							{
								_logger.LogInformation("Finished the match game rule creation.");
							});
							break;
					}

					_logger.LogInformation("Retrieving the rule we just saved.");
					rule = await GetRuleAsync(ruleUid, gameUid, gameType);
					if (rule == null)
					{
						_logger.LogError("Unable to retrieve rule.");
						await _scheduledPostRepository.RollbackTransactionAsync();

						return false;
					}

					var scheduledPostUid = Guid.NewGuid();

					_logger.LogInformation("Creating the scheduled post for the rule {rule}.", rule.ToJsonString());
					await _scheduledPostRepository.CreateScheduledPostAsync(
						scheduledPostUid,
						profile.ProfileUid,
						rule.RuleId,
						gameType.ToString(),
						post,
						scheduledDate,
						urlLink,
						ScheduledStatus.New,
						timezoneId,
						isDaylight).ContinueWith(_ =>
						{
							_logger.LogInformation("Scheduled post creation task completed.");
						});

					foreach (var imageLocation in mediaLocation)
					{
						var scheduledPostImageUid = Guid.NewGuid();
						_logger.LogInformation("Saving the scheduled post image {imageLocation}", imageLocation);
						await _scheduledPostRepository.CreateScheduledPostImageAsync(
							scheduledPostUid,
							scheduledPostImageUid,
							imageLocation).ContinueWith(_ =>
							{
								_logger.LogInformation("Scheduled post image creation task completed.");
							});
					}

					await _scheduledPostRepository.CommitTransactionAsync().ContinueWith(_ =>
					{
						_logger.LogInformation("Transaction committed!");
					});

					return true;
				}
				catch (Exception ex)
				{
					_logger.LogError("An error occurred trying to save the Scheduled Tweet. Rolling back the transaction. Exception Message: {message}",
						ex.Message);

					await _scheduledPostRepository.RollbackTransactionAsync();

					return false;
				}
			}
		}

		private async Task<Rule> GetRuleAsync(Guid ruleUid, Guid gameUid, GameTypesEnum gameType)
		{
			var rules = await GetRulesAsync(gameType, gameUid, default);

			return rules.FirstOrDefault(r => r.RuleUid == ruleUid);
		}

		private static Core.Clients.Common.Message BuildMessage(
			Guid gameUid,
			Profile profile,
			IList<string> mediaLocation,
			string watchValue,
			int watchTypeId,
			string tweetText)
		{
			return new Core.Clients.Common.Message
			{
				GameUid = gameUid,
				Hashtag = watchValue,
				IsScheduled = false,
				Profile = profile,
				Media = mediaLocation,
				Text = tweetText,
				WatchTypeId = watchTypeId
			};
		}

		private ProfileType GetProfileType(string externalClientType)
		{
			return Enum.Parse<ExternalClientType>(externalClientType) switch
			{
				ExternalClientType.Twitter => ProfileType.Twitter,
				ExternalClientType.Facebook => ProfileType.Facebook,
				ExternalClientType.Instagram => ProfileType.Facebook,
				_ => ProfileType.Facebook,
			};
		}

		private async Task<int> GetGameRuleCountsForAsync(Guid userUid)
		{
			var rules = await _accountsService.GetAllRulesAsync(userUid);

			if (rules == null)
			{
				return 0;
			}

			return rules.GetActiveRules().Count();
		}

		public async Task<bool> GameExistsForSubdomainAsync(string subdomain, Guid gameUid)
		{
			var accountCompany = await _accountCompanyService.GetAccountCompanyAsync(subdomain);

			if (accountCompany == null) return false;

			//does the game exist?
			var sweepGame = await _sweepService.GetSweepstakeAsync(gameUid, default);
			if (sweepGame != null) return true;

			var matchGame = await _matchGameService.GetMatchGameAsync(gameUid, default);
			if (matchGame != null) return true;

			return false;
		}

		public async Task<bool> GameExistsForAccountAsync(Guid accountUid, Guid gameUid)
		{
			using (_logger.GameServiceGameExistsForAccount(gameUid, accountUid))
			{
				var sweepGame = await _sweepService.GetSweepstakeAsync(gameUid, default);
				var matchGame = await _matchGameService.GetMatchGameAsync(gameUid, default);

				if (!IsAnyGameFound(sweepGame, matchGame))
				{
					_logger.LogInformation("Game not found.");
					return false;
				}

				var account = await _accountsService.GetAccountByUidAsync(accountUid);
				if (account == null)
				{
					_logger.LogInformation("Account not found.");
					return false;
				}

				if (IsSweepGameAccessibleByAccount(sweepGame, accountUid))
				{
					return true;
				}

				if (await IsMatchGameAccessibleByAccount(matchGame, accountUid))
				{
					return true;
				}

				_logger.LogInformation("Account does not have access to the game.");

				return false;
			}
		}

		private bool IsAnyGameFound(Sweep sweepGame, MatchGame matchGame)
		{
			return !(sweepGame == null && matchGame == null);
		}

		private bool IsSweepGameAccessibleByAccount(Sweep sweepGame, Guid accountUid)
		{
			return sweepGame != null && sweepGame.Account.AccountUid == accountUid;
		}

		private async Task<bool> IsMatchGameAccessibleByAccount(MatchGame matchGame, Guid accountUid)
		{
			if (matchGame == null)
			{
				return false;
			}

			var accountMatchGames = await _matchGameService.GetAccountMatchGamesAsync(accountUid, default);
			return accountMatchGames.Any(c => c.Uid == matchGame.Uid);
		}
	}
}
