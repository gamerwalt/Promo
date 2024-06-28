using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Common;
using hrvster.Core.Authentication;
using hrvster.Exceptions;
using hrvster.Models;
using hrvster.Templates.Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace hrvster.Middleware
{
	public class GameDetectingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<GameDetectingMiddleware> _logger;
		private readonly IGameService _gameService;
		private readonly ICampaignService _campaignService;
		private readonly ISessionManager _sessionManager;

		public GameDetectingMiddleware(
			RequestDelegate next,
			ILoggerFactory loggerFactory,
			IGameService gameService,
			ICampaignService campaignService,
			ISessionManager sessionManager)
		{
			_next = next;
			_logger = loggerFactory.CreateLogger<GameDetectingMiddleware>();
			_gameService = gameService;
			_campaignService = campaignService;
			_sessionManager = sessionManager;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				var pathSegments = context.Request.Path.Value.Split("/");

				if (IsErrorPage(pathSegments))
				{
					await _next(context);
					return;
				}

				if (IsSpinToWinRequest(pathSegments, context))
				{
					await _next(context);
				}

				if (IsGameRequest(pathSegments, context))
				{
					await HandleGameRequest(context, pathSegments);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				_logger.LogError(ex.StackTrace);
				throw;
			}
			finally
			{
				await _next(context);
			}
		}

		private bool IsSpinToWinRequest(string[] pathSegments, HttpContext context)
		{
			return pathSegments[1] == "SpinToWin";
		}

		private bool IsErrorPage(string[] pathSegments)
		{
			return pathSegments.Length > 0 && pathSegments[1] == "Error";
		}

		private bool IsGameRequest(string[] pathSegments, HttpContext context)
		{
			var existingGameUidContext = context.Items[ContextConstants.GameUid] as string;

			return pathSegments.Length > 3 && pathSegments[1] == "Games" && string.IsNullOrWhiteSpace(existingGameUidContext);
		}

		private async Task HandleGameRequest(HttpContext context, string[] pathSegments)
		{
			var gameUidString = pathSegments[3];

			if (string.IsNullOrWhiteSpace(gameUidString))
			{
				throw new ArgumentException("The Game uid does not exist.");
			}

			if (!Guid.TryParse(gameUidString, out Guid gameUid))
			{
				return;
			}

			var gameDetails = await _gameService.GetGameDetailsAsync(gameUidString);
			if (gameDetails == null)
			{
				throw new ArgumentException($"Game details for game uid {gameUidString} cannot be found.");
			}

			ValidateCampaign(gameDetails);

			var campaign = await _campaignService.GetCampaignAsync(gameDetails.CampaignUid, default);
			ValidateCampaignAndAccount(campaign, gameDetails.Uid);

			SetSessionValues(context, campaign, pathSegments);
		}

		private void ValidateCampaign(GameDetail gameDetails)
		{
			if (gameDetails.CampaignUid == Guid.Empty)
			{
				throw new MissingCampaignException($"Campaign uid is empty for game uid {gameDetails.Uid}. Exiting!");
			}
		}

		private void ValidateCampaignAndAccount(Campaign campaign, Guid gameUid)
		{
			if (campaign == null)
			{
				throw new MissingCampaignException($"Campaign for game uid {gameUid} cannot be found.");
			}

			if (campaign.Account == null)
			{
				throw new MissingAccountException($"Account for game uid {gameUid} cannot be found.");
			}
		}

		private void SetSessionValues(HttpContext context, Campaign campaign, string[] pathSegments)
		{
			_sessionManager.SetKeyValue(SessionConstants.CampaignUid, campaign.CampaignUid.ToString());
			_sessionManager.SetKeyValue(SessionConstants.GameUid, pathSegments[3]);
			_sessionManager.SetKeyValue(SessionConstants.AccountUid, campaign.Account.AccountUid.ToString());
			_sessionManager.SetKeyValue(SessionConstants.ExternalClient, context.Request.Query[ContextConstants.ExternalClient].ToString());
			_sessionManager.SetKeyValue(SessionConstants.RuleUid, context.Request.Query[ContextConstants.RuleUid].ToString());
		}
	}
}
