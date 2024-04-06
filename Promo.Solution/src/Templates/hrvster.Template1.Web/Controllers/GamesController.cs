using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.Extensions.LogExtensions;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Models;
using hrvster.Template1.Web.Models.Query;
using hrvster.Templates.Core.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using hrvster.Models.Options;
using System.Web;
using hrvster.Models.Extensions;
using hrvster.ApplicationServices.Accounts;
using hrvster.Templates.Infrastructure.Customizations;

namespace hrvster.Template1.Web.Controllers
{
	public class GamesController : Controller
	{
		private readonly ILogger _logger;
		private readonly IGameService _gameService;
		private readonly ISessionManager _sessionManager;
		private readonly ITwitterHttpService _twitterHttpService;
		private readonly ICampaignService _campaignService;
		private readonly HarvesterSettingsOptions _harvesterSettingOptions;
		private readonly IClientMemberService _clientMemberService;
		private readonly IMemberProfileRepository _memberProfileRepository;

		public GamesController(
			ILoggerFactory loggerFactory,
			IGameService gameService,
			ISessionManager sessionManager,
			ITwitterHttpService twitterHttpService,
			ICampaignService campaignService,
			HarvesterSettingsOptions harvesterSettingOptions,
			IClientMemberService clientMemberService,
			IMemberProfileRepository memberProfileRepository)
		{
			_logger = loggerFactory.CreateLogger<GamesController>();
			_gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
			_sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
			_twitterHttpService = twitterHttpService ?? throw new ArgumentNullException(nameof(twitterHttpService));
			_campaignService = campaignService ?? throw new ArgumentNullException(nameof(campaignService));
			_harvesterSettingOptions = harvesterSettingOptions ?? throw new ArgumentNullException(nameof(harvesterSettingOptions));
			_clientMemberService = clientMemberService ?? throw new ArgumentNullException(nameof(clientMemberService));
			_memberProfileRepository = memberProfileRepository ?? throw new ArgumentNullException(nameof(memberProfileRepository));
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

			return RedirectToAction("HandleErrorCode", "Error", new { statusCode = 404 });
		}


		[HttpGet("Games/Pick3MatchGame/{gameUid}")]
		public async Task<IActionResult> Pick3MatchGame(
			Guid gameUid,
			[FromQuery] string externalClient,
			[FromQuery] Guid ruleUid,
			[FromQuery] string handshakeKey,
			CancellationToken cancellationToken)
		{
			_logger.LogInformation("Got a request to play the game for a pick 3 game with uid {gameUid}", gameUid);

			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
			_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.MatchGames.ToString());

			var game = await _gameService.GetGameDetailsAsync(GameTypesEnum.MatchGames, gameUid);
			if (game == null)
			{
				_logger.LogError("Game not found!");

				return RedirectToAction("HandlerErrorCode", "Error", new { statusCode = 400});
			}

			var campaign = await _campaignService.GetCampaignAsync(game.CampaignUid, default);
			if (campaign == null)
			{
				_logger.LogError("Campaign not found!");

				return RedirectToAction("HandlerErrorCode", StatusCodes.Status400BadRequest);
			}

			_sessionManager.SetKeyValue(SessionConstants.CampaignUid, campaign.CampaignUid.ToString());
			_sessionManager.SetKeyValue(SessionConstants.AccountUid, campaign.Account.AccountUid.ToString());

			using (_logger.Pick3MatchGameLoggingEntry(ruleUid, gameUid, campaign.CampaignUid, campaign.Account.AccountUid))
			{
				_logger.LogInformation("Received a request to play the pick 3 match game from the social {externalClient}",
				externalClient);

				if (!await _gameService.GameExistsForAccountAsync(campaign.Account.AccountUid, gameUid))
				{
					_logger.LogError("Game does not exist for the account.");

					return RedirectToAction(nameof(GameDoesNotExist), new { gameUid, externalClient, ruleUid });
				}

				_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
				_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
				_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
				_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.MatchGames.ToString());
			}

