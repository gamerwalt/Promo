using Azure.Storage.Blobs.Models;
using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.Extensions.LogExtensions;
using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
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
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;

namespace hrvster.Template1.Web.Controllers;

[Route("Sweeps")]
public class SweepstakesController : Controller
{
	private readonly ILogger<SweepstakesController> _logger;
	private readonly IGameService _gameService;
	private readonly ISessionManager _sessionManager;
	private readonly IMemberRepository _memberRepository;
	private readonly ISweepstakesService _sweepStakeService;
	private readonly IPlaySweepstakeService _playSweepstakeService;
	private readonly IAccountsService _accountsService;
	private readonly ICampaignService _campaignService;
	private readonly HarvesterSettingsOptions _harvesterSettingOptions;

	public SweepstakesController(
		ILoggerFactory loggerFactory,
		IGameService gameService,
		ISessionManager sessionManager,
		IMemberRepository memberRepository,
		ISweepstakesService sweepStakesService,
		IPlaySweepstakeService playSweepstakeService,
		IAccountsService accountsService,
		ICampaignService campaignService,
		HarvesterSettingsOptions harvesterSettingOptions)
	{
		_logger = loggerFactory.CreateLogger<SweepstakesController>();
		_gameService = gameService;
		_sessionManager = sessionManager;
		_memberRepository = memberRepository;
		_sweepStakeService = sweepStakesService;
		_playSweepstakeService = playSweepstakeService;
		_accountsService = accountsService;
		_campaignService = campaignService;
		_harvesterSettingOptions = harvesterSettingOptions;
	}

	[HttpGet("routes")]
	public IActionResult ListRoutes()
	{
		var routes = new StringBuilder();

		var endpoints = HttpContext.RequestServices.GetRequiredService<EndpointDataSource>();

		foreach (var endpoint in endpoints.Endpoints)
		{
			if (endpoint is RouteEndpoint routeEndpoint)
			{
				var routeInfo = $"{routeEndpoint.RoutePattern.RawText}";
				routes.AppendLine(routeInfo);
			}
		}

		return Content(routes.ToString(), "text/plain");
	}


