using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.MatchGames;
using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Requests.MatchGames;
using hrvster.Exceptions;
using hrvster.Models.Extensions;
using hrvster.Models.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using NToastNotify;
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
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly HarvesterSettingsOptions _hrvsterSettings;

		public CreateMatchGame(
			ILoggerFactory loggerFactory,
			IMatchGamesService service,
			ICampaignService campaignService,
			IToastNotification notificationService,
			IAccountSubscriptionService accountSubscriptionService,
			HarvesterSettingsOptions harvesterSettings)
		{
			_logger = loggerFactory.CreateLogger<CreateMatchGame>();
			_service = service;
			_campaignService = campaignService;
			_notificationService = notificationService;
			_accountSubscriptionService = accountSubscriptionService;
			_hrvsterSettings = harvesterSettings;
		}

		public async Task<bool> Handle(CreateMatchGameRequest request, CancellationToken cancellationToken)
		{
			var accountSubscription = await _accountSubscriptionService.GetAccountSubscriptionAsync(request.AccountUid);
			if (accountSubscription == null)
			{
				_logger.LogWarning("Account with uid {accountUid} is not subscribed.", request.AccountUid);

				throw new AccountIsNotSubscribedException($"Account with uid {request.AccountUid} is not subscribed.");
			}

			var matchGames = await _service.GetAccountMatchGamesAsync(request.AccountUid, cancellationToken);
			var activeMatchGames = matchGames.Where(s => s.IsActive() || s.WillBeInProgress()).ToList();

			var isTrial = accountSubscription.IsTrial;

			if (isTrial)
			{
				if (activeMatchGames.Count >= _hrvsterSettings.TrialMatchGames)
				{
					_notificationService.AddErrorToastMessage("You have reached the maximum number of match games allowed for the trial.");

					return false;
				}

				if(request.NumberOfBoards > _hrvsterSettings.TrialMatchBoards)
				{
					_notificationService.AddErrorToastMessage($"You cannot create more than {_hrvsterSettings.TrialMatchBoards} boards.");

					return false;
				}
			}

			if (!isTrial)
			{
				if (accountSubscription.Plan.MatchGamesPerMonth != 0)
				{
					if (activeMatchGames.Count >= accountSubscription.Plan.MatchGamesPerMonth)
					{
						_notificationService.AddErrorToastMessage("You have reached the maximum number of match games allowed for your subscription plan.");
						return false;
					}
				}
			}

			if (!await ValidateCreateMatchGameRequestName(request, cancellationToken))
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
