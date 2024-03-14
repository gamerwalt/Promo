using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Extensions.LogExtensions;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Models;
using hrvster.Templates.Core.Common;
using Microsoft.AspNetCore.Mvc;

namespace hrvster.Template1.Web.Controllers
{
	public class GamesController : Controller
	{
		private readonly ILogger _logger;
		private readonly IGameService _gameService;
		private readonly ISessionManager _sessionManager;
		private readonly ITwitterHttpService _twitterHttpService;

		public GamesController(
			ILoggerFactory loggerFactory,
			IGameService gameService,
			ISessionManager sessionManager,
			ITwitterHttpService twitterHttpService)
		{
			_logger = loggerFactory.CreateLogger<GamesController>();
			_gameService = gameService;
			_sessionManager = sessionManager;
			_twitterHttpService = twitterHttpService;
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
			[FromQuery] Guid ruleUid)
		{
			_logger.LogInformation("Got a request to play the game for a pick 3 game with uid {gameUid}", gameUid);

			_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
			_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.MatchGames.ToString());

			var campaignUidString = _sessionManager.GetValue(SessionConstants.CampaignUid);
			var accountUidString = _sessionManager.GetValue(SessionConstants.AccountUid);

			if (string.IsNullOrEmpty(campaignUidString) || string.IsNullOrEmpty(accountUidString))
			{
				_logger.LogError("CampaignUid or AccountUid is null or empty for the game uid {gameUid} with externalClient {externalClient} for rule {ruleUid}",
					gameUid,
					externalClient,
					ruleUid);

				return RedirectToAction("HandleErrorCode", "Error", new { statusCode = 400 });
			}

			var campaignUid = Guid.Parse(campaignUidString);
			var accountUid = Guid.Parse(_sessionManager.GetValue(SessionConstants.AccountUid));

			if (campaignUid == Guid.Empty || accountUid == Guid.Empty)
			{
				_logger.LogError("CampaignUid or AccountUid is empty for the game uid {gameUid} with externalClient {externalClient} for rule {ruleUid}",
					gameUid,
					externalClient,
					ruleUid);

				return RedirectToAction("HandleErrorCode", "Error", new { statusCode = 400 });
			}

			_sessionManager.SetKeyValue(SessionConstants.CampaignUid, campaignUidString);
			_sessionManager.SetKeyValue(SessionConstants.AccountUid, accountUidString);

			using (_logger.Pick3MatchGameLoggingEntry(ruleUid, gameUid, campaignUid, accountUid))
			{
				_logger.LogInformation("Received a request to play the pick 3 match game from the social {externalClient}",
				externalClient);

				if (!await _gameService.GameExistsForAccountAsync(accountUid, gameUid))
				{
					_logger.LogError("Game does not exist for the account.");

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
					_logger.LogInformation("Redirecting to game controller.");

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
			_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
			_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.Sweeps.ToString());

			var campaignUidString = _sessionManager.GetValue(SessionConstants.CampaignUid);
			var accountUidString = _sessionManager.GetValue(SessionConstants.AccountUid);

			if (string.IsNullOrEmpty(campaignUidString) || string.IsNullOrEmpty(accountUidString))
			{
				_logger.LogWarning("CampaignUid or AccountUid is null or empty for the game uid {gameUid} with externalClient {externalClient} for rule {ruleUid}",
					gameUid,
					externalClient,
					ruleUid);

				return RedirectToAction("HandleErrorCode", "Error", new { statusCode = 400 });
			}

			var campaignUid = Guid.Parse(campaignUidString);
			var accountUid = Guid.Parse(_sessionManager.GetValue(SessionConstants.AccountUid));

			if (campaignUid == Guid.Empty || accountUid == Guid.Empty)
			{
				_logger.LogWarning("CampaignUid or AccountUid is empty for the game uid {gameUid} with externalClient {externalClient} for rule {ruleUid}",
					gameUid,
					externalClient,
					ruleUid);

				return RedirectToAction("HandleErrorCode", "Error", new { statusCode = 400 });
			}

			_sessionManager.SetKeyValue(SessionConstants.CampaignUid, campaignUidString);
			_sessionManager.SetKeyValue(SessionConstants.AccountUid, accountUidString);

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
