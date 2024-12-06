using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.SpinToWin;
using hrvster.Core.Requests.SpinToWinGame;
using hrvster.Models.Extensions;
using hrvster.Models.SpinToWin;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace hrvster.Core.Handlers.SpinToWin
{
	public class CreateSpinToWinGame : IRequestHandler<CreateSpinToWinGameRequest, bool>
	{
		private readonly ILogger<CreateSpinToWinGame> _logger;
		private readonly ICampaignService _campaignService;
		private readonly ISpinToWinService _service;

		public CreateSpinToWinGame(
			ILoggerFactory loggerFactory, 
			ICampaignService campaignService,
			ISpinToWinService service) 
		{ 
			_logger = loggerFactory.CreateLogger<CreateSpinToWinGame>();
			_campaignService = campaignService;
			_service = service;
		}

		public async Task<bool> Handle(CreateSpinToWinGameRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Received a request to create a spin to win game. Details: {Details}", request.ToJsonString());

			var campaign = await _campaignService.GetCampaignAsync(request.CampaignUid, cancellationToken);

			var spinToWinGame = new SpinToWinGame
			{
				Uid = request.GameUid,
				Name = request.Name,
				Description = request.Description,
				Campaign = new Models.Campaign
				{
					CampaignUid = campaign.CampaignUid,
					CampaignId = campaign.CampaignId
				},
				MaximumSpins = request.NumberOfSpins,
				MaximumWinsForPhysicalPrizes = request.MaxWinForPhysicalPrizes,
				MaximumWinsForPointPrizes = request.MaxWinForPoints,
				PointsForFreePlay = request.PointsForFreePlay,
				StartDate = request.StartDate,
				StartDateUTC = TimeZoneInfo.ConvertTimeToUtc(request.StartDate),
				EndDate = request.EndDate,
				EndDateUTC = TimeZoneInfo.ConvertTimeToUtc(request.EndDate),
				PieGenerateDate = request.GeneratePieDate,
				PieGenerateDateUTC = TimeZoneInfo.ConvertTimeToUtc(request.GeneratePieDate)
			};

			await _service.CreateSpinToWinGameAsync(spinToWinGame, cancellationToken);

			return true;
		}
	}
}
