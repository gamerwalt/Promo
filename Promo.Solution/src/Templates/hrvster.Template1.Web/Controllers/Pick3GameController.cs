using AutoMapper;
using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.Communications;
using hrvster.ApplicationServices.Extensions.LogExtensions;
using hrvster.ApplicationServices.MatchGames;
using hrvster.ApplicationServices.Reporting;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Harvester;
using hrvster.Infrastructure.MatchGames;
using hrvster.Infrastructure.Reporting;
using hrvster.Models;
using hrvster.Models.Options;
using hrvster.Template1.Web.Models;
using hrvster.Template1.Web.Models.Query;
using hrvster.Templates.Core.Common;
using hrvster.Templates.Infrastructure.Customizations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Web;

namespace hrvster.Template1.Web.Controllers
{
	public class Pick3GameController : Controller
	{
		private readonly ILogger<Pick3GameController> _logger;
		private readonly IGameService _gameService;
		private readonly IBlacklistedUserRepository _blackListedUserRepository;
		private readonly ISessionManager _sessionManager;
		private readonly IMatchGamePlayReportingRepository _matchGamePlayReportingRepository;
		private readonly IMatchPlayRulesRepository _matchPlayRulesRepository;
		private readonly ICampaignService _campaignService;
		private readonly IMatchGameplayService _matchGameplayService;
		private readonly IMatchGamesService _matchGamesService;
		private readonly IMatchGameBoardService _matchGameBoardService;
		private readonly IMapper _mapper;
		private readonly IReportingService _reportingService;
		private readonly IMemberRepository _memberRepository;
		private readonly IMatchGameRulesRepository _matchGameRulesRepository;
		private readonly IAccountsService _accountsService;
		private readonly IProfilesRepository _profilesRepository;
		private readonly ITwitterHttpService _twitterHttpService;
		private readonly ICommunicationService _communicationService;
		private readonly HarvesterSettingsOptions _harvesterSettingOptions;
		private readonly IClientMemberService _clientMemberService;

		public Pick3GameController(
			ILoggerFactory loggerFactory,
			IGameService gameService,
			IBlacklistedUserRepository blacklistedUserRepository,
			ISessionManager sessionManager,
			IMatchGamePlayReportingRepository matchGamePlayReportingRepository,
			IMatchPlayRulesRepository matchPlayRulesRepository,
			ICampaignService campaignService,
			IMatchGameplayService matchGameplayService,
			IMatchGamesService matchGamesService,
			IMatchGameBoardService matchGameBoardService,
			IMapper mapper,
			IReportingService reportingService,
			IMemberRepository memberRepository,
			IMatchGameRulesRepository matchGameRulesRepository,
			IAccountsService accountsService,
			IProfilesRepository profilesRepository,
			ITwitterHttpService twitterHttpService,
			ICommunicationService communicationService,
			HarvesterSettingsOptions harvesterSettingOptions,
			IClientMemberService clientMemberService)
		{
			_logger = loggerFactory.CreateLogger<Pick3GameController>();
			_gameService = gameService;
			_blackListedUserRepository = blacklistedUserRepository;
			_sessionManager = sessionManager;
			_matchGamePlayReportingRepository = matchGamePlayReportingRepository;
			_matchPlayRulesRepository = matchPlayRulesRepository;
			_campaignService = campaignService;
			_matchGameplayService = matchGameplayService;
			_matchGamesService = matchGamesService;
			_matchGameBoardService = matchGameBoardService;
			_mapper = mapper;
			_reportingService = reportingService;
			_memberRepository = memberRepository;
			_matchGameRulesRepository = matchGameRulesRepository;
			_accountsService = accountsService;
			_profilesRepository = profilesRepository;
			_twitterHttpService = twitterHttpService;
			_communicationService = communicationService;
			_harvesterSettingOptions = harvesterSettingOptions;
			_clientMemberService = clientMemberService;
		}


