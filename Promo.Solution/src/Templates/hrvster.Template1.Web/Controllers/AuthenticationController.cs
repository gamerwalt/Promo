using hrvster.ApplicationServices;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Core.Clients.Twitter.Models;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Template1.Web.Models.Query;
using hrvster.Templates.Core.Common;
using hrvster.Templates.Infrastructure.Customizations;
using hrvster.Templates.Models.Extensions;
using hrvster.Templates.Models.Members;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrvster.Template1.Web.Controllers;
public class AuthenticationController : Controller
{
	private readonly ILogger<AuthenticationController> _logger;
	private readonly ISessionManager _sessionManager;
	private readonly ITwitterHttpService _twitterHttpService;
	private readonly IGameService _gameService;
	private readonly IMemberProfileRepository _memberProfileRepository;

	public AuthenticationController(
		ILogger<AuthenticationController> logger,
		ISessionManager sessionManager,
		ITwitterHttpService twitterHttpService,
		IGameService gameService,
		IMemberProfileRepository memberProfileRepository)
	{
		_logger = logger;
		_sessionManager = sessionManager;
		_twitterHttpService = twitterHttpService;
		_gameService = gameService;
		_memberProfileRepository = memberProfileRepository;
	}

	[HttpGet]
	[Route("Authentication/login/external/{providerCode}")]
	public async Task<IActionResult> ExternalLogin(string providerCode)
		=> new RedirectResult(
			_twitterHttpService.GetAuthorizationUrl(
				Request.Scheme,
				Request.Host.Value,
				$"/Authentication/Validate{providerCode}Auth")
			);

	public async Task<IActionResult> ValidateTwitterAuth()
	{
		
		if (IsRequestDenied())
		{
			return ErrorRedirect("TwitterAuthenticationFailed", StatusCodes.Status401Unauthorized);
		}

		if (!IsValidSessionData(out var gameUid, out var ruleUid, out var externalClient, out var gameType))
		{
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

		await SignInUserAsync(userDetails);

		return await HandleGameAndUserProfile(userDetails, gameType, gameUid, ruleUid, externalClient);
	}

	private async Task SignInUserAsync(UserProviderDetails userDetails)
	{
		var principal = BuildPrincipalFromClaims(userDetails);

		await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
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
			return ErrorRedirect("AccountUid is missing.", StatusCodes.Status400BadRequest);
		}

		var accountUid = Guid.Parse(accountUidString);
		if (accountUid == Guid.Empty)
		{
			return ErrorRedirect("AccountUid is missing.", StatusCodes.Status400BadRequest);
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
