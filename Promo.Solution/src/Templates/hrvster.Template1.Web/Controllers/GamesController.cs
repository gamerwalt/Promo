using AutoMapper;
using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.MatchGames;
using hrvster.ApplicationServices.Reporting;
using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Harvester;
using hrvster.Infrastructure.MatchGames;
using hrvster.Infrastructure.Reporting;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Options;
using hrvster.Template1.Web.Models;
using hrvster.Templates.Core.Common;
using hrvster.Templates.Infrastructure.Customizations;
using hrvster.Templates.Models.Members;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Constants = hrvster.Templates.Core.Common.Constants;

namespace hrvster.Template1.Web.Controllers
{
	public class GamesController : Controller
	{
		private readonly ILogger _logger;
		private readonly IGameService _gameService;
		private readonly ISessionManager _sessionManager;
		private readonly IMemberProfileRepository _memberProfileRepository;
		private readonly IMemberRepository _memberRepository;
		private readonly IMapper _mapper;
		private readonly ISweepstakesService _sweepStakeService;
		private readonly IPlaySweepstakeService _playSweepstakeService;
		private readonly ITwitterHttpService _twitterHttpService;
		private readonly IBlacklistedUserRepository _blackListedUserRepository;
		private readonly IMatchGamePlayReportingRepository _matchGamePlayReportingRepository;
		private readonly IMatchPlayRulesRepository _matchPlayRulesRepository;
		private readonly ICampaignService _campaignService;
		private readonly IAccountsService _accountsService;
		private readonly IMatchGameplayService _matchGamePlayService;
		private readonly HarvesterSettingsOptions _harvesterSettings;
		private readonly IMatchGamesService _matchGameService;
		private readonly IMatchGameBoardService _gameBoardService;
		private readonly IReportingService _reportingService;
		private readonly IMatchGameRulesRepository _matchGameRulesRepository;
		private readonly IProfilesRepository _profilesRepository;

		public GamesController(
			ILoggerFactory loggerFactory,
			IGameService gameService,
			ISessionManager sessionManager,
			IMemberProfileRepository memberProfileRepository,
			IMemberRepository memberRepository,
			IMapper mapper,
			ISweepstakesService sweepstakesService,
			IPlaySweepstakeService playSweepstakeService,
			ITwitterHttpService twitterHttpService,
			IBlacklistedUserRepository blacklistedUserRepository,
			IMatchGamePlayReportingRepository matchGamePlayReportingRepository,
			ICampaignService campaignService,
			IMatchPlayRulesRepository matchPlayRulesRepository,
			IAccountsService accountsService,
			IMatchGameplayService matchGameplayService,
			HarvesterSettingsOptions hrvsterSettings,
			IMatchGamesService matchGameService,
			IMatchGameBoardService gameBoardService,
			IReportingService reportingService,
			IMatchGameRulesRepository matchGameRulesRepository,
			IProfilesRepository profilesRepository)
		{
			_logger = loggerFactory.CreateLogger<GamesController>();
			_gameService = gameService;
			_sessionManager = sessionManager;
			_memberProfileRepository = memberProfileRepository;
			_memberRepository = memberRepository;
			_mapper = mapper;
			_sweepStakeService = sweepstakesService;
			_playSweepstakeService = playSweepstakeService;
			_twitterHttpService = twitterHttpService;
			_blackListedUserRepository = blacklistedUserRepository;
			_matchGamePlayReportingRepository = matchGamePlayReportingRepository;
			_campaignService = campaignService;
			_matchPlayRulesRepository = matchPlayRulesRepository;
			_accountsService = accountsService;
			_matchGamePlayService = matchGameplayService;
			_harvesterSettings = hrvsterSettings;
			_matchGameService = matchGameService;
			_gameBoardService = gameBoardService;
			_reportingService = reportingService;
			_matchGameRulesRepository = matchGameRulesRepository;
			_profilesRepository = profilesRepository;
		}

		[HttpGet("Games")]
		public async Task<IActionResult> Index()
		{
			return new OkObjectResult("Ok");
		}