		[HttpGet]
		[Route("Pick3/PrePlay")]
		public async Task<IActionResult> PrePlay(
			[FromQuery] Guid gameUid,
			[FromQuery] Guid ruleUid,
			[FromQuery] string externalClient,
			[FromQuery] Guid campaignUid,
			[FromQuery] string handshakeKey,
			CancellationToken cancellationToken)
		{
			_logger.LogInformation("Received a request to play the pick 3 game with uid {GameUid}", gameUid);

			using (_logger.BeginPrePlayForPick3GameScope(gameUid, ruleUid, externalClient))
			{
				_logger.LogInformation("PrePlay for Pick3 game");

				var game = await _gameService.GetGameDetailsAsync(GameTypesEnum.MatchGames, gameUid);
				if (game == null)
				{
					_logger.LogError("Game not found!");

					return ErrorRedirect("Game not found!", StatusCodes.Status400BadRequest);
				}

				if(DateTime.UtcNow > game.EndDateUtc)
				{
					_logger.LogError("Game is past end date.");

					return ErrorRedirect("Game is past end date", StatusCodes.Status400BadRequest);
				}

				if(DateTime.UtcNow < game.StartDateUtc)
				{
					_logger.LogError("Game has not started.");

					return ErrorRedirect("Game has not started.", StatusCodes.Status400BadRequest);
				}

				if(!game.GameBoardsHasBeenGenerated())
				{
					_logger.LogError("Game boards have not been generated.");

					return ErrorRedirect("Game boards have not been generated.", StatusCodes.Status400BadRequest);
				}

				var campaign = await _campaignService.GetCampaignAsync(campaignUid, cancellationToken);
				if (campaign == null)
				{
					_logger.LogError("Campaign {CampaignUid} not found!", campaignUid);

					return ErrorRedirect("Campaign not found!", StatusCodes.Status400BadRequest);
				}

				if (!User.Identity.IsAuthenticated)
				{
					if (externalClient == ExternalClientType.Twitter.ToString())
					{
						_logger.LogWarning("User is not authenticated.");

						var code = GetProviderCode(externalClient);
						_logger.LogInformation("Redirecting with provider code {code}", code);

						return RedirectToAction(
							"ExternalLogin",
							"Authentication",
							new
							{
								providerCode = code,
								gameUid,
								ruleUid,
								externalClient,
								gameType = GameTypesEnum.Sweeps.ToString(),
								campaign.CampaignUid,
								campaign.Account.AccountUid
							});
					}
					else
					{
						_logger.LogInformation("User was not authenticated, authenticating now with handshake.");

						var handshakeKeyDecoded = HttpUtility.UrlDecode(handshakeKey);

						var decryptedData = CryptoHelper.DecryptHandshakeKey(
						handshakeKeyDecoded,
							_harvesterSettingOptions.EncryptionKey,
							_harvesterSettingOptions.EncryptionIV);

						var userQuery = UserQuery.Parse(decryptedData);

						var userDetailsForClient = new UserProviderDetails
						{
							AccessToken = Guid.NewGuid().ToString(),
							AccessTokenSecret = Guid.NewGuid().ToString(),
							ExternalProfileId = userQuery.UserId,
							Name = userQuery.UserName,
							ScreenName = userQuery.UserName,
							Description = $"{userQuery.UserName}-{userQuery.UserId}",
							RefreshToken = Guid.NewGuid().ToString()
						};

						_logger.LogInformation("Signing in the user automatically.");

						await SignInUserAsync(userDetailsForClient, ExternalClientType.Client);
					}
				}

				var userClaims = User.Identities.FirstOrDefault()?.Claims;
				var externalProfileId = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?.Value;
				var screenName = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.DisplayName)?.Value;
				screenName = $"@{screenName}";

				if (string.IsNullOrWhiteSpace(externalProfileId))
				{
					_logger.LogError("ExternalProfileId is empty!");

					return ErrorRedirect("ExternalProfileId is empty!", StatusCodes.Status400BadRequest);
				}

				if (string.IsNullOrWhiteSpace(screenName))
				{
					_logger.LogError("ScreenName is empty!");

					return ErrorRedirect("ScreenName is empty!", StatusCodes.Status400BadRequest);
				}

				var canPlayGame = await CanPlayMatchGame(gameUid, campaign.CampaignUid, externalProfileId, screenName, cancellationToken);

				if (!canPlayGame)
				{
					_logger.LogError("User cannot play game!");

					return ErrorRedirect("User cannot play game!", StatusCodes.Status400BadRequest);
				}

				var blacklisted = await _blackListedUserRepository.GetBlacklistedUsersAsync(gameUid);
				if (blacklisted.Any(c => c.UserId.Equals(externalProfileId)))
				{
					_logger.LogError("User is blacklisted!");

					return ErrorRedirect("User is blacklisted!", StatusCodes.Status400BadRequest);
				}

				var rules = await _gameService.GetRulesAsync(GameTypesEnum.MatchGames, gameUid, cancellationToken);
				if (rules == null)
				{
					_logger.LogError("No rules have been configured for the game with uid {gameUid}", gameUid);

					return ErrorRedirect("No rules have been configured for the game!", StatusCodes.Status400BadRequest);
				}

				var rule = rules.FirstOrDefault(r => r.RuleUid == ruleUid);
				if (rule == null)
				{
					_logger.LogError("No rule found for the game with uid {gameUid} and ruleUid {ruleUid}", gameUid, ruleUid);

					return ErrorRedirect("No rule found for the game!", StatusCodes.Status400BadRequest);
				}

				if (rule.MustBeFollowing)
				{
					/*
					var followers = await _twitterHttpService.GetFollowersAsync(externalProfileId);

								if (!followers.Any(c => c.Id == profile.ExternalProfileId))*/

					//return RedirectToAction("CannotPlayMatchGame", new { gameUid, ruleUid, externalClient });
				}

				_logger.LogInformation("PrePlay for Pick3 game completed successfully");
				_logger.LogInformation("{Screename} with External Profile Id {externalProfileId} can now play the game.",
					screenName,
					externalProfileId);

				_logger.LogInformation("Reserving a gameboard for {Screename} with External Profile Id {externalProfileId}",
					screenName,
					externalProfileId);

				var gameplayUid = await _matchGameplayService.ReserveGameboardAsync(
					gameUid,
					screenName,
					GetUserIdentifierType(externalClient),
					default);

				if(gameplayUid == Guid.Empty)
				{
					_logger.LogError("THere are no more gameboards available for the game with uid {GameUid}", gameUid);

					return ErrorRedirect("There are no more game boards available!", StatusCodes.Status400BadRequest);
				}

				if (gameplayUid == Guid.Empty)
				{
					_logger.LogError("No gameboard found for {Screename} with External Profile Id {externalProfileId}",
						screenName,
						externalProfileId);

					return ErrorRedirect("No gameboard found for the user!", StatusCodes.Status400BadRequest);
				}

				if (!await _matchGameBoardService.IsBoardPlayableByGameplayAsync(gameplayUid, default))
				{
					_logger.LogError("Gameboard with Gameplay Uid {gameplayUid} is not playable for {Screename} with External Profile Id {externalProfileId}",
						gameplayUid,
						screenName,
						externalProfileId);

					return ErrorRedirect("Gameboard is not playable for the user!", StatusCodes.Status400BadRequest);
				}

				return RedirectToAction("Play", new { gameUid, ruleUid, gameplayUid, externalClient, campaignUid, handshakeKey });
			}
		}


