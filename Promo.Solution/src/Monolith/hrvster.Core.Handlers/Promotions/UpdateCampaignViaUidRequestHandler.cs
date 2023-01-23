using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.Notifications;
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
	public class UpdateCampaignViaUidRequestHandler : IRequestHandler<UpdateCampaignViaUidRequest, Campaign>
	{
		private readonly ILogger<UpdateCampaignViaUidRequestHandler> _logger;
		private readonly ICampaignRepository _repository;
		private readonly IMediator _mediator;
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly IToastNotification _notificationService;

		public UpdateCampaignViaUidRequestHandler(
			ILoggerFactory loggerFactory,
			ICampaignRepository repository,
			IMediator mediator,
			IAccountSubscriptionService accountSubscriptionService,
			IToastNotification notificationService)
		{
			_logger = loggerFactory.CreateLogger<UpdateCampaignViaUidRequestHandler>();
			_repository = repository;
			_mediator = mediator;
			_accountSubscriptionService = accountSubscriptionService;
			_notificationService = notificationService;
		}

		public async Task<Campaign> Handle(UpdateCampaignViaUidRequest request, CancellationToken cancellationToken)
		{
			var campaign = await _repository.GetCampaignByUidAsync(request.CampaignUid);

			var accountSubscription = await _accountSubscriptionService.GetAccountSubscriptionAsync(request.AccountUid);

			if (accountSubscription == null)
			{
				_logger.LogWarning("Account with uid {accountUid} is not subscribed.", request.AccountUid);

				_notificationService.AddErrorToastMessage("You need to be subscribed to continue.");

				return campaign;
			}

			var campaigns = await _repository.GetAccountCampaignsAsync(request.AccountUid);
			var activeCampaigns = campaigns
				.Where(c => c.ActiveInd && c.IsActive() || c.WillBeInProgress())
				.ToList();
			if(!activeCampaigns.Any(c => c.CampaignUid == request.CampaignUid))
			{
				if (accountSubscription.Plan.PromotionsPerMonth != 0)
				{
					if (activeCampaigns.Count >= accountSubscription.Plan.PromotionsPerMonth)
					{
						_logger.LogWarning("Account with uid {accountUid} has a total of {activeCampaignCount} active campaigns and their current max is {maxPromotions}",
							request.AccountUid,
							activeCampaigns.Count,
							accountSubscription.Plan.PromotionsPerMonth);

						_notificationService.AddErrorToastMessage($"You cannot update this promotion to be active.");
						_notificationService.AddInfoToastMessage($"You can only have {accountSubscription.Plan.PromotionsPerMonth} active promotions per month.");

						return campaign;
					}
				}
			}			

			await _repository.UpdateCampaignAsync(request.CampaignUid, new Campaign
			{
				CampaignName = request.Request.CampaignName,
				Description = request.Request.Description,
				StartDate = request.Request.StartDate,
				EndDate = request.Request.EndDate,
				ActiveInd = request.Request.ActiveInd,
				CampaignType = new CampaignType
				{
					CampaignTypeUid = request.Request.CampaignTypeUid
				}
			}, request.Timezone, request.IsDaylight);

			campaign = await _repository.GetCampaignByUidAsync(request.CampaignUid, request.AccountUid);

			await _mediator.Publish(new CampaignUpdated(request.CampaignUid, request.AccountUid));

			return campaign;
		}
	}
}
