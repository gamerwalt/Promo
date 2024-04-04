using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Requests.MatchGames;
using MediatR;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.MatchGame
{
	public class CreateMatchGame : IRequestHandler<CreateMatchGameRequest, bool>
	{
		private readonly ILogger<CreateMatchGame> _logger;
		private readonly IMatchGamesService _service;
		private readonly ICampaignService _campaignService;
		private readonly IToastNotification _notificationService;

		public CreateMatchGame(
			ILoggerFactory loggerFactory,
			IMatchGamesService service,
			ICampaignService campaignService,
			IToastNotification notificationService)
		{
			_logger = loggerFactory.CreateLogger<CreateMatchGame>();
			_service = service;
			_campaignService = campaignService;
			_notificationService = notificationService;
		}

		public async Task<bool> Handle(CreateMatchGameRequest request, CancellationToken cancellationToken)
		{
			if (! await ValidateCreateMatchGameRequestName(request, cancellationToken))
			{
				_notificationService.AddErrorToastMessage("The match game name you supplied already exists.");

				return false;
			}

			var campaign = await _campaignService.GetCampaignAsync(request.PromotionUid, cancellationToken);

			var matchGame = new Games.Match.Models.MatchGame
			{
				CampaignUid = request.PromotionUid,
				Name = request.Name,
				Description = request.Description,
				CampaignId = campaign.CampaignId,
				NumberOfBoards = request.NumberOfBoards,
				NumberOfWinnableBoards = request.WinnableBoards,
				TileCountPerBoard = 9,
				Uid = request.MatchGameUid,
				WinProbability = request.WinProbability,
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				GenerateBoardDate = request.GenerateBoardDate,
				AccumulatePointsOnLoss = request.AccumulatePointsOnLoss,
				PointsPerLoss = request.PointsPerLoss
			};

			_logger.LogInformation("Creating match game {matchGame} for account {accountUid}",
				matchGame,
				request.AccountUid);

			await _service.CreateMatchGameAsync(matchGame, request.TimeZone, request.IsDayLight, cancellationToken);

			_notificationService.AddSuccessToastMessage("Match game created successfully.");

			return true;
		}

		private async Task<bool> ValidateCreateMatchGameRequestName(
			CreateMatchGameRequest request, CancellationToken cancellationToken)
		{
			var matchGames = await _service.GetCampaignMatchGamesAsync(request.PromotionUid, cancellationToken);

			return !matchGames.Any(c => c.Name == request.Name);
		}
	}
}