		[HttpGet("Games/{gameUid}/error")]
		public async Task<IActionResult> Error(string subdomainName, Guid gameUid, string reason)
		{
			_logger.LogError("An error occurred for the subdomain {subdomain} and game uid {gameUid}. Error Message: {message}",
				subdomainName, gameUid, reason);

			return RedirectToAction(nameof(ErrorController.HandleErrorCode), nameof(ErrorController), new { statusCode = 500 });
		}


		[HttpGet("Games/Pick3MatchGame/{gameUid}")]
		public async Task<IActionResult> Pick3MatchGame(
			Guid gameUid,
			[FromQuery] string externalClient,
			[FromQuery] Guid ruleUid)
		{
			HttpContext.Items.TryGetValue(Constants.ContextItemsSubdomain, out var subdomain);

			var subdomainName = subdomain.ToString();

			_logger.LogInformation("Received a request to play the pick 3 match game with uid {gameUid} and from the social {externalClient}",
				gameUid,
				externalClient);

			_logger.LogInformation("This will play the pick 3 match for game with uid {gameUid} and the user will have to logon via {externalClient}. The rule is {ruleUid}",
				gameUid,
				externalClient,
				ruleUid);

			if (!await _gameService.GameExistsForSubdomainAsync(subdomainName, gameUid))
				return RedirectToAction("GameDoesNotExist", new { subdomainName, gameUid });

			_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
			_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.MatchGames.ToString());

			var providerCode = GetProviderCode(externalClient);

			var url = _twitterHttpService.GetAuthorizationUrl(Request.Scheme, Request.Host.Value, $"/Games/Validate{providerCode}Auth");

			return new RedirectResult(url);
		}

		[HttpGet("Games/Sweeps/{gameUid}")]
		public async Task<IActionResult> Sweeps(Guid gameUid, [FromQuery] string externalClient, [FromQuery] Guid ruleUid)
		{
			HttpContext.Items.TryGetValue(Constants.ContextItemsSubdomain, out var subdomain);
			var subdomainName = subdomain.ToString();

			_logger.LogInformation("Received a request to play the sweeps game with uid {gameUid} and from the social {externalClient}",
				gameUid,
				externalClient);

			_logger.LogInformation("This will play the sweepstakes for game with uid {gameUid} and the user will have to logon via {externalClient}. The rule is {ruleUid}",
				gameUid,
				externalClient,
				ruleUid);

			if (!await _gameService.GameExistsForSubdomainAsync(subdomainName, gameUid))
			{
				_logger.LogWarning("Game with uid {gameUid} does not exist.", gameUid);

				return RedirectToAction(
					"Error", new { subdomainName, gameUid, reason = GameErrorReasons.GameDoesNotExist });
			}

			_logger.LogInformation("The game with uid {gameUid} exists for the company with subdomain {subdomain}", subdomainName);

			_sessionManager.SetKeyValue(TemplateSessionConstants.Subdomain, subdomainName);
			_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
			_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.Sweeps.ToString());

			var providerCode = GetProviderCode(externalClient);

			var url = _twitterHttpService.GetAuthorizationUrl(Request.Scheme, Request.Host.Value, $"/Games/Validate{providerCode}Auth");

			_logger.LogInformation("Redirecting to {url} for authorization.", url);

