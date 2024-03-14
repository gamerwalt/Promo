using Azure.Storage.Blobs.Models;
using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Extensions.LogExtensions;
using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Core.Authentication;
using hrvster.Models;
using hrvster.Template1.Web.Models;
using hrvster.Template1.Web.Models.Query;
using hrvster.Templates.Core.Common;
using hrvster.Templates.Infrastructure.Customizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading;

namespace hrvster.Template1.Web.Controllers;

public class SweepstakesController : Controller
{
	private readonly ILogger<SweepstakesController> _logger;
	private readonly IGameService _gameService;
	private readonly ISessionManager _sessionManager;
	private readonly IMemberRepository _memberRepository;
	private readonly ISweepstakesService _sweepStakeService;
	private readonly IPlaySweepstakeService _playSweepstakeService;
	private readonly IAccountsService _accountsService;

	public SweepstakesController(
		ILoggerFactory loggerFactory,
		IGameService gameService,
		ISessionManager sessionManager,
		IMemberRepository memberRepository,
		ISweepstakesService sweepStakesService,
		IPlaySweepstakeService playSweepstakeService,
		IAccountsService accountsService)
	{
		_logger = loggerFactory.CreateLogger<SweepstakesController>();
		_gameService = gameService;
		_sessionManager = sessionManager;
		_memberRepository = memberRepository;
		_sweepStakeService = sweepStakesService;
		_playSweepstakeService = playSweepstakeService;
		_accountsService = accountsService;
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


	[HttpGet("Sweepstakes/PrePlay/{gameUid}/rule/{ruleUid}/externalClient/{externalClient}")]
	public async Task<IActionResult> PrePlay(
		Guid gameUid,
		Guid ruleUid,
		string externalClient)
	{
		_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
		_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
		_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
		_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.Sweeps.ToString());

		using (_logger.BeginPreplayForSweepsGameScope(gameUid, ruleUid, externalClient))
		{
			var sweepGame = await _sweepStakeService.GetSweepstakeAsync(gameUid, default);
			if(sweepGame == null)
			{
				_logger.LogWarning("Unable to locate sweepstakes game.");

				return ErrorRedirect("Unable to locate sweepstakes game.", StatusCodes.Status404NotFound);
			}
			
			_sessionManager.SetKeyValue(SessionConstants.AccountUid, sweepGame.Account.AccountUid.ToString());

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

			var accountUidString = _sessionManager.GetValue(SessionConstants.AccountUid);

			if(string.IsNullOrWhiteSpace(accountUidString))
			{
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

			var accountUid = Guid.Parse(accountUidString);
			if (string.IsNullOrWhiteSpace(accountUidString) || accountUid == Guid.Empty)
			{
				_logger.LogWarning("AccountUid is empty!");

				return ErrorRedirect("AccountUid is empty!", StatusCodes.Status400BadRequest);
			}

			if (!User.Identity.IsAuthenticated)
			{
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
				accountUid,
				externalClient
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

	[HttpGet("Sweepstakes/Play/Game/{gameUid:Guid}/Rule/{ruleUid:Guid}/Account/{accountUid}/ExternalClient/{externalClient}")]
	public async Task<IActionResult> Play(
		Guid gameUid, 
		Guid ruleUid, 
		Guid accountUid, 
		string externalClient)
	{
		_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
		_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
		_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
		_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.Sweeps.ToString());

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

			_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
			_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.Sweeps.ToString());

			if (!User.Identity.IsAuthenticated)
			{
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
				AccountUid = accountUid
			};

			return View(playGame);
		}
	}

	[HttpPost("Sweepstakes/Play/Game/{gameUid:Guid}/Rule/{ruleUid:Guid}/Account/{accountUid}/ExternalClient/{externalClient}")]
	public async Task<IActionResult> Play(
		Guid gameUid, 
		Guid ruleUid, 
		Guid accountUid, 
		string externalClient,
		PlaySweepsGame form)
	{
		_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
		_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
		_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
		_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.Sweeps.ToString());

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

			_sessionManager.SetKeyValue(TemplateSessionConstants.GameUid, gameUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.RuleUid, ruleUid.ToString());
			_sessionManager.SetKeyValue(TemplateSessionConstants.ExternalClient, externalClient);
			_sessionManager.SetKeyValue(TemplateSessionConstants.GameType, GameTypesEnum.Sweeps.ToString());

			if (!User.Identity.IsAuthenticated)
			{
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

			var member = await _memberRepository.GetMemberViaExternalProfileIdAsync(accountUid, GetProfileType(externalClient), externalProfileId);
			if (member == null)
			{
				_logger.LogWarning("Unable to find member details.");

				return ErrorRedirect("Unable to find member details.", StatusCodes.Status400BadRequest);
			}

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
					member.Profiles.FirstOrDefault()?.AccessToken,
					member.Profiles.FirstOrDefault()?.RefreshToken);

				if (entryUid != Guid.Empty)
				{
					_logger.LogInformation("User has entered the sweepstakes game.");

					return RedirectToActionPermanent("SweepEntered", new { entryUid = entryUid });
				}

				_logger.LogWarning("Unable to enter sweepstakes game.");

				return RedirectToActionPermanent("SweepEntryFailed", new { gameUid, ruleUid, externalClient });
			}

			return RedirectToActionPermanent("SweepEntryFailed", new { gameUid, ruleUid, externalClient });
		}
	}

	[Authorize]
	[HttpGet("Sweepstakes/Entered/{entryUid}")]
	public async Task<IActionResult> SweepEntered(Guid entryUid)
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
		var accountUid = Guid.Parse(accountUidString);
		if (string.IsNullOrWhiteSpace(accountUidString) || accountUid == Guid.Empty)
		{
			_logger.LogWarning("AccountUid is empty!");

			return ErrorRedirect("AccountUid is empty!", StatusCodes.Status400BadRequest);
		}

		var accountProfile = await _accountsService.GetProfileAsync(accountUid, ProfileType.Twitter);

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
		return Enum.Parse<ExternalClientType>(externalClient) switch
		{
			ExternalClientType.Twitter => ProfileType.Twitter.ToString(),
			ExternalClientType.Facebook => ProfileType.Facebook.ToString(),
			_ => ProfileType.Twitter.ToString(),
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