		[HttpGet]
		[Route("Pick3/Play")]
		public async Task<IActionResult> Play(
			[FromQuery] Guid gameUid, 
			[FromQuery] Guid ruleUid,
			[FromQuery] string externalClient, 
			[FromQuery] Guid gameplayUid,
			[FromQuery] Guid campaignUid,
			[FromQuery] string handshakeKey,
			CancellationToken cancellationToken)
		{

			if (!User.Identity.IsAuthenticated)
			{
				if (externalClient == ExternalClientType.Twitter.ToString())
				{
					_logger.LogWarning("User is not authenticated.");
					var campaign = await _campaignService.GetCampaignAsync(campaignUid, cancellationToken);

					var code = GetProviderCode(externalClient);
					_logger.LogInformation("Redirecting with provider code {code}", code);

					return RedirectToAction(
						"ExternalLogin",
						"Authentication",
						new
						{
							providerCode = code,
							gameUid,
							ruleUid,
							externalClient,
							gameType = GameTypesEnum.Sweeps.ToString(),
							campaignUid,
							campaign.Account.AccountUid
						});
				}
				else
				{
					_logger.LogInformation("User was not authenticated, authenticating now with handshake.");

					var handshakeKeyDecoded = HttpUtility.UrlDecode(handshakeKey);

					var decryptedData = CryptoHelper.DecryptHandshakeKey(
					handshakeKeyDecoded,
						_harvesterSettingOptions.EncryptionKey,
						_harvesterSettingOptions.EncryptionIV);

					var userQuery = UserQuery.Parse(decryptedData);

					var userDetailsForClient = new UserProviderDetails
					{
						AccessToken = Guid.NewGuid().ToString(),
						AccessTokenSecret = Guid.NewGuid().ToString(),
						ExternalProfileId = userQuery.UserId,
						Name = userQuery.UserName,
						ScreenName = userQuery.UserName,
						Description = $"{userQuery.UserName}-{userQuery.UserId}",
						RefreshToken = Guid.NewGuid().ToString()
					};

					_logger.LogInformation("Signing in the user automatically.");

					await SignInUserAsync(userDetailsForClient, ExternalClientType.Client);
				}
			}

			if (User.Identity.IsAuthenticated)
			{
				using (_logger.BeginGetPlayForPick3GameScope(gameUid, ruleUid, externalClient, gameplayUid))
				{
					var game = await _gameService.GetGameDetailsAsync(GameTypesEnum.MatchGames, gameUid);
					if (game == null)
					{
						_logger.LogError("Game not found!");

						return ErrorRedirect("Game not found!", StatusCodes.Status400BadRequest);
					}

					if (DateTime.UtcNow > game.EndDateUtc)
					{
						_logger.LogError("Game is past end date.");

						return ErrorRedirect("Game is past end date", StatusCodes.Status400BadRequest);
					}

					if (DateTime.UtcNow < game.StartDateUtc)
					{
						_logger.LogError("Game has not started.");

						return ErrorRedirect("Game has not started.", StatusCodes.Status400BadRequest);
					}

					if (!game.GameBoardsHasBeenGenerated())
					{
						_logger.LogError("Game boards have not been generated.");

						return ErrorRedirect("Game boards have not been generated.", StatusCodes.Status400BadRequest);
					}

					var campaign = await _campaignService.GetCampaignFromGameUidAsync(gameUid, GameTypesEnum.MatchGames.ToString());

					_sessionManager.SetKeyValue(SessionConstants.AccountUid, campaign.Account.AccountUid.ToString());

					if (gameplayUid == Guid.Empty)
					{
						_logger.LogError("No gameboard found for {gameplayUid}", gameplayUid);

						return ErrorRedirect("No gameboard found for the user!", StatusCodes.Status400BadRequest);
					}

					if (!await _matchGameBoardService.IsBoardPlayableByGameplayAsync(gameplayUid, cancellationToken))
					{
						_logger.LogError("Gameboard with Gameplay Uid {gameplayUid} is not playable", gameplayUid);

						return ErrorRedirect("Gameboard is not playable for the user!", StatusCodes.Status400BadRequest);
					}

					var gameplay = await _matchGameplayService.GetAsync(gameplayUid, cancellationToken);
					if (gameplay == null)
					{
						_logger.LogError("Gameplay not found!");

						return ErrorRedirect("Gameplay not found!", StatusCodes.Status400BadRequest);
					}

					var gameboard = await _matchGameBoardService.GetGameBoardAsync(gameplay.MatchBoard.Uid, cancellationToken);
					if (gameboard == null)
					{
						_logger.LogError("Gameboard not found!");

						return ErrorRedirect("Gameboard not found!", StatusCodes.Status400BadRequest);
					}

					var tiles = await _matchGameBoardService.GetTilesAsync(gameboard.Uid, cancellationToken);
					if (tiles == null)
					{
						_logger.LogError("Tiles not found!");

						return ErrorRedirect("Tiles not found!", StatusCodes.Status400BadRequest);
					}

					var model = new PlayMatchGame
					{
						GameplayUid = gameplay.Uid,
						GameUid = gameUid,
						GameName = gameboard.MatchGame.Name,
						MatchBoard = _mapper.Map<MatchBoard>(gameboard),
						Tiles = LoadTiles(tiles),
						ExternalClient = externalClient,
						HandshakeKey = handshakeKey ?? Guid.NewGuid().ToString(),
						CampaignUid = campaignUid,
						RuleUid = ruleUid
					};

					return View(model);
				}
			}

			_logger.LogWarning("User is not authenticated.");

			return ErrorRedirect("User is not authenticated.", StatusCodes.Status401Unauthorized);
		}