			return new RedirectResult(url);
		}

		public async Task<IActionResult> ValidateTwitterAuth()
		{
			_logger.LogInformation("Validating the Twitter user.");
			var subdomainName = _sessionManager.GetValue(TemplateSessionConstants.Subdomain);
			var gameUid = Guid.Parse(_sessionManager.GetValue(TemplateSessionConstants.GameUid));

			if (Request.Query["denied"].Count > 0)
				return RedirectToAction(
					"Error",
					new
					{
						subdomainName,
						gameUid,
						reason = GameErrorReasons.TwitterAuthenticationFailed
					});

			if (!Guid.TryParse(_sessionManager.GetValue(TemplateSessionConstants.GameUid), out var gameOutUid))
				return RedirectToAction(
					"Error",
					new
					{
						subdomainName,
						gameUid,
						reason = GameErrorReasons.GameDoesNotExist
					});

			if (!Guid.TryParse(_sessionManager.GetValue(TemplateSessionConstants.RuleUid), out var ruleUid))
				return RedirectToAction(
					"Error",
					new
					{
						subdomainName,
						gameUid,
						reason = GameErrorReasons.BadRule
					});

			var externalClient = _sessionManager.GetValue(TemplateSessionConstants.ExternalClient);

			if (string.IsNullOrWhiteSpace(externalClient))
				return RedirectToAction(
					"Error",
					new
					{
						subdomainName,
						gameUid,
						reason = GameErrorReasons.BadExternalClient
					});

			var gameType = _sessionManager.GetValue(TemplateSessionConstants.GameType);

			if (string.IsNullOrWhiteSpace(gameType))
				return RedirectToAction(
					"Error",
					new
					{
						subdomainName,
						gameUid,
						reason = GameErrorReasons.BadGameType
					});

			var code = Request.Query["code"].ToString();

			var accessTokenObject = await _twitterHttpService.GetAccessTokenAsyncForOfflineAccess(
				code, Request.Scheme, Request.Host.Value, $"/Games/ValidateTWITTERAuth");

			var userDetails = await _twitterHttpService.GetAuthenticatedUserAsync(accessTokenObject);

			if (userDetails == null)
				return RedirectToAction("Error", new { subdomainName, gameUid, reason = GameErrorReasons.TwitterAuthenticationFailed });

			var gameDetails = await _gameService.GetGameDetailsAsync(gameType, gameUid);

			if (DateTime.UtcNow > gameDetails.EndDate)
				return RedirectToAction(
					"Error",
					new
					{
						subdomainName,
						gameUid,
						reason = GameErrorReasons.GameCompleted
					});

			if (DateTime.UtcNow < gameDetails.StartDate)
				return RedirectToAction(
					"Error",
					new
					{
						subdomainName,
						gameUid,
						reason = GameErrorReasons.GameNotStarted
					});

			_logger.LogInformation("Retrieving the member profile for the external profile id {externalProfileId}", userDetails.ExternalProfileId);

			var memberProfile = await _memberProfileRepository.GetByExternalProfileIdAsync(userDetails.ExternalProfileId);

			await SignInUserAsync(userDetails);

			if (memberProfile == null)
			{
				_logger.LogWarning("Member profile does not exist for the external profile id {externalProfileId}", userDetails.ExternalProfileId);

				_sessionManager.SetKeyValue(TemplateSessionConstants.UserDetails, userDetails.ToJsonString());

				_logger.LogInformation("Redirecting to create a member for this authenticated user.");

				return RedirectToAction("CreateMember", new { gameUid, ruleUid, externalClient });
			}

			await UpdateMemberProfileAsync(userDetails, memberProfile);

			return RedirectToGamePlayFromGameType(gameUid, ruleUid, externalClient, gameType);
		}

		public async Task<IActionResult> ValidateFBAuth(string code, string state, string error, string error_description)
		{
			return new OkObjectResult("Facebook authentication validated.");
		}

		[Authorize]
		[HttpGet("Games/Sweeps/{gameUid}/Rule/{ruleUid}/member/create")]
		public async Task<IActionResult> CreateMember(Guid gameUid, Guid ruleUid, [FromQuery] string externalClient)
		{
			var gameType = _sessionManager.GetValue(TemplateSessionConstants.GameType);

			if (string.IsNullOrWhiteSpace(gameType))
				return View("BadGameType");

			var gameDetails = await _gameService.GetGameDetailsAsync(gameType, gameUid);

			if (DateTime.UtcNow > gameDetails.EndDate || DateTime.UtcNow < gameDetails.StartDate)
				return View("BadGame");

			var model = new CreateMember()
			{
				GameUid = gameUid,
				RuleUid = ruleUid,
				ExternalClient = externalClient,
				GameName = gameDetails.Name,
				GameStartDate = gameDetails.StartDate,
				GameEndDate = gameDetails.EndDate
			};

			return View(model);
		}

		[Authorize]
		[HttpPost("Games/Sweeps/{gameUid}/Rule/{ruleUid}/member/create")]
		public async Task<IActionResult> CreateMember(
			Guid gameUid,
			Guid ruleUid,
			[FromQuery] string externalClient,
			CreateMember form)
		{
			var gameType = _sessionManager.GetValue(TemplateSessionConstants.GameType);

			if (string.IsNullOrWhiteSpace(gameType))
				return View("BadGameType");

			var gameDetails = await _gameService.GetGameDetailsAsync(gameType, gameUid);

			if (DateTime.UtcNow > gameDetails.EndDate || DateTime.UtcNow < gameDetails.StartDate)
				return View("BadGame");

			if (ModelState.IsValid)
			{
				var userClaims = User.Identities.FirstOrDefault()?.Claims;
				var externalProfileId = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?.Value;
				var screenName = userClaims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
				var displayName = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.DisplayName)?.Value;
				var accessToken = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.AccessToken)?.Value;
				var refreshToken = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.RefreshToken)?.Value;
				var description = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.Description)?.Value;

				var newMember = _mapper.Map<Member>(form);
				newMember.Uid = Guid.NewGuid();
				var memberProfileUid = Guid.NewGuid();

				await _memberRepository.CreateMemberAsync(newMember);
				await _memberProfileRepository.CreateMemberProfileAsync(new MemberProfile
				{
					Member = newMember,
					Uid = memberProfileUid,
					Description = description,
					ExternalProfileId = externalProfileId,
					Name = displayName,
					ScreenName = screenName,
					ProfileType = ProfileType.Twitter,
					AccessToken = accessToken,
					RefreshToken = refreshToken
				});

				if (gameType == GameTypes.Sweepstake)
					return RedirectToActionPermanent("SweepsPlay", new { gameUid, ruleUid, externalClient });

				return RedirectToActionPermanent("Pick3MatchPlay", new { gameUid, ruleUid, externalClient });
			}

			return View(form);
		}

		[Authorize]
		[HttpGet("Games/Pick3MatchPlay/{gameUid}/rule/{ruleUid}/Play")]
		public async Task<IActionResult> Pick3MatchPlay(Guid gameUid, Guid ruleUid, [FromQuery] string externalClient, CancellationToken cancellationToken)
		{
			var gameType = _sessionManager.GetValue(TemplateSessionConstants.GameType);
			var game = await _gameService.GetGameDetailsAsync(gameType, gameUid);
			var blacklisted = await _blackListedUserRepository.GetBlacklistedUsersAsync(gameUid);
			var campaign = await _campaignService.GetCampaignFromGameUidAsync(gameUid, gameType);
			var account = await _accountsService.GetAccountByUidAsync(campaign.Account.AccountUid);
			var profile = await _accountsService.GetProfileAsync(account.AccountUid, GetActualProfileType(externalClient));

			_logger.LogInformation("Retrieved a total of {count} blacklisted users for the game {gameUid}",
				blacklisted.Count(),
				gameUid);

			var userClaims = User.Identities.FirstOrDefault()?.Claims;
			var externalProfileId = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?.Value;
			var screenName = userClaims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

			var canPlayGame = await CanPlayMatchGame(gameUid, campaign.CampaignUid, externalProfileId, screenName, cancellationToken);

			if (!canPlayGame)
				return RedirectToAction("CannotPlayMatchGame", new { gameUid, ruleUid, externalClient });

			if (blacklisted.Any(c => c.UserId.Equals(externalProfileId)))
				return RedirectToAction("CannotPlayMatchGame", new { gameUid, ruleUid, externalClient });

			var rules = await _gameService.GetRulesAsync(GameTypesEnum.MatchGames, gameUid, cancellationToken);
			var rule = rules.FirstOrDefault(r => r.RuleUid == ruleUid);

			if (rule.MustBeFollowing)
			{
				/*
				var followers = await _twitterHttpService.GetFollowersAsync(externalProfileId);

				if (!followers.Any(c => c.Id == profile.ExternalProfileId))*/

				//return RedirectToAction("CannotPlayMatchGame", new { gameUid, ruleUid, externalClient });
			}

			var gamePlayUid = await _matchGamePlayService.ReserveGameboardAsync(
				gameUid, screenName, UserIdentifierType.TwitterHandle, cancellationToken);

			return RedirectToActionPermanent("PlayMatch3Game", new { gameUid, ruleUid, externalClient, gamePlayUid });
		}

		[Authorize]
		[HttpGet("Games/PlayMatch3Game/Game/{gameUid}/Rule/{ruleUid}/Play/{gamePlayUid}")]
		public async Task<IActionResult> PlayMatch3Game(
			Guid gameUid,
			Guid ruleUid,
			Guid gamePlayUid,
			[FromQuery] string externalClient,
			CancellationToken cancellationToken)
		{
			if (User.Identity.IsAuthenticated)
			{
				if (!await _matchGameService.GamePlayableAsync(gameUid, cancellationToken))
				{
					return RedirectToAction("GameUnplayable", new
					{
						gameUid = gameUid,
						gamePlayUid = gamePlayUid
					});
				}

				if (!await _gameBoardService.IsBoardPlayableByGameplayAsync(gamePlayUid, cancellationToken))
				{
					return RedirectToAction("BoardUnplayable", new
					{
						gameUid = gameUid,
						gamePlayUid = gamePlayUid
					});
				}

				var gameplay = await _matchGamePlayService.GetAsync(gamePlayUid, cancellationToken);
				var gameboard = await _gameBoardService.GetGameBoardAsync(gameplay.MatchBoard.Uid, cancellationToken);
				var tiles = await _gameBoardService.GetTilesAsync(gameboard.Uid, cancellationToken);
				if (tiles == null)
				{
					return RedirectToAction("NoTilesAvailable", new
					{
						gameUid = gameUid,
						gamePlayUid = gamePlayUid
					});
				}

				var model = new PlayMatchGame
				{
					GameplayUid = gameplay.Uid,
					GameUid = gameboard.MatchGame.Uid,
					GameName = gameboard.MatchGame.Name,
					MatchBoard = _mapper.Map<Models.MatchBoard>(gameboard),
					Tiles = LoadTiles(tiles),
					ExternalClient = externalClient
				};

				return View(model);
			}
			else
			{
				return RedirectToAction("Pick3MatchGame", new { gameUid, ruleUid, externalClient });
			}
		}

		[Authorize]
		[HttpPost("Games/PlayMatch3Game/Game/{gameUid}/Rule/{ruleUid}/Play/{gamePlayUid}")]
		public async Task<IActionResult> PlayMatch3Game(
			Guid gameUid,
			Guid ruleUid,
			Guid gamePlayUid,
			[FromQuery] string? externalClient,
			PlayMatchGame form,
			CancellationToken cancellationToken)
		{

			if (User.Identity.IsAuthenticated)
			{
				if (!await _matchGameService.GamePlayableAsync(gameUid, cancellationToken))
				{
					return RedirectToAction("GameUnplayable", new
					{
						Uid = gameUid,
						gameplayUid = gamePlayUid
					});
				}

				_logger.LogInformation("Verifying the gameplay and the gameboard");

				if (!await _gameBoardService.IsBoardPlayableByGameplayAsync(gamePlayUid, cancellationToken))
				{
					return RedirectToAction("BoardUnplayable", new
					{
						Uid = gameUid,
						gameplayUid = gamePlayUid
					});
				}

				if (ModelState.IsValid)
				{
					var userClaims = User.Identities.FirstOrDefault()?.Claims;
					var externalProfileId = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?.Value;
					var screenName = userClaims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

					await _reportingService.SaveMatchGameAsync(
						gameUid,
						gamePlayUid,
						form.BoardUid.GetValueOrDefault(),
						UserIdentifierType.TwitterHandle,
						screenName,
						externalProfileId,
						cancellationToken);

					return await ReturnWinOrLossAsync(gameUid, gamePlayUid, ruleUid, form, externalClient, cancellationToken);
				}

				return Redirect(_harvesterSettings.HrvsterUrl);
			}
			else
			{
				return RedirectToAction("Pick3MatchGame", new { gameUid, ruleUid, externalClient });
			}
		}

		[Authorize]
		[HttpGet("Games/Sweeps/{gameUid}/Rule/{ruleUid}/Play")]
		public async Task<IActionResult> SweepsPlay(Guid gameUid, Guid ruleUid, [FromQuery] string externalClient)
		{
			//retrieve user details to know who is logged in and display the name of the person to play
			var userName = User.Identities
				.FirstOrDefault()?
				.Claims?
				.FirstOrDefault(x => x.Type == ClaimTypes.Name)?
				.Value;
			var externalProfileId = User.Identities
				.FirstOrDefault()?
				.Claims?
				.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?
				.Value;

			var gameType = _sessionManager.GetValue(TemplateSessionConstants.GameType);

			if (string.IsNullOrWhiteSpace(gameType))
				return View("BadGameType");

			var gameDetails = await _gameService.GetGameDetailsAsync(gameType, gameUid);

			if (DateTime.UtcNow > gameDetails.EndDate)
				return RedirectToAction("GameCompleted", new { gameUid, ruleUid, externalClient });
			if (DateTime.UtcNow < gameDetails.StartDate)
				return RedirectToAction("GameNotStarted", new { gameUid, ruleUid, externalClient });

			var member = await _memberRepository.GetMemberViaExternalProfileIdAsync(GetProfileType(externalClient), externalProfileId);

			var sweepGame = await _sweepStakeService.GetSweepstakeAsync(gameUid, default);

			var playGame = new PlaySweepsGame
			{
				GameUid = gameUid,
				RuleUid = ruleUid,
				ExternalClient = externalClient,
				NumberOfEntries = 1, //we can get this from the rules later on
				GameName = gameDetails.Name,
				GameStartDate = gameDetails.StartDate,
				GameEndDate = gameDetails.EndDate,
				ScreenName = member.Profiles.FirstOrDefault()?.Name,
				FirstName = member.FirstName,
				LastName = member.LastName
			};

			return View(playGame);
		}

		[Authorize]
		[HttpPost("Games/Sweeps/{gameUid}/Rule/{ruleUid}/Play")]
		public async Task<IActionResult> SweepsPlay(
			Guid gameUid,
			Guid ruleUid,
			[FromQuery] string externalClient,
			PlaySweepsGame form)
		{
			var userName = User.Identities
				.FirstOrDefault()?
				.Claims?
				.FirstOrDefault(x => x.Type == ClaimTypes.Name)?
				.Value;
			var externalProfileId = User.Identities
				.FirstOrDefault()?
				.Claims?
				.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?
				.Value;

			var gameType = _sessionManager.GetValue(TemplateSessionConstants.GameType);

			if (string.IsNullOrWhiteSpace(gameType))
				return View("BadGameType");

			var gameDetails = await _gameService.GetGameDetailsAsync(gameType, gameUid);

			if (DateTime.UtcNow > gameDetails.EndDate || DateTime.UtcNow < gameDetails.StartDate)
				return View("BadGame");

			var member = await _memberRepository.GetMemberViaExternalProfileIdAsync(GetProfileType(externalClient), externalProfileId);

			var sweepGame = await _sweepStakeService.GetSweepstakeAsync(gameUid, default);

			var entryUid = await _playSweepstakeService.EnterSweepstakeAsync(
				sweepGame,
				ruleUid,
				externalProfileId,
				member.Profiles.FirstOrDefault()?.Name,
				GetUserIdentifierType(externalClient),
				MessageConstants.SweepstakeTweetReply,
				externalClient,
				member.Profiles.FirstOrDefault()?.AccessToken,
				member.Profiles.FirstOrDefault()?.RefreshToken);

			if (entryUid != Guid.Empty)
				return RedirectToActionPermanent("SweepEntered", new { entryUid = entryUid });

			return RedirectToActionPermanent("SweepEntryFailed", new { gameUid, ruleUid, externalClient });
		}

		[Authorize]
		[HttpGet("Games/Sweeps/SweepEntered/{entryUid}")]
		public async Task<IActionResult> SweepEntered(Guid entryUid)
		{
			var entry = await _sweepStakeService.GetSweepstakeEntryAsync(entryUid);

			if (entry == null) return RedirectToAction("BadSweepEntry");

			var rules = await _sweepStakeService.GetSweepRulesAsync(entry.Sweep.SweepUid);
			var rule = rules.FirstOrDefault(r => r.RuleUid == entry.Rule.RuleUid);
			entry.Rule = rule;

			return View(entry);
		}

		[Authorize]
		[HttpGet("Games/Sweeps/SweepEntryFailed/{gameUid}/Rule/{ruleUid}")]
		public async Task<IActionResult> SweepEntryFailed(Guid gameUid, Guid ruleUid, string externalClient)
		{
			return new OkObjectResult(new { gameUid, ruleUid, externalClient });
		}

		[Authorize]
		[HttpGet]
		[Route("/Games/matchgame/win/{Uid:Guid}/gameplay/{gameplayUid:Guid}")]
		public async Task<IActionResult> MatchGameWin(
			Guid Uid,
			Guid gameplayUid,
			CancellationToken cancellationToken)
		{
			var gameplay = await _matchGamePlayService.GetAsync(gameplayUid, cancellationToken);

			if (!gameplay.PlayedIndicator && !gameplay.MatchBoard.PlayedIndicator && !gameplay.WinIndicator)
			{
				return Redirect(_harvesterSettings.HrvsterUrl);
			}

			var gameboard = await _gameBoardService.GetGameBoardAsync(gameplay.MatchBoard.Uid, cancellationToken);
			var tiles = await _gameBoardService.GetTilesAsync(gameboard.Uid, cancellationToken);

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

		[Authorize]
		[HttpGet]
		[Route("/Games/matchgame/loss/{Uid:Guid}/gameplay/{gameplayUid:Guid}")]
		public async Task<IActionResult> MatchGameLoss(
			Guid Uid,
			Guid gameplayUid,
			CancellationToken cancellationToken)
		{
			var gameplay = await _matchGamePlayService.GetAsync(gameplayUid, cancellationToken);

			if (!gameplay.PlayedIndicator && !gameplay.MatchBoard.PlayedIndicator && gameplay.WinIndicator)
			{
				return Redirect(_harvesterSettings.HrvsterUrl);
			}

			var gameboard = await _gameBoardService.GetGameBoardAsync(gameplay.MatchBoard.Uid, cancellationToken);
			var tiles = await _gameBoardService.GetTilesAsync(gameboard.Uid, cancellationToken);

			var model = new GameplayLoss
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

		private async Task<IActionResult> ReturnWinOrLossAsync(
			Guid gameUid,
			Guid gameplayUid,
			Guid ruleUid,
			PlayMatchGame form,
			string externalClient,
			CancellationToken cancellationToken)
		{
			var userClaims = User.Identities.FirstOrDefault()?.Claims;
			var externalProfileId = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?.Value;
			var screenName = userClaims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

			var member = await _memberRepository.GetMemberViaExternalProfileIdAsync(GetProfileType(externalClient), externalProfileId);
			var rule = await _matchGameRulesRepository.GetAsync(ruleUid);
			var game = await _matchGameService.GetMatchGameAsync(gameUid, cancellationToken);
			var campaign = await _campaignService.GetCampaignAsync(game.CampaignUid, cancellationToken);
			var account = await _accountsService.GetAccountByUidAsync(campaign.Account.AccountUid);
			var profiles = await _profilesRepository.GetProfileAsync(account.AccountUid, GetActualProfileType(externalClient));

			var hashtag = rule.WatchValue;
			var replyTo = $"@{profiles.ScreenName}";
			var message = string.Empty;
			var accessToken = member.Profiles.FirstOrDefault().AccessToken;
			var refreshToken = member.Profiles.FirstOrDefault().RefreshToken;

			if (await _matchGamePlayService.IsAWinAsync(gameUid: gameUid, gameplayUid, form.SelectedTiles, cancellationToken))
			{
				await SetGameplayAsWonAsync(gameplayUid, form, cancellationToken);
				var prize = await _matchGameService.GetPrizeFromGameplayUidAsync(gameplayUid, cancellationToken);

				message = $"{replyTo} I just played the Pick 3 game {game.Name} and won. I just won a prize with a value of ${prize.Value}. {Environment.NewLine} {hashtag}";
				await _twitterHttpService.PublishTweetInResponseToAsync(message, rule.ExternalId, accessToken, refreshToken);

				return RedirectToAction("MatchGameWin", new { Uid = gameUid, gameplayUid = gameplayUid });
			}

			await SetGameplayAsLossAsync(gameplayUid, form, cancellationToken);

			message = $"{replyTo} I just played the Pick 3 game {game.Name} and lost unfortunately ;). {Environment.NewLine} {hashtag}";
			await _twitterHttpService.PublishTweetInResponseToAsync(message, rule.ExternalId, accessToken, refreshToken);

			return RedirectToAction("MatchGameLoss", new { Uid = gameUid, gameplayUid = gameplayUid });
		}

		private async Task SetGameplayAsWonAsync(Guid gameplayUid, PlayMatchGame form, CancellationToken cancellationToken)
		{
			_logger.LogInformation("The gameplay with uid {gameplayUid} was won with the tiles {selectedTiles}",
				gameplayUid,
				form.SelectedTiles);

			_logger.LogInformation("Setting the gameplay as a win.");

			await _matchGamePlayService.MarkAsPlayedAndWinAsync(gameplayUid, form.SelectedTiles, cancellationToken);
		}

		private async Task SetGameplayAsLossAsync(Guid gameplayUid, PlayMatchGame form, CancellationToken cancellationToken)
		{
			_logger.LogInformation("The gameplay with uid {gameplayUid} was lost with the tiles {selectedTiles}",
				gameplayUid,
				form.SelectedTiles);

			_logger.LogInformation("Setting the gameplay as a loss.");

			await _matchGamePlayService.MarkAsPlayedAndLostAsync(gameplayUid, form.SelectedTiles, cancellationToken);
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

		private UserIdentifierType GetUserIdentifierType(string externalClient)
		{
			switch (Enum.Parse<ExternalClientType>(externalClient))
			{
				case ExternalClientType.Twitter:
					return UserIdentifierType.TwitterHandle;
				case ExternalClientType.Facebook:
					return UserIdentifierType.FacebookHandle;
			}

			return UserIdentifierType.TwitterHandle;
		}

		private string GetProfileType(string externalClient)
		{
			switch (Enum.Parse<ExternalClientType>(externalClient))
			{
				case ExternalClientType.Twitter:
					return ProfileType.Twitter.ToString();
				case ExternalClientType.Facebook:
					return ProfileType.Facebook.ToString();
			}

			return ProfileType.Twitter.ToString();
		}

		private ProfileType GetActualProfileType(string externalClient)
		{
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

		private string GetProviderCode(string externalClient)
			=> Enum.Parse<ExternalClientType>(externalClient) switch
			{
				ExternalClientType.Twitter => hrvster.Models.Constants.TwitterProvider,
				ExternalClientType.Facebook => hrvster.Models.Constants.FaceBookProvider,
				ExternalClientType.Instagram => hrvster.Models.Constants.InstagramProvider,
				_ => hrvster.Models.Constants.TwitterProvider,
			};

		private async Task SignInUserAsync(UserProviderDetails userDetails)
		{
			var principal = BuildPrincipalFromClaims(userDetails);

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
		}

		private async Task UpdateMemberProfileAsync(UserProviderDetails userDetails, MemberProfile memberProfile)
		{
			_logger.LogInformation("Authenticated member already exists. We will try to update as much as possible.");

			memberProfile.AccessToken = userDetails.AccessToken;
			memberProfile.RefreshToken = userDetails.RefreshToken;
			memberProfile.Name = userDetails.Name;
			memberProfile.ScreenName = userDetails.ScreenName;
			memberProfile.PictureUrl = userDetails.ProfileImage;

			await _memberProfileRepository.UpdateMemberProfileAsync(memberProfile);

			_logger.LogWarning("Member profile found for external profile id {externalProfileId}", userDetails.ExternalProfileId);
		}

		private IActionResult RedirectToGamePlayFromGameType(Guid gameUid, Guid ruleUid, string externalClient, string gameType)
		{
			if (gameType == GameTypes.Sweepstake || gameType == GameTypesEnum.Sweeps.ToString())
				return RedirectToAction("SweepsPlay", new { gameUid, ruleUid, externalClient });

			return RedirectToAction("Pick3MatchPlay", new { gameUid, ruleUid, externalClient });
		}

		private static ClaimsPrincipal BuildPrincipalFromClaims(UserProviderDetails userDetails)
		{
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, userDetails.ExternalProfileId),
				new Claim(ClaimTypes.Name, userDetails.Name),
				new Claim(HrvsterClaimTypes.DisplayName, userDetails.ScreenName),
				new Claim(HrvsterClaimTypes.AccessToken, userDetails.AccessToken),
				new Claim(HrvsterClaimTypes.RefreshToken, userDetails.RefreshToken),
				new Claim(HrvsterClaimTypes.Description, userDetails.Description),
				new Claim(HrvsterClaimTypes.ExternalProfileId, userDetails.ExternalProfileId)
			};

			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);

			return principal;
		}
	}
}
