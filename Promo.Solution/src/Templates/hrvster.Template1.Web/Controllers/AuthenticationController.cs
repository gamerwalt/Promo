using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Common;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Core.Clients.Publishers;
using hrvster.Core.Clients.Twitter.Models;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Options;
using hrvster.Template1.Web.Models.Query;
using hrvster.Templates.Core.Common;
using hrvster.Templates.Infrastructure.Customizations;
using hrvster.Templates.Models.Extensions;
using hrvster.Templates.Models.Members;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web;

namespace hrvster.Template1.Web.Controllers;
public class AuthenticationController : Controller
{
	private const string Message = "Received a request to validate a handshake. Details: HandshakeKey: {handshakeKey} - GameUid: {gameUid} - RuleUid: {ruleUid} - ExternalClient: {externalClient} - GameType: {gameType}";
	private readonly ILogger<AuthenticationController> _logger;
	private readonly ISessionManager _sessionManager;
	private readonly ITwitterHttpService _twitterHttpService;
	private readonly IGameService _gameService;
	private readonly IMemberProfileRepository _memberProfileRepository;
	private readonly HarvesterSettingsOptions _harvesterSettingOptions;
	private readonly ICampaignService _campaignService;

	public AuthenticationController(
		ILogger<AuthenticationController> logger,
		ISessionManager sessionManager,
		ITwitterHttpService twitterHttpService,
		IGameService gameService,
		IMemberProfileRepository memberProfileRepository,
		HarvesterSettingsOptions harvesterSettingsOptions,
		ICampaignService campaignService)
	{
		_logger = logger;
		_sessionManager = sessionManager;
		_twitterHttpService = twitterHttpService;
		_gameService = gameService;
		_memberProfileRepository = memberProfileRepository;
		_harvesterSettingOptions = harvesterSettingsOptions;
		_campaignService = campaignService;
	}

