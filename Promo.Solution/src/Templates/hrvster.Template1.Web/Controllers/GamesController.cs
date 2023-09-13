using AutoMapper;
using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.Communications;
using hrvster.ApplicationServices.Extensions.LogExtensions;
using hrvster.ApplicationServices.MatchGames;
using hrvster.ApplicationServices.Reporting;
using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Exceptions;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Harvester;
using hrvster.Infrastructure.MatchGames;
using hrvster.Infrastructure.Reporting;
using hrvster.Models;
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
		private readonly ICommunicationService _communicationService;

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
			IProfilesRepository profilesRepository,
			ICommunicationService communicationService)
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
			_communicationService = communicationService;
		}

		[HttpGet("Games")]
		public async Task<IActionResult> Index()
		{
			return new OkObjectResult("Ok");
		}

		[HttpGet("Games/{gameUid}/error")]
		public IActionResult Error(Guid gameUid, string reason)
		{
			_logger.LogError(reason);

			return RedirectToAction("HandleErrorCode", "ErrorController", new { statusCode = 400 });
		}

		[HttpGet("Games/{gameUid}/GameDoesNotExist")]
		public IActionResult GameDoesNotExist(Guid gameUid, [FromQuery] string externalClient, [FromQuery] Guid ruleUid)
		{
			_logger.LogWarning("Game does not exist for the game uid {gameUid} with externalClient {externalClient} for rule {ruleUid}",
				gameUid,
				externalClient,
				ruleUid);

			return RedirectToAction(nameof(ErrorController.HandleErrorCode), new { statusCode = 404 });
		}


		[HttpGet("Games/Pick3MatchGame/{gameUid}")]
		public async Task<IActionResult> Pick3MatchGame(
			Guid gameUid,
			[FromQuery] string externalClient,
			[FromQuery] Guid ruleUid)
		{
			var campaignUidString = _sessionManager.GetValue(SessionConstants.CampaignUid);
			var accountUidString = _sessionManager.GetValue(SessionConstants.AccountUid);

			if (string.IsNullOrEmpty(campaignUidString) || string.IsNullOrEmpty(accountUidString))
			{
				_logger.LogWarning("CampaignUid or AccountUid is null or empty for the game uid {gameUid} with externalClient {externalClient} for rule {ruleUid}",
					gameUid,
					externalClient,
					ruleUid);

				return RedirectToAction(nameof(ErrorController.HandleErrorCode), new { statusCode = 400 });
			}

			var campaignUid = Guid.Parse(campaignUidString);
			var accountUid = Guid.Parse(_sessionManager.GetValue(SessionConstants.AccountUid));

			if (campaignUid == Guid.Empty || accountUid == Guid.Empty)
			{
				_logger.LogWarning("CampaignUid or AccountUid is empty for the game uid {gameUid} with externalClient {externalClient} for rule {ruleUid}",
					gameUid,
					externalClient,
					ruleUid);

				return RedirectToAction(nameof(ErrorController.HandleErrorCode), new { statusCode = 400 });
			}

			using (_logger.Pick3MatchGameLoggingEntry(ruleUid, gameUid, campaignUid, accountUid))
			{
				_logger.LogInformation("Received a request to play the pick 3 match game from the social {externalClient}",
				externalClient);

				if (!await _gameService.GameExistsForAccountAsync(accountUid, gameUid))
				{
					return RedirectToAction(nameof(GameDoesNotExist), new { gameUid, externalClient, ruleUid });
				}

				_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
				_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
				_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
				_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.MatchGames.ToString());
			}

			var authKey = _sessionManager.GetValue(TemplateSessionConstants.AuthenticationKey);

			if (!string.IsNullOrWhiteSpace(authKey))
			{
				var userDetails = _twitterHttpService.GetAuthenticatedUserAsync(authKey);

				if (userDetails != null)
				{
					return RedirectToGame(GameTypesEnum.MatchGames.ToString(), gameUid, ruleUid, externalClient);
				}
			}

			var code = GetProviderCode(externalClient);
			_logger.LogInformation("Redirecting with provider code {code}", code);

			return RedirectToAction(
				"ExternalLogin",
				"Authentication",
				new
				{
					providerCode = code
				});
		}

		private IActionResult RedirectToGame(string gameType, Guid gameUid, Guid ruleUid, string externalClient)
		{
			var controllerName = gameType == GameTypesEnum.Sweeps.ToString()
				? "Sweepstakes"
				: "Pick3Game";

			var actionName = "PrePlay";

			return RedirectToActionPermanent(actionName, controllerName, new { gameUid, ruleUid, externalClient });
		}

		[HttpGet("Games/Sweeps/{gameUid}")]
		public async Task<IActionResult> Sweeps(
			Guid gameUid,
			[FromQuery] string externalClient,
			[FromQuery] Guid ruleUid)
		{
			var campaignUidString = _sessionManager.GetValue(SessionConstants.CampaignUid);
			var accountUidString = _sessionManager.GetValue(SessionConstants.AccountUid);

			if (string.IsNullOrEmpty(campaignUidString) || string.IsNullOrEmpty(accountUidString))
			{
				_logger.LogWarning("CampaignUid or AccountUid is null or empty for the game uid {gameUid} with externalClient {externalClient} for rule {ruleUid}",
					gameUid,
					externalClient,
					ruleUid);

				return RedirectToAction(nameof(ErrorController.HandleErrorCode), new { statusCode = 400 });
			}

			var campaignUid = Guid.Parse(campaignUidString);
			var accountUid = Guid.Parse(_sessionManager.GetValue(SessionConstants.AccountUid));

			if (campaignUid == Guid.Empty || accountUid == Guid.Empty)
			{
				_logger.LogWarning("CampaignUid or AccountUid is empty for the game uid {gameUid} with externalClient {externalClient} for rule {ruleUid}",
					gameUid,
					externalClient,
					ruleUid);

				return RedirectToAction(nameof(ErrorController.HandleErrorCode), new { statusCode = 400 });
			}

			using (_logger.SweepsGameLoggingEntry(ruleUid, gameUid, campaignUid, accountUid))
			{
				_logger.LogInformation("Received a request to play the sweepstakes game from the social {externalClient}",
				externalClient);

				if (!await _gameService.GameExistsForAccountAsync(accountUid, gameUid))
				{
					return RedirectToAction(nameof(GameDoesNotExist), new { gameUid, externalClient, ruleUid });
				}

				_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
				_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
				_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
				_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.Sweeps.ToString());

				var authKey = _sessionManager.GetValue(TemplateSessionConstants.AuthenticationKey);

				if (!string.IsNullOrWhiteSpace(authKey))
				{
					var userDetails = _twitterHttpService.GetAuthenticatedUserAsync(authKey);

					if (userDetails != null)
					{
						return RedirectToGame(GameTypesEnum.Sweeps.ToString(), gameUid, ruleUid, externalClient);
					}
				}

				var code = GetProviderCode(externalClient);
				_logger.LogInformation("Redirecting with provider code {code}", code);

				return RedirectToAction(
					"ExternalLogin",
					"Authentication",
					new
					{
						providerCode = code
					});
			}
		}

		//[Authorize]
		//[HttpGet("Games/Sweeps/{gameUid}/Rule/{ruleUid}/Play")]
		//public async Task<IActionResult> SweepsPlay(Guid gameUid, Guid ruleUid, [FromQuery] string externalClient)
		//{
		//	//retrieve user details to know who is logged in and display the name of the person to play
		//	var userName = User.Identities
		//		.FirstOrDefault()?
		//		.Claims?
		//		.FirstOrDefault(x => x.Type == HrvsterClaimTypes.DisplayName)?
		//		.Value;
		//	var externalProfileId = User.Identities
		//		.FirstOrDefault()?
		//		.Claims?
		//		.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?
		//		.Value;

		//	var gameType = _sessionManager.GetValue(TemplateSessionConstants.GameType);

		//	if (string.IsNullOrWhiteSpace(gameType))
		//		return View("BadGameType");

		//	var gameDetails = await _gameService.GetGameDetailsAsync(Enum.Parse<GameTypesEnum>(gameType), gameUid);

		//	if (DateTime.UtcNow > gameDetails.EndDate)
		//		return RedirectToAction("GameCompleted", new { gameUid, ruleUid, externalClient });
		//	if (DateTime.UtcNow < gameDetails.StartDate)
		//		return RedirectToAction("GameNotStarted", new { gameUid, ruleUid, externalClient });

		//	var accountUid = Guid.Parse(Request.HttpContext.Items[ContextConstants.AccountUid]?.ToString());

		//	var member = await _memberRepository.GetMemberViaExternalProfileIdAsync(accountUid, GetProfileType(externalClient), externalProfileId);

		//	var sweepGame = await _sweepStakeService.GetSweepstakeAsync(gameUid, default);

		//	var playGame = new PlaySweepsGame
		//	{
		//		GameUid = gameUid,
		//		RuleUid = ruleUid,
		//		ExternalClient = externalClient,
		//		NumberOfEntries = 1, //we can get this from the rules later on
		//		GameName = gameDetails.Name,
		//		GameStartDate = gameDetails.StartDate,
		//		GameEndDate = gameDetails.EndDate,
		//		ScreenName = member.Profiles.FirstOrDefault()?.Name,
		//		FirstName = member.FirstName,
		//		LastName = member.LastName
		//	};

		//	return View(playGame);
		//}

		//[Authorize]
		//[HttpPost("Games/Sweeps/{gameUid}/Rule/{ruleUid}/Play")]
		//public async Task<IActionResult> SweepsPlay(
		//	Guid gameUid,
		//	Guid ruleUid,
		//	[FromQuery] string externalClient,
		//	PlaySweepsGame form)
		//{
		//	var userName = User.Identities
		//		.FirstOrDefault()?
		//		.Claims?
		//		.FirstOrDefault(x => x.Type == HrvsterClaimTypes.DisplayName)?
		//		.Value;
		//	var externalProfileId = User.Identities
		//		.FirstOrDefault()?
		//		.Claims?
		//		.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?
		//		.Value;

		//	var gameType = _sessionManager.GetValue(TemplateSessionConstants.GameType);

		//	if (string.IsNullOrWhiteSpace(gameType))
		//		return View("BadGameType");

		//	var gameDetails = await _gameService.GetGameDetailsAsync(Enum.Parse<GameTypesEnum>(gameType), gameUid);

		//	if (DateTime.UtcNow > gameDetails.EndDate || DateTime.UtcNow < gameDetails.StartDate)
		//		return View("BadGame");

		//	var accountUid = Guid.Parse(Request.HttpContext.Items[ContextConstants.AccountUid]?.ToString());

		//	var member = await _memberRepository.GetMemberViaExternalProfileIdAsync(accountUid, GetProfileType(externalClient), externalProfileId);

		//	var sweepGame = await _sweepStakeService.GetSweepstakeAsync(gameUid, default);

		//	var entryUid = await _playSweepstakeService.EnterSweepstakeAsync(
		//		sweepGame,
		//		ruleUid,
		//		externalProfileId,
		//		userName,
		//		GetUserIdentifierType(externalClient),
		//		MessageConstants.SweepstakeTweetReply,
		//		externalClient,
		//		member.Profiles.FirstOrDefault()?.AccessToken,
		//		member.Profiles.FirstOrDefault()?.RefreshToken);

		//	if (entryUid != Guid.Empty)
		//		return RedirectToActionPermanent("SweepEntered", new { entryUid = entryUid });

		//	return RedirectToActionPermanent("SweepEntryFailed", new { gameUid, ruleUid, externalClient });
		//}

		//[Authorize]
		//[HttpGet("Games/Sweeps/SweepEntered/{entryUid}")]
		//public async Task<IActionResult> SweepEntered(Guid entryUid)
		//{
		//	var entry = await _sweepStakeService.GetSweepstakeEntryAsync(entryUid);

		//	if (entry == null) return RedirectToAction("BadSweepEntry");

		//	var rules = await _sweepStakeService.GetSweepRulesAsync(entry.Sweep.SweepUid);
		//	var rule = rules.FirstOrDefault(r => r.RuleUid == entry.Rule.RuleUid);
		//	entry.Rule = rule;

		//	return View(entry);
		//}

		//[Authorize]
		//[HttpGet("Games/Sweeps/SweepEntryFailed/{gameUid}/Rule/{ruleUid}")]
		//public async Task<IActionResult> SweepEntryFailed(Guid gameUid, Guid ruleUid, string externalClient)
		//{
		//	return new OkObjectResult(new { gameUid, ruleUid, externalClient });
		//}

		//private List<Guid> LoadTileStrings(string tileUids)
		//{
		//	var tiles = new List<Guid>();

		//	var tileList = tileUids.Split(',').ToList();

		//	foreach (var tileUid in tileList)
		//	{
		//		tiles.Add(Guid.Parse(tileUid));
		//	}

		//	return tiles;
		//}

		//private List<Models.MatchTile> GetMatchTiles(IEnumerable<Games.Match.Models.MatchTile> tiles)
		//{
		//	var matchTiles = new List<Models.MatchTile>();

		//	foreach (var tile in tiles)
		//	{
		//		matchTiles.Add(new Models.MatchTile
		//		{
		//			Uid = tile.Uid,
		//			ImageUrl = tile.Image.LocationUrl
		//		});
		//	}

		//	return matchTiles;
		//}

		//private async Task<IActionResult> ReturnWinOrLossAsync(
		//	Guid gameUid,
		//	Guid gameplayUid,
		//	Guid ruleUid,
		//	PlayMatchGame form,
		//	string externalClient,
		//	CancellationToken cancellationToken)
		//{
		//	var userClaims = User.Identities.FirstOrDefault()?.Claims;
		//	var externalProfileId = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?.Value;
		//	var screenName = userClaims?.FirstOrDefault(x => x.Type == HrvsterClaimTypes.DisplayName)?.Value;

		//	var accountUid = Guid.Parse(Request.HttpContext.Items[ContextConstants.AccountUid]?.ToString());

		//	var member = await _memberRepository.GetMemberViaExternalProfileIdAsync(accountUid, GetProfileType(externalClient), externalProfileId);
		//	var rule = await _matchGameRulesRepository.GetAsync(ruleUid);
		//	var game = await _matchGameService.GetMatchGameAsync(gameUid, cancellationToken);
		//	var campaign = await _campaignService.GetCampaignAsync(game.CampaignUid, cancellationToken);
		//	var account = await _accountsService.GetAccountByUidAsync(campaign.Account.AccountUid);
		//	var profiles = await _profilesRepository.GetProfileAsync(account.AccountUid, GetActualProfileType(externalClient));

		//	var hashtag = rule.WatchValue;
		//	var replyTo = $"@{profiles.ScreenName}";
		//	var message = string.Empty;
		//	var accessToken = member.Profiles.FirstOrDefault().AccessToken;
		//	var refreshToken = member.Profiles.FirstOrDefault().RefreshToken;

		//	if (await _matchGamePlayService.IsAWinAsync(gameUid: gameUid, gameplayUid, form.SelectedTiles, cancellationToken))
		//	{
		//		await SetGameplayAsWonAsync(gameplayUid, form, cancellationToken);
		//		var prize = await _matchGameService.GetPrizeFromGameplayUidAsync(gameplayUid, cancellationToken);

		//		message = $"{replyTo} I just played the Pick 3 game {game.Name} and won. I just won a prize with a value of ${prize.Value}. {Environment.NewLine} {hashtag}";
		//		await _twitterHttpService.PublishTweetInResponseToAsync(message, rule.ExternalId, accessToken, refreshToken);

		//		//Send email to owner
		//		await _communicationService.NotifyAccountOfWin(account.EmailAddress, game.Uid, game.Name, screenName, prize.Name);

		//		await _communicationService.NotifyMemberOfWin(member.EmailAddress, game.Name, gameplayUid, prize.Name, prize.Value);

		//		return RedirectToAction("MatchGameWin", new { Uid = gameUid, gameplayUid = gameplayUid });
		//	}

		//	await SetGameplayAsLossAsync(gameplayUid, form, cancellationToken);

		//	message = $"{replyTo} I just played the Pick 3 game {game.Name} and lost unfortunately ;). {Environment.NewLine} {hashtag}";
		//	await _twitterHttpService.PublishTweetInResponseToAsync(message, rule.ExternalId, accessToken, refreshToken);

		//	return RedirectToAction("MatchGameLoss", new { Uid = gameUid, gameplayUid = gameplayUid });
		//}

		//private async Task SetGameplayAsWonAsync(Guid gameplayUid, PlayMatchGame form, CancellationToken cancellationToken)
		//{
		//	_logger.LogInformation("The gameplay with uid {gameplayUid} was won with the tiles {selectedTiles}",
		//		gameplayUid,
		//		form.SelectedTiles);

		//	_logger.LogInformation("Setting the gameplay as a win.");

		//	await _matchGamePlayService.MarkAsPlayedAndWinAsync(gameplayUid, form.SelectedTiles, cancellationToken);
		//}

		//private async Task SetGameplayAsLossAsync(Guid gameplayUid, PlayMatchGame form, CancellationToken cancellationToken)
		//{
		//	_logger.LogInformation("The gameplay with uid {gameplayUid} was lost with the tiles {selectedTiles}",
		//		gameplayUid,
		//		form.SelectedTiles);

		//	_logger.LogInformation("Setting the gameplay as a loss.");

		//	await _matchGamePlayService.MarkAsPlayedAndLostAsync(gameplayUid, form.SelectedTiles, cancellationToken);
		//}

		//private List<Guid> LoadTiles(IEnumerable<Games.Match.Models.MatchTile> tiles)
		//{
		//	var tileList = new List<Guid>();

		//	foreach (var tile in tiles)
		//	{
		//		tileList.Add(tile.Uid);
		//	}

		//	return tileList;
		//}

		//private async Task<bool> CanPlayMatchGame(
		//	Guid gameUid,
		//	Guid campaignUid,
		//	string externalProfileId,
		//	string screenName,
		//	CancellationToken cancellationToken)
		//{
		//	var report = await _matchGamePlayReportingRepository.GetGamePlayReportAsync(
		//		gameUid,
		//		campaignUid,
		//		UserIdentifierType.TwitterHandle.ToString(),
		//		externalProfileId,
		//		screenName,
		//		cancellationToken);

		//	if (report == null) return true;

		//	var playRules = await _matchPlayRulesRepository.GetAsync(gameUid, cancellationToken);

		//	if (playRules == null)
		//	{
		//		_logger.LogWarning("No play rules have been configured for the game with uid {gameUid}", gameUid);

		//		return false;
		//	}

		//	//Todo: Put this back
		//	//return report.Plays < playRules.PlayPerGame;
		//	return true;
		//}

		//private UserIdentifierType GetUserIdentifierType(string externalClient)
		//{
		//	switch (Enum.Parse<ExternalClientType>(externalClient))
		//	{
		//		case ExternalClientType.Twitter:
		//			return UserIdentifierType.TwitterHandle;
		//		case ExternalClientType.Facebook:
		//			return UserIdentifierType.FacebookHandle;
		//	}

		//	return UserIdentifierType.TwitterHandle;
		//}

		//private string GetProfileType(string externalClient)
		//{
		//	return Enum.Parse<ExternalClientType>(externalClient) switch
		//	{
		//		ExternalClientType.Twitter => ProfileType.Twitter.ToString(),
		//		ExternalClientType.Facebook => ProfileType.Facebook.ToString(),
		//		_ => ProfileType.Twitter.ToString(),
		//	};
		//}

		//private ProfileType GetActualProfileType(string externalClient)
		//{
		//	switch (Enum.Parse<ExternalClientType>(externalClient))
		//	{
		//		case ExternalClientType.Twitter:
		//			return ProfileType.Twitter;
		//		case ExternalClientType.Facebook:
		//			return ProfileType.Facebook;
		//		default:
		//			return ProfileType.Twitter;
		//	}
		//}

		private string GetProviderCode(string externalClient)
			=> Enum.Parse<ExternalClientType>(externalClient) switch
			{
				ExternalClientType.Twitter => hrvster.Models.Constants.TwitterProvider,
				ExternalClientType.Facebook => hrvster.Models.Constants.FaceBookProvider,
				ExternalClientType.Instagram => hrvster.Models.Constants.InstagramProvider,
				_ => hrvster.Models.Constants.TwitterProvider,
			};

		//private async Task SignInUserAsync(UserProviderDetails userDetails)
		//{
		//	var principal = BuildPrincipalFromClaims(userDetails);

		//	await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
		//}

		//private async Task UpdateMemberProfileAsync(UserProviderDetails userDetails, MemberProfile memberProfile)
		//{
		//	_logger.LogInformation("Authenticated member already exists. We will try to update as much as possible.");

		//	memberProfile.AccessToken = userDetails.AccessToken;
		//	memberProfile.RefreshToken = userDetails.RefreshToken;
		//	memberProfile.Name = userDetails.Name;
		//	memberProfile.ScreenName = userDetails.ScreenName;
		//	memberProfile.PictureUrl = userDetails.ProfileImage;

		//	await _memberProfileRepository.UpdateMemberProfileAsync(memberProfile);

		//	_logger.LogWarning("Member profile found for external profile id {externalProfileId}", userDetails.ExternalProfileId);
		//}

		//private static ClaimsPrincipal BuildPrincipalFromClaims(UserProviderDetails userDetails)
		//{
		//	var claims = new List<Claim>()
		//	{
		//		new Claim(ClaimTypes.NameIdentifier, userDetails.ExternalProfileId),
		//		new Claim(ClaimTypes.Name, userDetails.Name),
		//		new Claim(HrvsterClaimTypes.DisplayName, userDetails.ScreenName),
		//		new Claim(HrvsterClaimTypes.AccessToken, userDetails.AccessToken),
		//		new Claim(HrvsterClaimTypes.RefreshToken, userDetails.RefreshToken),
		//		new Claim(HrvsterClaimTypes.Description, userDetails.Description),
		//		new Claim(HrvsterClaimTypes.ExternalProfileId, userDetails.ExternalProfileId)
		//	};

		//	var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
		//	var principal = new ClaimsPrincipal(identity);

		//	return principal;
		//}
	}
}