		[HttpPost]
		[Route("Pick3/Play")]
		public async Task<IActionResult> Play(
			[FromQuery] Guid gameUid,
			[FromQuery] Guid ruleUid,
			[FromQuery] string externalClient,
			[FromQuery] Guid gameplayUid,
			[FromQuery] Guid campaignUid,
			[FromQuery] string handshakeKey,
			PlayMatchGame form, 
			CancellationToken cancellationToken)
		{
			if (!User.Identity.IsAuthenticated)
			{
				if (externalClient == ExternalClientType.Twitter.ToString())
				{
					_logger.LogWarning("User is not authenticated.");
					var campaign = await _campaignService.GetCampaignAsync(campaignUid, cancellationToken);

					var code = GetProviderCode(externalClient);
					_logger.LogInformation("Redirecting with provider code {code}", code);

					return RedirectToAction(
						"ExternalLogin",
						"Authentication",
						new
						{
							providerCode = code,
							gameUid,
							ruleUid,
							externalClient,
							gameType = GameTypesEnum.Sweeps.ToString(),
							campaignUid,
							campaign.Account.AccountUid
						});
				}
				else
				{
					_logger.LogInformation("User was not authenticated, authenticating now with handshake.");
					var handshakeKeyDecoded = HttpUtility.UrlDecode(handshakeKey);

					var decryptedData = CryptoHelper.DecryptHandshakeKey(
					handshakeKeyDecoded,
						_harvesterSettingOptions.EncryptionKey,
						_harvesterSettingOptions.EncryptionIV);

					var userQuery = UserQuery.Parse(decryptedData);

					var userDetailsForClient = new UserProviderDetails
					{
						AccessToken = Guid.NewGuid().ToString(),
						AccessTokenSecret = Guid.NewGuid().ToString(),
						ExternalProfileId = userQuery.UserId,
						Name = userQuery.UserName,
						ScreenName = userQuery.UserName,
						Description = $"{userQuery.UserName}-{userQuery.UserId}",
						RefreshToken = Guid.NewGuid().ToString()
					};

					_logger.LogInformation("Signing in the user automatically.");

					await SignInUserAsync(userDetailsForClient, ExternalClientType.Client);
				}
			}

			if (User.Identity.IsAuthenticated)
			{
				using (_logger.BeginPostPlayForPick3GameScope(gameUid, ruleUid, externalClient, gameplayUid))
				{
					var game = await _gameService.GetGameDetailsAsync(GameTypesEnum.MatchGames, gameUid);
					if (game == null)
					{
						_logger.LogWarning("Game not found!");

						return ErrorRedirect("Game not found!", StatusCodes.Status400BadRequest);
					}

					if (DateTime.UtcNow > game.EndDateUtc)
					{
						_logger.LogError("Game is past end date.");

						return ErrorRedirect("Game is past end date", StatusCodes.Status400BadRequest);
					}

					if (DateTime.UtcNow < game.StartDateUtc)
					{
						_logger.LogError("Game has not started.");

						return ErrorRedirect("Game has not started.", StatusCodes.Status400BadRequest);
					}

					if (!game.GameBoardsHasBeenGenerated())
					{
						_logger.LogError("Game boards have not been generated.");

						return ErrorRedirect("Game boards have not been generated.", StatusCodes.Status400BadRequest);
					}

					var campaign = await _campaignService.GetCampaignFromGameUidAsync(gameUid, GameTypesEnum.MatchGames.ToString());

					_sessionManager.SetKeyValue(SessionConstants.AccountUid, campaign.Account.AccountUid.ToString());

					if (gameplayUid == Guid.Empty)
					{
						_logger.LogWarning("No gameboard found for {gameplayUid}", gameplayUid);

						return ErrorRedirect("No gameboard found for the user!", StatusCodes.Status400BadRequest);
					}

					if (!await _matchGameBoardService.IsBoardPlayableByGameplayAsync(gameplayUid, default))
					{
						_logger.LogWarning("Gameboard with Gameplay Uid {gameplayUid} is not playable", gameplayUid);

						return ErrorRedirect("Gameboard is not playable for the user!", StatusCodes.Status400BadRequest);
					}

					if (ModelState.IsValid)
					{
						if(externalClient == ExternalClientType.Twitter.ToString())
						{
							var userClaims = User.Identities.FirstOrDefault()?.Claims;
							var externalProfileId = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?.Value;
							var screenName = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.DisplayName)?.Value;

							await _reportingService.SaveMatchGameAsync(
								gameUid,
								gameplayUid,
								form.BoardUid.GetValueOrDefault(),
								GetUserIdentifierType(externalClient),
								screenName,
							externalProfileId,
							cancellationToken);
						}

						//consider passing the accountUid, decryptedValues of the user here
						return await ReturnWinOrLossAsync(gameUid, gameplayUid, ruleUid, form, externalClient, campaign.Account.AccountUid, handshakeKey, cancellationToken);
					}
				}
			}

