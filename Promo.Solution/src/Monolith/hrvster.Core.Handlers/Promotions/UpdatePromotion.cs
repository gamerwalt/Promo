using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests.Promotions;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Promotions
{
	public class UpdatePromotion : AsyncRequestHandler<UpdatePromotionRequest>
	{
		private readonly ILogger<UpdatePromotion> _logger;
		private readonly ICampaignRepository _repository;
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly ISessionManager _sessionManager;
		private readonly IToastNotification _notificationService;

		private HrvsterUser _hrvsterUser;

		public UpdatePromotion(
			ILoggerFactory loggerFactory,
			ICampaignRepository repository,
			IAccountSubscriptionService accountSubscriptionService,
			ISessionManager sessionManager,
			IToastNotification notificationService)
		{
			_logger = loggerFactory.CreateLogger<UpdatePromotion>();
			_repository = repository;
			_accountSubscriptionService = accountSubscriptionService;
			_sessionManager = sessionManager;
			_notificationService = notificationService;
		}

		protected override async Task Handle(UpdatePromotionRequest request, CancellationToken cancellationToken)
		{
			var campaign = await _repository.GetCampaignByUidAsync(request.CampaignUid);
			_hrvsterUser = _sessionManager.GetUserInformation();

			_logger.LogInformation("Handling {className} for Request {request}",
				nameof(UpdatePromotion),
				request.ToJsonString());

			var accountSubscription = await _accountSubscriptionService.GetAccountSubscriptionAsync(_hrvsterUser.UserUid);

			if (accountSubscription == null)
			{
				_logger.LogWarning("Account with uid {accountUid} is not subscribed.", _hrvsterUser.UserUid);

				return;
			}

			var campaigns = await _repository.GetAccountCampaignsAsync(_hrvsterUser.UserUid);
			var activeCampaigns = campaigns
				.Where(c => c.ActiveInd && c.IsActive() || c.WillBeInProgress())
				.ToList();
			if(!activeCampaigns.Any(c => c.CampaignUid == campaign.CampaignUid))
			{
				if (accountSubscription.Plan.PromotionsPerMonth != 0)
				{
					if (activeCampaigns.Count >= accountSubscription.Plan.PromotionsPerMonth)
					{
						_logger.LogWarning("Account with uid {accountUid} has a total of {activeCampaignCount} active campaigns and their current max is {maxPromotions}",
							_hrvsterUser.UserUid,
							activeCampaigns.Count,
							accountSubscription.Plan.PromotionsPerMonth);

						_notificationService.AddErrorToastMessage($"You cannot update this promotion to be active.");
						_notificationService.AddInfoToastMessage($"You can only have {accountSubscription.Plan.PromotionsPerMonth} active promotions per month.");

						return;
					}
				}
			}			

			await _repository.UpdateCampaignAsync(request.CampaignUid, request.CampaignName,
				request.Description, request.CampaignTypeUid, request.StartDate, request.EndDate, request.TimeZone, request.IsDaylight);
		}
	}
}