			if(externalClient == ExternalClientType.Twitter.ToString())
			{
				var authKey = _sessionManager.GetValue(TemplateSessionConstants.AuthenticationKey);

				if (!string.IsNullOrWhiteSpace(authKey))
				{
					UserProviderDetails? userDetails;

					var memberProfile = await _memberProfileRepository.GetByAccessTokenAsync(authKey);
					if(memberProfile == null)
					{
						userDetails = await _twitterHttpService.GetAuthenticatedUserAsync(authKey);

						if (userDetails != null)
						{
							_logger.LogInformation("Redirecting to game controller.");

							await SignInUserAsync(userDetails, ExternalClientType.Twitter);

							return RedirectToGame(
								GameTypesEnum.MatchGames.ToString(),
								gameUid,
								ruleUid,
								externalClient,
								campaign.CampaignUid,
								handshakeKey);
						}
					}
					else
					{
						//AccessToken = accessTokenObject.AccessToken,
						//RefreshToken = accessTokenObject.RefreshToken,
						//Description = twitterUserResponse.Profile.Description,
						//ExternalProfileId = twitterUserResponse.Profile.Id,
						//Name = twitterUserResponse.Profile.Name,
						//ScreenName = twitterUserResponse.Profile.Username,
						//ProfileImage = twitterUserResponse.Profile.ProfileImageUrl,
						//Location = twitterUserResponse.Profile.Location

						await SignInUserAsync(new UserProviderDetails
						{
							AccessToken = authKey,
							RefreshToken = memberProfile.RefreshToken,
							AccessTokenSecret = memberProfile.AccessToken,
							Description = memberProfile.Description,
							BearerToken = memberProfile.AccessToken,
							ExternalProfileId = memberProfile.ExternalProfileId,
							ScreenName = memberProfile.ScreenName,
							Name = memberProfile.Name
						}, ExternalClientType.Twitter);

						return RedirectToGame(
								GameTypesEnum.MatchGames.ToString(),
								gameUid,
								ruleUid,
								externalClient,
								campaign.CampaignUid,
								handshakeKey);
					}
				}

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
						gameType = GameTypesEnum.MatchGames.ToString(),
						campaign.CampaignUid,
						campaign.Account.AccountUid
					});
			}

			//External Client 
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

			await CreateClientMemberAsync(userQuery, campaign.Account, cancellationToken);

			_logger.LogInformation("Signing in the user automatically.");

			await SignInUserAsync(userDetailsForClient, ExternalClientType.Client);

			return await HandleGameAndUserProfile(
				GameTypesEnum.MatchGames.ToString(),
				gameUid,
				ruleUid,
				externalClient,
				campaign.CampaignUid, 
				handshakeKey);
		}

		private async Task CreateClientMemberAsync(UserQuery userQuery, Account account, CancellationToken cancellationToken)
		{
			await _clientMemberService.CreateAsync(userQuery.UserId, userQuery.UserName, account, cancellationToken);
		}

		private IActionResult RedirectToGame(string gameType, Guid gameUid, Guid ruleUid, string externalClient, Guid campaignUid, string handshakeKey)
		{
			var controllerName = gameType == GameTypesEnum.Sweeps.ToString()
				? "Sweepstakes"
				: "Pick3Game";

			var actionName = "PrePlay";

			return RedirectToActionPermanent(
				actionName, 
				controllerName, 
				new 
				{ 
					gameUid, 
					ruleUid, 
					externalClient, 
					campaignUid, 
					handshakeKey 
				});
		}

		[HttpGet("Games/Sweeps/{gameUid}")]
		public async Task<IActionResult> Sweeps(
			Guid gameUid,
			[FromQuery] string externalClient,
			[FromQuery] Guid ruleUid,
			[FromQuery] string handshakeKey)
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
			_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.Sweeps.ToString());

			var game = await _gameService.GetGameDetailsAsync(GameTypesEnum.Sweeps, gameUid);
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

			_sessionManager.SetKeyValue(SessionConstants.CampaignUid, campaign.CampaignUid.ToString());
			_sessionManager.SetKeyValue(SessionConstants.AccountUid, campaign.Account.AccountUid.ToString());

			using (_logger.SweepsGameLoggingEntry(ruleUid, gameUid, campaign.CampaignUid, campaign.Account.AccountUid))
			{
				_logger.LogInformation("Received a request to play the sweepstakes game from the social {externalClient}",
				externalClient);

				if (!await _gameService.GameExistsForAccountAsync(campaign.Account.AccountUid, gameUid))
				{
					return RedirectToAction(nameof(GameDoesNotExist), new { gameUid, externalClient, ruleUid });
				}

				_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
				_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
				_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
				_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.Sweeps.ToString());

				if (externalClient == ExternalClientType.Twitter.ToString())
				{
					var authKey = _sessionManager.GetValue(TemplateSessionConstants.AuthenticationKey);

					if (!string.IsNullOrWhiteSpace(authKey))
					{
						var userDetails = _twitterHttpService.GetAuthenticatedUserAsync(authKey);

						if (userDetails != null)
						{
							return RedirectToGame(GameTypesEnum.Sweeps.ToString(), gameUid, ruleUid, externalClient, campaign.CampaignUid, handshakeKey);
						}
					}

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

				//External Client 
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

				return await HandleGameAndUserProfile(
					GameTypesEnum.Sweeps.ToString(), 
					gameUid, 
					ruleUid, 
					externalClient, 
					campaign.CampaignUid,
					handshakeKey);
			}
		}

		private async Task<IActionResult> HandleGameAndUserProfile(
			string gameType,
			Guid gameUid,
			Guid ruleUid,
			string externalClient,
			Guid campaignUid,
			string handshakeKey)
		{
			var gameDetails = await _gameService.GetGameDetailsAsync(Enum.Parse<GameTypesEnum>(gameType), gameUid);

			if (DateTime.UtcNow > gameDetails.EndDateUtc)
			{
				return RedirectToAction("HandlerErrorCode", 
					new 
					{ 
						reason = "Game has ended.", 
						statusCode = StatusCodes.Status400BadRequest 
					});
			}

			if (DateTime.UtcNow < gameDetails.StartDateUtc)
			{
				return RedirectToAction("HandlerErrorCode",
					new
					{
						reason = "Game has not started.",
						statusCode = StatusCodes.Status400BadRequest
					});
			}

			//Redirect directly to play the game if from client.
			return RedirectToGame(gameType, gameUid, ruleUid, externalClient, campaignUid, handshakeKey);
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
				_ => hrvster.Models.Constants.TwitterProvider,
			};
	}
}