			_logger.LogWarning("User is not authenticated.");

			return ErrorRedirect("User is not authenticated.", StatusCodes.Status401Unauthorized);
		}

		[HttpGet]
		[Route("/Pick3/win/{Uid:Guid}/gameplay/{gameplayUid:Guid}")]
		public async Task<IActionResult> MatchGameWin(
			Guid Uid,
			Guid gameplayUid,
			CancellationToken cancellationToken)
		{
			var gameplay = await _matchGameplayService.GetAsync(gameplayUid, cancellationToken);

			if (!gameplay.PlayedIndicator && !gameplay.MatchBoard.PlayedIndicator && !gameplay.WinIndicator)
			{
				_logger.LogWarning("Gameplay {gameplayUid} is not a win", gameplayUid);

				return ErrorRedirect("Gameplay is not a win!", StatusCodes.Status400BadRequest);
			}

			var gameboard = await _matchGameBoardService.GetGameBoardAsync(gameplay.MatchBoard.Uid, cancellationToken);
			var tiles = await _matchGameBoardService.GetTilesAsync(gameboard.Uid, cancellationToken);

			var model = new GameplayWin
			{
				GameplayUid = gameplay.Uid,
				GameUid = gameboard.MatchGame.Uid,
				GameName = gameboard.MatchGame.Name,
				MatchBoard = _mapper.Map<Models.MatchBoard>(gameboard),
				Tiles = GetMatchTiles(tiles),
				SelectedTiles = LoadTileStrings(gameplay.SelectedTiles)
			};

			return View(model);
		}

		[HttpGet]
		[Route("/Pick3/loss/{Uid:Guid}/gameplay/{gameplayUid:Guid}/handshakeKey/{handshakeKey}/account/{accountUid}")]
		public async Task<IActionResult> MatchGameLoss(
			Guid Uid,
			Guid gameplayUid,
			string handshakeKey,
			Guid accountUid,
			CancellationToken cancellationToken)
		{
			var userQuery = GetUserQuery(handshakeKey);

			var points = await _clientMemberService.GetClientMemberPointAsync(userQuery.UserId, userQuery.UserName, accountUid);

			var gameplay = await _matchGameplayService.GetAsync(gameplayUid, cancellationToken);

			if (!gameplay.PlayedIndicator && !gameplay.MatchBoard.PlayedIndicator && gameplay.WinIndicator)
			{
				_logger.LogWarning("Gameplay {gameplayUid} is not a loss", gameplayUid);

				return ErrorRedirect($"Gameplay {gameplayUid} is not a loss", StatusCodes.Status400BadRequest);
			}

			var gameboard = await _matchGameBoardService.GetGameBoardAsync(gameplay.MatchBoard.Uid, cancellationToken);
			var tiles = await _matchGameBoardService.GetTilesAsync(gameboard.Uid, cancellationToken);

			var model = new GameplayLoss
			{
				GameplayUid = gameplay.Uid,
				GameUid = gameboard.MatchGame.Uid,
				GameName = gameboard.MatchGame.Name,
				MatchBoard = _mapper.Map<Models.MatchBoard>(gameboard),
				Tiles = GetMatchTiles(tiles),
				SelectedTiles = LoadTileStrings(gameplay.SelectedTiles),
				Points = points
			};

			return View(model);
		}

		private List<Models.MatchTile> GetMatchTiles(IEnumerable<Games.Match.Models.MatchTile> tiles)
		{
			var matchTiles = new List<Models.MatchTile>();

			foreach (var tile in tiles)
			{
				matchTiles.Add(new Models.MatchTile
				{
					Uid = tile.Uid,
					ImageUrl = tile.Image.LocationUrl
				});
			}

			return matchTiles;
		}

		private List<Guid> LoadTileStrings(string tileUids)
		{
			var tiles = new List<Guid>();

			var tileList = tileUids.Split(',').ToList();

			foreach (var tileUid in tileList)
			{
				tiles.Add(Guid.Parse(tileUid));
			}

			return tiles;
		}

		private async Task<IActionResult> ReturnWinOrLossAsync(
			Guid gameUid,
			Guid gameplayUid,
			Guid ruleUid,
			PlayMatchGame form,
			string externalClient,
			Guid accountUid,
			string handshakeKey,
			CancellationToken cancellationToken)
		{
			//decrypt and get the data from the handshakeKey
			var accountUidString = _sessionManager.GetValue(SessionConstants.AccountUid);
			if (string.IsNullOrEmpty(accountUidString))
			{
				_logger.LogWarning("AccountUid not found in session!");

				return ErrorRedirect("AccountUid not found in session!", StatusCodes.Status400BadRequest);
			}

			var rule = await _matchGameRulesRepository.GetAsync(ruleUid);
			if (rule == null)
			{
				_logger.LogWarning("Rule not found!");

				return ErrorRedirect("Rule not found!", StatusCodes.Status400BadRequest);
			}

			var game = await _matchGamesService.GetMatchGameAsync(gameUid, cancellationToken);
			if (game == null)
			{
				_logger.LogWarning("Game not found!");

				return ErrorRedirect("Game not found!", StatusCodes.Status400BadRequest);
			}

			var campaign = await _campaignService.GetCampaignAsync(game.CampaignUid, cancellationToken);
			if (campaign == null)
			{
				_logger.LogWarning("Campaign not found!");

				return ErrorRedirect("Campaign not found!", StatusCodes.Status400BadRequest);
			}

			var account = await _accountsService.GetAccountByUidAsync(campaign.Account.AccountUid);
			if (account == null)
			{
				_logger.LogWarning("Account not found!");

				return ErrorRedirect("Account not found!", StatusCodes.Status400BadRequest);
			}

			var profiles = await _profilesRepository.GetProfileAsync(account.AccountUid, GetActualProfileType(externalClient));
			if (profiles == null)
			{
				_logger.LogWarning("Profile not found!");

				return ErrorRedirect("Profile not found!", StatusCodes.Status400BadRequest);
			}

			var hashtag = rule.WatchValue;
			var replyTo = $"@{profiles.ScreenName}";
			var message = string.Empty;
			
			if (await _matchGameplayService.IsAWinAsync(gameUid: gameUid, gameplayUid, form.SelectedTiles, cancellationToken))
			{
				await SetGameplayAsWonAsync(gameplayUid, form, cancellationToken);
				var prize = await _matchGamesService.GetPrizeFromGameplayUidAsync(gameplayUid, cancellationToken);

				message = $"{replyTo} I just played the Pick 3 game {game.Name} and won. I just won a prize with a value of ${prize.Value}. {Environment.NewLine} {hashtag}";

				var screenName = string.Empty;

				if(externalClient == ExternalClientType.Twitter.ToString())
				{
					var userClaims = User.Identities.FirstOrDefault()?.Claims;
					var externalProfileId = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?.Value;
					screenName = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.DisplayName)?.Value;

					var member = await _memberRepository.GetMemberViaExternalProfileIdAsync(accountUid, GetProfileType(externalClient), externalProfileId);

					if (string.IsNullOrWhiteSpace(externalProfileId))
					{
						_logger.LogWarning("ExternalProfileId is empty!");

						return ErrorRedirect("ExternalProfileId is empty!", StatusCodes.Status400BadRequest);
					}

					if (string.IsNullOrWhiteSpace(screenName))
					{
						_logger.LogWarning("ScreenName is empty!");

						return ErrorRedirect("ScreenName is empty!", StatusCodes.Status400BadRequest);
					}

					var accessToken = member.Profiles.FirstOrDefault().AccessToken;
					var refreshToken = member.Profiles.FirstOrDefault().RefreshToken;

					await _twitterHttpService.PublishTweetInResponseToAsync(message, rule.ExternalId, accessToken, refreshToken);
					await _communicationService.NotifyMemberOfWin(member.EmailAddress, game.Name, gameplayUid, prize.Name, prize.Value);
				}
				else
				{
					var userQuery = GetUserQuery(handshakeKey);
					screenName = userQuery.UserName;
				}

				//Send email to owner
				await _communicationService.NotifyAccountOfWin(account.EmailAddress, game.Uid, game.Name, screenName, prize.Name);

				return RedirectToAction("MatchGameWin", new { Uid = gameUid, gameplayUid = gameplayUid });
			}

			await SetGameplayAsLossAsync(gameplayUid, form, cancellationToken);

			message = $"{replyTo} I just played the Pick 3 game {game.Name} and lost unfortunately ;). {Environment.NewLine} {hashtag}";

			if(externalClient == ExternalClientType.Twitter.ToString())
			{
				var userClaims = User.Identities.FirstOrDefault()?.Claims;
				var externalProfileId = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?.Value;
				var screenName = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.DisplayName)?.Value;

				var member = await _memberRepository.GetMemberViaExternalProfileIdAsync(accountUid, GetProfileType(externalClient), externalProfileId);

				if (string.IsNullOrWhiteSpace(externalProfileId))
				{
					_logger.LogWarning("ExternalProfileId is empty!");

					return ErrorRedirect("ExternalProfileId is empty!", StatusCodes.Status400BadRequest);
				}

				if (string.IsNullOrWhiteSpace(screenName))
				{
					_logger.LogWarning("ScreenName is empty!");

					return ErrorRedirect("ScreenName is empty!", StatusCodes.Status400BadRequest);
				}

				var accessToken = member.Profiles.FirstOrDefault().AccessToken;
				var refreshToken = member.Profiles.FirstOrDefault().RefreshToken;

				await _twitterHttpService.PublishTweetInResponseToAsync(message, rule.ExternalId, accessToken, refreshToken);

				return RedirectToAction("MatchGameLoss", new { Uid = gameUid, gameplayUid = gameplayUid });
			}
			else
			{
				if (game.AccumulatePointsOnLoss)
				{
					var userQuery = GetUserQuery(handshakeKey);

					await _clientMemberService.CreateClientMemberPoints(game.PointsPerLoss, userQuery.UserId, userQuery.UserName, account);
				}

				return RedirectToAction("MatchGameLoss", new { Uid = gameUid, gameplayUid = gameplayUid, accountUid = accountUid, handshakeKey = handshakeKey });
			}
		}

		private UserQuery GetUserQuery(string handshakeKey)
		{
			var handshakeKeyDecoded = HttpUtility.UrlDecode(handshakeKey);

			var decryptedData = CryptoHelper.DecryptHandshakeKey(
			handshakeKeyDecoded,
				_harvesterSettingOptions.EncryptionKey,
				_harvesterSettingOptions.EncryptionIV);

			var userQuery = UserQuery.Parse(decryptedData);
			var externalProfileId = userQuery.UserId;
			var screenName = userQuery.UserName;
			return userQuery;
		}

		private string GetProfileType(string externalClient)
		{
			if(!externalClient.Equals(ExternalClientType.Twitter.ToString(), StringComparison.InvariantCultureIgnoreCase))
			{
				externalClient = ExternalClientType.Client.ToString();
			}

			Enum.TryParse<ExternalClientType>(externalClient, out var externalClientEnum);

			return externalClientEnum switch
			{
				ExternalClientType.Twitter => ProfileType.Twitter.ToString(),
				ExternalClientType.Facebook => ProfileType.Facebook.ToString(),
				ExternalClientType.Client => ProfileType.Client.ToString(),
				_ => ProfileType.Client.ToString(),
			};
		}

		private UserIdentifierType GetUserIdentifierType(string externalClient)
		{
			if (!externalClient.Equals(ExternalClientType.Twitter.ToString(), StringComparison.InvariantCultureIgnoreCase))
			{
				externalClient = ExternalClientType.Client.ToString();
			}

			switch (Enum.Parse<ExternalClientType>(externalClient))
			{
				case ExternalClientType.Twitter:
					return UserIdentifierType.TwitterHandle;
				case ExternalClientType.Facebook:
					return UserIdentifierType.FacebookHandle;
				case ExternalClientType.Client:
					return UserIdentifierType.Client;
			}

			return UserIdentifierType.TwitterHandle;
		}

		private ProfileType GetActualProfileType(string externalClient)
		{
			if (!externalClient.Equals(ExternalClientType.Twitter.ToString(), StringComparison.InvariantCultureIgnoreCase))
			{
				externalClient = ExternalClientType.Client.ToString();
			}

			switch (Enum.Parse<ExternalClientType>(externalClient))
			{
				case ExternalClientType.Twitter:
					return ProfileType.Twitter;
				case ExternalClientType.Facebook:
					return ProfileType.Facebook;
				default:
					return ProfileType.Twitter;
			}
		}

		private async Task SetGameplayAsWonAsync(Guid gameplayUid, PlayMatchGame form, CancellationToken cancellationToken)
		{
			_logger.LogInformation("The gameplay with uid {gameplayUid} was won with the tiles {selectedTiles}",
				gameplayUid,
				form.SelectedTiles);

			_logger.LogInformation("Setting the gameplay as a win.");

			await _matchGameplayService.MarkAsPlayedAndWinAsync(gameplayUid, form.SelectedTiles, cancellationToken);
		}

		private async Task SetGameplayAsLossAsync(Guid gameplayUid, PlayMatchGame form, CancellationToken cancellationToken)
		{
			_logger.LogInformation("The gameplay with uid {gameplayUid} was lost with the tiles {selectedTiles}",
				gameplayUid,
				form.SelectedTiles);

			_logger.LogInformation("Setting the gameplay as a loss.");

			await _matchGameplayService.MarkAsPlayedAndLostAsync(gameplayUid, form.SelectedTiles, cancellationToken);
		}

		private List<Guid> LoadTiles(IEnumerable<Games.Match.Models.MatchTile> tiles)
		{
			var tileList = new List<Guid>();

			foreach (var tile in tiles)
			{
				tileList.Add(tile.Uid);
			}

			return tileList;
		}

		private async Task<bool> CanPlayMatchGame(
			Guid gameUid,
			Guid campaignUid,
			string externalProfileId,
			string screenName,
			CancellationToken cancellationToken)
		{
			var report = await _matchGamePlayReportingRepository.GetGamePlayReportAsync(
				gameUid,
				campaignUid,
				UserIdentifierType.TwitterHandle.ToString(),
				externalProfileId,
				screenName,
				cancellationToken);

			if (report == null) return true;

			var playRules = await _matchPlayRulesRepository.GetAsync(gameUid, cancellationToken);

			if (playRules == null)
			{
				_logger.LogWarning("No play rules have been configured for the game with uid {gameUid}", gameUid);

				return false;
			}

			//Todo: Put this back
			//return report.Plays < playRules.PlayPerGame;
			return true;
		}

		private IActionResult ErrorRedirect(string reason, int statusCode)
		{
			_logger.LogError("Redirecting to the error handler with reason {reason}", reason);

			return RedirectToActionPermanent(
				"HandleErrorCode",
				"Error",
				new { reason, statusCode });
		}

		private string GetProviderCode(string externalClient)
		{
			if (!externalClient.Equals(ExternalClientType.Twitter.ToString(), StringComparison.InvariantCultureIgnoreCase))
			{
				externalClient = ExternalClientType.Client.ToString();
			}

			return Enum.Parse<ExternalClientType>(externalClient) switch
			{
				ExternalClientType.Twitter => hrvster.Models.Constants.TwitterProvider,
				ExternalClientType.Facebook => hrvster.Models.Constants.FaceBookProvider,
				ExternalClientType.Instagram => hrvster.Models.Constants.InstagramProvider,
				ExternalClientType.Client => hrvster.Models.Constants.ClientProvider,
				_ => hrvster.Models.Constants.TwitterProvider,
			};
		}

		private async Task SignInUserAsync(UserProviderDetails userDetails, ExternalClientType externalClient)
		{
			var principal = BuildPrincipalFromClaims(userDetails, externalClient);

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
		}

		private static ClaimsPrincipal BuildPrincipalFromClaims(
			UserProviderDetails userDetails, ExternalClientType externalClient)
		{
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, userDetails.ExternalProfileId),
				new Claim(ClaimTypes.Name, userDetails.Name),
				new Claim(HrvsterClaimTypes.DisplayName, userDetails.ScreenName),
				new Claim(HrvsterClaimTypes.AccessToken, userDetails.AccessToken),
				new Claim(HrvsterClaimTypes.RefreshToken, userDetails.RefreshToken ?? Guid.NewGuid().ToString()),
				new Claim(HrvsterClaimTypes.Description, userDetails.Description),
				new Claim(HrvsterClaimTypes.ExternalProfileId, userDetails.ExternalProfileId),
				new Claim(HrvsterClaimTypes.ExternalClient, externalClient.ToString())
			};

			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

			var principal = new ClaimsPrincipal(identity);

			return principal;
		}
	}
}