	[HttpGet]
	[Route("Authentication/login/external/{providerCode}")]
	public async Task<IActionResult> ExternalLogin(string providerCode, string gameUid, string ruleUid, string externalClient, string gameType)
	{
		_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid);
		_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid);
		_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
		_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, gameType);

		return new RedirectResult(
				_twitterHttpService.GetAuthorizationUrl(
					Request.Scheme,
					Request.Host.Value,
					$"/Authentication/Validate{providerCode}Auth")
				);
	}

	public async Task<IActionResult> ValidateTwitterAuth()
	{
		if (IsRequestDenied())
		{
			return ErrorRedirect("TwitterAuthenticationFailed", StatusCodes.Status401Unauthorized);
		}

		if (!IsValidSessionData(out var gameUid, out var ruleUid, out var externalClient, out var gameType))
		{
			_logger.LogError("Unable to determine either the gameuid, ruleuid, externalclient or gameType");
			return ErrorRedirect("Missing required information.", StatusCodes.Status404NotFound);
		}

		var accessTokenObject = await FetchAccessToken();
		if (accessTokenObject == null)
		{
			return ErrorRedirect("TwitterAuthenticationFailed", StatusCodes.Status401Unauthorized);
		}

		var userDetails = await FetchAuthenticatedUserDetails(accessTokenObject);
		if (userDetails == null)
		{
			return ErrorRedirect("TwitterAuthenticationFailed", StatusCodes.Status401Unauthorized);
		}

		await SignInUserAsync(userDetails, ExternalClientType.Twitter);

		return await HandleGameAndUserProfile(userDetails, gameType, gameUid, ruleUid, externalClient);
	}

	//[HttpGet]
	//[Route("Authentication/login/external/client/handshake/{handshakeKey}/game/{gameUid}/rule/{ruleUid}/externalclient/{externalClient}/GameType/{gameType}")]
	//public async Task<IActionResult> ValidateAccount(string handshakeKey, Guid gameUid, Guid ruleUid, string externalClient, string gameType)
	//{
	//	try
	//	{
	//		_logger.LogInformation(Message, handshakeKey, gameUid, ruleUid, externalClient, gameType);

	//		if (string.IsNullOrWhiteSpace(handshakeKey))
	//		{
	//			_logger.LogWarning("Handshake key is not valid.");

	//			return ErrorRedirect("ClientAuthenticationFailed", StatusCodes.Status401Unauthorized);
	//		}

	//		_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
	//		_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
	//		_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
	//		_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.MatchGames.ToString());

	//		var handshakeKeyDecoded = HttpUtility.UrlDecode(handshakeKey);

	//		var decryptedData = CryptoHelper.DecryptHandshakeKey(
	//			handshakeKeyDecoded,
	//			_harvesterSettingOptions.EncryptionKey,
	//			_harvesterSettingOptions.EncryptionIV);

	//		var userQuery = UserQuery.Parse(decryptedData);

	//		var userDetails = new UserProviderDetails
	//		{
	//			AccessToken = userQuery.UserId,
	//			AccessTokenSecret = userQuery.UserName,
	//			ExternalProfileId = userQuery.UserId,
	//			Name = userQuery.UserName,
	//			ScreenName = userQuery.UserName,
	//			Description = userQuery.UserId,
	//			RefreshToken = Guid.NewGuid().ToString()
	//		};

	//		_logger.LogInformation("Signing in the user automatically.");

	//		await SignInUserAsync(userDetails, ExternalClientType.Client);

	//		return await HandleGameAndUserProfile(userDetails, gameType, gameUid, ruleUid, externalClient);
	//	}
	//	catch (Exception ex)
	//	{
	//		_logger.LogError("An error occurred while trying to validate a handshake. Message: {message}", ex.Message);

	//		return ErrorRedirect("Unknown", 500);
	//	}
		
	//}

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
				new Claim(HrvsterClaimTypes.RefreshToken, userDetails.RefreshToken),
				new Claim(HrvsterClaimTypes.Description, userDetails.Description),
				new Claim(HrvsterClaimTypes.ExternalProfileId, userDetails.ExternalProfileId),
				new Claim(HrvsterClaimTypes.ExternalClient, externalClient.ToString())
			};

		var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

		var principal = new ClaimsPrincipal(identity);

		return principal;
	}

	private bool IsRequestDenied() => Request.Query["denied"].Count > 0;

	private bool IsValidSessionData(out Guid gameUid, out Guid ruleUid, out string externalClient, out string gameType)
	{
		gameUid = Guid.Parse(_sessionManager.GetValue(TemplateSessionConstants.GameUid));
		ruleUid = Guid.Parse(_sessionManager.GetValue(TemplateSessionConstants.RuleUid));
		externalClient = _sessionManager.GetValue(TemplateSessionConstants.ExternalClient);
		gameType = _sessionManager.GetValue(TemplateSessionConstants.GameType);

		return gameUid != Guid.Empty &&
			   ruleUid != Guid.Empty &&
			   !string.IsNullOrWhiteSpace(externalClient) &&
			   !string.IsNullOrWhiteSpace(gameType);
	}

	private async Task<AccessTokenResponse> FetchAccessToken()
	{
		var code = Request.Query["code"].ToString();
		return await _twitterHttpService.GetAccessTokenAsyncForOfflineAccess(
			code, Request.Scheme, Request.Host.Value, "/Authentication/ValidateTWITTERAuth");
	}

	private async Task<UserProviderDetails> FetchAuthenticatedUserDetails(AccessTokenResponse accessTokenObject)
	{
		_sessionManager.SetKeyValue(TemplateSessionConstants.AuthenticationKey, accessTokenObject.AccessToken);
		_sessionManager.SetKeyValue(TemplateSessionConstants.RefreshKey, accessTokenObject.RefreshToken);

		return await _twitterHttpService.GetAuthenticatedUserAsync(accessTokenObject);
	}

	private async Task<IActionResult> HandleGameAndUserProfile(
		UserProviderDetails userDetails,
		string gameType,
		Guid gameUid,
		Guid ruleUid,
		string externalClient)
	{
		var gameDetails = await _gameService.GetGameDetailsAsync(Enum.Parse<GameTypesEnum>(gameType), gameUid);

		if (DateTime.UtcNow > gameDetails.EndDateUtc)
		{
			return ErrorRedirect("Game has ended.", StatusCodes.Status400BadRequest);
		}

		if (DateTime.UtcNow < gameDetails.StartDateUtc)
		{
			return ErrorRedirect("Game has not started.", StatusCodes.Status400BadRequest);
		}

		var accountUidString = _sessionManager.GetValue(SessionConstants.AccountUid);
		if (string.IsNullOrWhiteSpace(accountUidString))
		{
			var game = await _gameService.GetGameDetailsAsync(Enum.Parse<GameTypesEnum>(gameType), gameUid);
			if (game == null)
			{
				_logger.LogError("Game not found!");

				return RedirectToAction("HandlerErrorCode", "Error", new { statusCode = 400 });
			}

			var campaign = await _campaignService.GetCampaignAsync(game.CampaignUid, default);
			if (campaign == null)
			{
				_logger.LogError("Campaign not found!");

				return RedirectToAction("HandlerErrorCode", StatusCodes.Status400BadRequest);
			}

			accountUidString = campaign.Account.AccountUid.ToString();
		}

		var accountUid = Guid.Parse(accountUidString);
		if (accountUid == Guid.Empty)
		{
			return ErrorRedirect("AccountUid is missing.", StatusCodes.Status400BadRequest);
		}

		//Redirect directly to play the game if from client.
		if(externalClient != ExternalClientType.Twitter.ToString())
		{
			return RedirectToGame(gameType, gameUid, ruleUid, externalClient);
		}

		var memberProfile = await _memberProfileRepository.GetByExternalProfileIdAsync(
			accountUid,
			userDetails.ExternalProfileId);

		if (memberProfile == null)
		{
			LogMissingMemberProfile(userDetails);

			_sessionManager.SetKeyValue(TemplateSessionConstants.UserDetails, userDetails.ToJsonString());

			var memberCreationQuery = new MemberCreationQuery(gameUid, ruleUid, externalClient, gameType);

			return RedirectToAction(
				"CreateMember",
				"Members",
				memberCreationQuery);
		}

		await UpdateMemberProfileAsync(userDetails, memberProfile);

		return RedirectToGame(gameType, gameUid, ruleUid, externalClient);
	}

	private IActionResult RedirectToGame(string gameType, Guid gameUid, Guid ruleUid, string externalClient)
	{
		var controllerName = gameType == GameTypesEnum.Sweeps.ToString()
			? "Sweepstakes"
			: "Pick3Game";

		var actionName = "PrePlay";

		return RedirectToAction(actionName, controllerName, new { gameUid, ruleUid, externalClient });
	}

	private void LogMissingMemberProfile(UserProviderDetails userDetails)
	{
		_logger.LogWarning("Member profile does not exist for the external profile id {externalProfileId}", userDetails.ExternalProfileId);

		_logger.LogInformation("Redirecting to create a member for this authenticated user.");
	}

	private IActionResult ErrorRedirect(string reason, int statusCode)
	{
		return RedirectToActionPermanent(
			"HandleErrorCode",
			"Error",
			new { reason, statusCode });
	}

	private async Task UpdateMemberProfileAsync(UserProviderDetails userDetails, MemberProfile memberProfile)
	{
		memberProfile.UpdateFromUserDetails(userDetails);

		await _memberProfileRepository.UpdateMemberProfileAsync(memberProfile);

		_logger.LogInformation("Updated member profile for external profile id {externalProfileId}", userDetails.ExternalProfileId);
	}
}