	[HttpGet("PrePlay")]
	public async Task<IActionResult> PrePlay(
		[FromQuery] Guid gameUid,
		[FromQuery] Guid ruleUid,
		[FromQuery] string externalClient,
		[FromQuery] Guid campaignUid,
		[FromQuery] string handshakeKey)
	{
		_logger.LogInformation("Received a request to enter into the sweepstake game with uid {GameUid}", gameUid);

		using (_logger.BeginPreplayForSweepsGameScope(gameUid, ruleUid, externalClient))
		{
			var sweepGame = await _sweepStakeService.GetSweepstakeAsync(gameUid, default);
			if(sweepGame == null)
			{
				_logger.LogWarning("Unable to locate sweepstakes game.");

				return ErrorRedirect("Unable to locate sweepstakes game.", StatusCodes.Status404NotFound);
			}

			if (DateTime.UtcNow > sweepGame.EndDateUtc)
			{
				_logger.LogWarning("Game has ended.");

				return ErrorRedirect("Game has ended.", StatusCodes.Status400BadRequest);
			}

			if (DateTime.UtcNow < sweepGame.StartDateUtc)
			{
				_logger.LogWarning("Game has not started.");

				return ErrorRedirect("Game has not started.", StatusCodes.Status400BadRequest);
			}

			var campaign = await _campaignService.GetCampaignAsync(campaignUid, default);
			if (campaign == null)
			{
				_logger.LogError("Campaign not found!");

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
							campaignUid,
							campaign.Account.AccountUid
						});
				}
				else
				{
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

			var rules = await _gameService.GetRulesAsync(GameTypesEnum.Sweeps, gameUid, default);
			if (rules == null)
			{
				_logger.LogWarning("No rules have been configured for the game with uid {gameUid}", gameUid);

				return ErrorRedirect("No rules have been configured for the game!", StatusCodes.Status400BadRequest);
			}

			var rule = rules.FirstOrDefault(r => r.RuleUid == ruleUid);

			if (rule.MustBeFollowing)
			{
				/*
				var followers = await _twitterHttpService.GetFollowersAsync(externalProfileId);

							if (!followers.Any(c => c.Id == profile.ExternalProfileId))*/

				//return RedirectToAction("CannotPlayMatchGame", new { gameUid, ruleUid, externalClient });
			}

			return RedirectToAction("Play", new
			{
				gameUid,
				ruleUid,
				campaign.Account.AccountUid,
				externalClient,
				campaignUid,
				handshakeKey
			});
		}
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
				new Claim(HrvsterClaimTypes.RefreshToken, userDetails.RefreshToken),
				new Claim(HrvsterClaimTypes.Description, userDetails.Description),
				new Claim(HrvsterClaimTypes.ExternalProfileId, userDetails.ExternalProfileId),
				new Claim(HrvsterClaimTypes.ExternalClient, externalClient.ToString())
			};

		var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

		var principal = new ClaimsPrincipal(identity);

		return principal;
	}

	private string GetProviderCode(string externalClient)
			=> Enum.Parse<ExternalClientType>(externalClient) switch
			{
				ExternalClientType.Twitter => hrvster.Models.Constants.TwitterProvider,
				ExternalClientType.Facebook => hrvster.Models.Constants.FaceBookProvider,
				ExternalClientType.Instagram => hrvster.Models.Constants.InstagramProvider,
				ExternalClientType.Client => hrvster.Models.Constants.ClientProvider,
				_ => hrvster.Models.Constants.TwitterProvider,
			};

	[HttpGet("Play")]
	public async Task<IActionResult> Play(
		[FromQuery] Guid gameUid, 
		[FromQuery] Guid ruleUid, 
		[FromQuery] Guid accountUid, 
		[FromQuery] string externalClient,
		[FromQuery] Guid campaignUid,
		[FromQuery] string handshakeKey)
	{
		using (_logger.BeginPlayForSweepsGameScope(gameUid, ruleUid, externalClient))
		{
			var gameDetails = await _gameService.GetGameDetailsAsync(GameTypesEnum.Sweeps, gameUid);
			if (gameDetails == null)
			{
				_logger.LogWarning("Unable to locate game.");

				return ErrorRedirect("Unable to locate game.", StatusCodes.Status404NotFound);
			}

			var sweepGame = await _sweepStakeService.GetSweepstakeAsync(gameUid, default);
			if (sweepGame == null)
			{
				_logger.LogWarning("Unable to locate sweepstakes game.");

				return ErrorRedirect("Unable to locate sweepstakes game.", StatusCodes.Status404NotFound);
			}

			if (DateTime.UtcNow > gameDetails.EndDateUtc)
			{
				_logger.LogWarning("Game has ended.");

				return ErrorRedirect("Game has ended.", StatusCodes.Status400BadRequest);
			}

			if (DateTime.UtcNow < gameDetails.StartDateUtc)
			{
				_logger.LogWarning("Game has not started.");

				return ErrorRedirect("Game has not started.", StatusCodes.Status400BadRequest);
			}

			if (!User.Identity.IsAuthenticated)
			{
				if (externalClient == ExternalClientType.Twitter.ToString())
				{
					_logger.LogWarning("User is not authenticated.");
					var campaign = await _campaignService.GetCampaignAsync(campaignUid, default);

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

			var rules = await _gameService.GetRulesAsync(GameTypesEnum.Sweeps, gameUid, default);
			if (rules == null)
			{
				_logger.LogWarning("No rules have been configured for the game with uid {gameUid}", gameUid);

				return ErrorRedirect("No rules have been configured for the game!", StatusCodes.Status400BadRequest);
			}

			var rule = rules.FirstOrDefault(r => r.RuleUid == ruleUid);

			if (rule.MustBeFollowing)
			{
				/*
				var followers = await _twitterHttpService.GetFollowersAsync(externalProfileId);

							if (!followers.Any(c => c.Id == profile.ExternalProfileId))*/

				//return RedirectToAction("CannotPlayMatchGame", new { gameUid, ruleUid, externalClient });
			}

			var userName = User.Identities
				.FirstOrDefault()?
				.Claims?
				.FirstOrDefault(x => x.Type == HrvsterClaimTypes.DisplayName)?
				.Value;
			if (string.IsNullOrWhiteSpace(userName))
			{
				_logger.LogWarning("Unable to find user name.");

				return ErrorRedirect("Unable to find user name.", StatusCodes.Status400BadRequest);
			}

			var externalProfileId = User.Identities
				.FirstOrDefault()?
				.Claims?
				.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?
				.Value;

			if (string.IsNullOrWhiteSpace(externalProfileId))
			{
				_logger.LogWarning("Unable to find external profile id for this user.");

				return ErrorRedirect("Unable to find external profile id for this user.", StatusCodes.Status400BadRequest);
			}

			if(externalClient == ExternalClientType.Twitter.ToString())
			{
				var member = await _memberRepository.GetMemberViaExternalProfileIdAsync(accountUid, GetProfileType(externalClient), externalProfileId);
				if (member == null)
				{
					_logger.LogWarning("Unable to find member details.");

					var memberCreationQuery = new MemberCreationQuery(gameUid, ruleUid, externalClient, GameTypesEnum.Sweeps.ToString());

					return RedirectToAction(
						"CreateMember",
						"Members",
						memberCreationQuery);
				}

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
					LastName = member.LastName,
					AccountUid = accountUid,
					CampaignUid = campaignUid,
					HandshakeKey = handshakeKey
				};

				return View(playGame);
			}
			else
			{
				var playGame = new PlaySweepsGame
				{
					GameUid = gameUid,
					RuleUid = ruleUid,
					ExternalClient = externalClient,
					NumberOfEntries = 1, //we can get this from the rules later on
					GameName = gameDetails.Name,
					GameStartDate = gameDetails.StartDate,
					GameEndDate = gameDetails.EndDate,
					ScreenName = userName,
					FirstName = externalProfileId,
					LastName = externalProfileId,
					AccountUid = accountUid,
					CampaignUid = campaignUid,
					HandshakeKey = handshakeKey
				};

				return View(playGame);
			}
		}
	}

	[HttpPost("Play")]
	public async Task<IActionResult> Play(
		[FromQuery] Guid gameUid,
		[FromQuery] Guid ruleUid,
		[FromQuery] Guid accountUid,
		[FromQuery] string externalClient,
		[FromQuery] Guid campaignUid,
		[FromQuery] string? handshakeKey,
		PlaySweepsGame form)
	{
		using (_logger.BeginPostPlayForSweepsGameScope(gameUid, ruleUid, externalClient))
		{
			var gameDetails = await _gameService.GetGameDetailsAsync(GameTypesEnum.Sweeps, gameUid);
			if (gameDetails == null)
			{
				_logger.LogWarning("Unable to locate game.");

				return ErrorRedirect("Unable to locate game.", StatusCodes.Status404NotFound);
			}

			var sweepGame = await _sweepStakeService.GetSweepstakeAsync(gameUid, default);
			if (sweepGame == null)
			{
				_logger.LogWarning("Unable to locate sweepstakes game.");

				return ErrorRedirect("Unable to locate sweepstakes game.", StatusCodes.Status404NotFound);
			}

			_sessionManager.SetKeyValue(SessionConstants.AccountUid, sweepGame.Account.AccountUid.ToString());

			if (DateTime.UtcNow > gameDetails.EndDateUtc)
			{
				_logger.LogWarning("Game has ended.");

				return ErrorRedirect("Game has ended.", StatusCodes.Status400BadRequest);
			}

			if (DateTime.UtcNow < gameDetails.StartDateUtc)
			{
				_logger.LogWarning("Game has not started.");

				return ErrorRedirect("Game has not started.", StatusCodes.Status400BadRequest);
			}

			if (!User.Identity.IsAuthenticated)
			{
				if (externalClient == ExternalClientType.Twitter.ToString())
				{
					_logger.LogWarning("User is not authenticated.");
					var campaign = await _campaignService.GetCampaignAsync(campaignUid, default);

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

			var userName = User.Identities
				.FirstOrDefault()?
				.Claims?
				.FirstOrDefault(x => x.Type == HrvsterClaimTypes.DisplayName)?
				.Value;
			if (string.IsNullOrWhiteSpace(userName))
			{
				_logger.LogWarning("Unable to find user name.");

				return ErrorRedirect("Unable to find user name.", StatusCodes.Status400BadRequest);
			}

			var externalProfileId = User.Identities
				.FirstOrDefault()?
				.Claims?
				.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?
				.Value;

			if (string.IsNullOrWhiteSpace(externalProfileId))
			{
				_logger.LogWarning("Unable to find external profile id for this user.");

				return ErrorRedirect("Unable to find external profile id for this user.", StatusCodes.Status400BadRequest);
			}

			if (externalClient == ExternalClientType.Twitter.ToString())
			{
				var member = await _memberRepository.GetMemberViaExternalProfileIdAsync(accountUid, GetProfileType(externalClient), externalProfileId);
				if (member == null)
				{
					_logger.LogWarning("Unable to find member details.");

					return ErrorRedirect("Unable to find member details.", StatusCodes.Status400BadRequest);
				}

				if (ModelState.IsValid)
				{
					var entryUid = await _playSweepstakeService.EnterSweepstakeAsync(
						sweepGame,
						ruleUid,
						externalProfileId,
						userName,
						GetUserIdentifierType(externalClient),
						MessageConstants.SweepstakeTweetReply,
						externalClient,
						member.Profiles.FirstOrDefault()?.AccessToken,
						member.Profiles.FirstOrDefault()?.RefreshToken);

					if (entryUid != Guid.Empty)
					{
						_logger.LogInformation("User has entered the sweepstakes game.");

						return RedirectToActionPermanent("SweepEntered", new { entryUid = entryUid, externalClient = externalClient });
					}

					_logger.LogWarning("Unable to enter sweepstakes game.");

					return RedirectToActionPermanent("SweepEntryFailed", new { gameUid, ruleUid, externalClient });
				}
			}
			else
			{
				if(ModelState.IsValid)
				{
					var entryUid = await _playSweepstakeService.EnterSweepstakeAsync(
						sweepGame,
						ruleUid,
						externalProfileId,
						userName,
						GetUserIdentifierType(externalClient),
						MessageConstants.SweepstakeTweetReply,
						externalClient,
						string.Empty,
						string.Empty);

					if (entryUid != Guid.Empty)
					{
						_logger.LogInformation("User has entered the sweepstakes game.");

						return RedirectToActionPermanent("SweepEntered", new { entryUid = entryUid, externalClient = externalClient });
					}

					_logger.LogWarning("Unable to enter sweepstakes game.");

					return RedirectToActionPermanent("SweepEntryFailed", new { gameUid, ruleUid, externalClient });
				}
			}

			return RedirectToActionPermanent("SweepEntryFailed", new { gameUid, ruleUid, externalClient });
		}
	}

	[Authorize]
	[HttpGet("SweepEntered/Entry/{entryUid:Guid}/ExternalClient/{externalClient}")]
	public async Task<IActionResult> SweepEntered(Guid entryUid, string externalClient)
	{
		var entry = await _sweepStakeService.GetSweepstakeEntryAsync(entryUid);

		if (entry == null)
		{
			return RedirectToAction("BadSweepEntry");
		}

		var rules = await _sweepStakeService.GetSweepRulesAsync(entry.Sweep.SweepUid);
		var rule = rules.FirstOrDefault(r => r.RuleUid == entry.Rule.RuleUid);
		entry.Rule = rule;

		var accountUidString = _sessionManager.GetValue(SessionConstants.AccountUid);
		
		if (string.IsNullOrWhiteSpace(accountUidString))
		{
			var game = await _sweepStakeService.GetSweepstakeAsync(entry.Sweep.SweepUid, default);
			if (game == null)
			{
				_logger.LogWarning("Game not found!");

				return ErrorRedirect("Game not found!", StatusCodes.Status400BadRequest);
			}

			var campaign = await _campaignService.GetCampaignAsync(game.Campaign.CampaignUid, default);
			if (campaign == null)
			{
				_logger.LogWarning("Campaign not found!");

				return ErrorRedirect("Campaign not found!", StatusCodes.Status400BadRequest);
			}

			accountUidString = campaign.Account.AccountUid.ToString();
		}

		var accountProfile = await _accountsService.GetProfileAsync(Guid.Parse(accountUidString), ProfileType.Twitter);

		if(accountProfile == null)
		{
			_logger.LogWarning("Unable to find account profile.");

			return ErrorRedirect("Unable to find account profile.", StatusCodes.Status400BadRequest);
		}

		entry.ScreenName = $"@{accountProfile.ScreenName}";

		return View(entry);
	}

	[Authorize]
	[HttpGet("Games/Sweeps/SweepEntryFailed/{gameUid}/Rule/{ruleUid}")]
	public async Task<IActionResult> SweepEntryFailed(Guid gameUid, Guid ruleUid, [FromQuery] string externalClient)
	{
		_logger.LogWarning("Unable to enter sweepstakes game.");

		var gameDetails = await _gameService.GetGameDetailsAsync(GameTypesEnum.Sweeps, gameUid);
		if (gameDetails == null)
		{
			_logger.LogWarning("Unable to locate game.");

			return ErrorRedirect("Unable to locate game.", StatusCodes.Status404NotFound);
		}

		var sweepGame = await _sweepStakeService.GetSweepstakeAsync(gameUid, default);
		if (sweepGame == null)
		{
			_logger.LogWarning("Unable to locate sweepstakes game.");

			return ErrorRedirect("Unable to locate sweepstakes game.", StatusCodes.Status404NotFound);
		}

		if (DateTime.UtcNow > gameDetails.EndDateUtc)
		{
			_logger.LogWarning("Game has ended.");

			return ErrorRedirect("Game has ended.", StatusCodes.Status400BadRequest);
		}

		if (DateTime.UtcNow < gameDetails.StartDateUtc)
		{
			_logger.LogWarning("Game has not started.");

			return ErrorRedirect("Game has not started.", StatusCodes.Status400BadRequest);
		}

		return View(new SweepstakeGameDetails
		{
			Name = sweepGame.Name,
			Description = sweepGame.Description,
			StartDate = sweepGame.StartDateUtc,
			EndDate = sweepGame.EndDateUtc,
			DrawDate = sweepGame.DrawDateUtc
		});
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

	private string GetProfileType(string externalClient)
	{
		if (!externalClient.Equals(ExternalClientType.Twitter.ToString(), StringComparison.InvariantCultureIgnoreCase))
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

	private IActionResult ErrorRedirect(string reason, int statusCode)
	{
		_logger.LogError("Redirecting to the error handler with reason {reason}", reason);

		return RedirectToActionPermanent(
			"HandleErrorCode",
			"Error",
			new { reason, statusCode });
	}
}
