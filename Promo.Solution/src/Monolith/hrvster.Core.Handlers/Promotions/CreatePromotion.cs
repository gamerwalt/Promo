using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.WebRequests.Promotions;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Promotions
{
	public class CreatePromotion : AsyncRequestHandler<CreatePromotionRequest>
	{
		private readonly ILogger<CreatePromotion> _logger;
		private readonly ICampaignRepository _repository;
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly IMediator _mediator;
		private readonly IToastNotification _notificationService;
		private readonly HarvesterSettingsOptions _hrvsterSettings;

		public CreatePromotion(
			ILoggerFactory loggerFactory,
			ICampaignRepository repository,
			IAccountSubscriptionService accountSubscriptionService,
			IMediator mediator,
			IToastNotification notificationService,
			HarvesterSettingsOptions harvesterSettings)
		{
			_logger = loggerFactory.CreateLogger<CreatePromotion>();
			_repository = repository;
			_accountSubscriptionService = accountSubscriptionService;
			_mediator = mediator;
			_notificationService = notificationService;
			_hrvsterSettings = harvesterSettings;
		}

		protected override async Task Handle(CreatePromotionRequest request, CancellationToken cancellationToken)
		{
			var accountSubscription = await _accountSubscriptionService.GetAccountSubscriptionAsync(request.AccountUid);

			if (accountSubscription == null)
			{
				_logger.LogWarning("Account with uid {accountUid} is not subscribed.", request.AccountUid);

				return;
			}

			var campaigns = await _repository.GetAccountCampaignsAsync(request.AccountUid);
			var activeCampaigns = campaigns.Where(c => c.ActiveInd && c.IsActive() || c.WillBeInProgress()).ToList();

			if (!accountSubscription.IsTrial)
			{
				if (accountSubscription.Plan.PromotionsPerMonth != 0)
				{
					if (activeCampaigns.Count >= accountSubscription.Plan.PromotionsPerMonth)
					{
						_logger.LogWarning("Account with uid {accountUid} has a total of {activeCampaignCount} active campaigns and their current max is {maxPromotions}",
							request.AccountUid,
							activeCampaigns.Count,
							accountSubscription.Plan.PromotionsPerMonth);

						_notificationService.AddErrorToastMessage($"You cannot create more promotions.");
						_notificationService.AddInfoToastMessage("You can only have {accountSubscription.Plan.PromotionsPerMonth} active promotions per month.");

						return;
					}
				}
			}
			else if (accountSubscription.IsTrial)
			{
				if (activeCampaigns.Count >= _hrvsterSettings.TrialPromotions)
				{
					_logger.LogWarning("Account with uid {accountUid} has a total of {activeCampaignCount} active campaigns and their current max is {maxPromotions}",
						request.AccountUid,
						activeCampaigns.Count,
						_hrvsterSettings.TrialPromotions);
					_notificationService.AddErrorToastMessage($"You cannot create more promotions.");
					_notificationService.AddInfoToastMessage($"You can only have {_hrvsterSettings.TrialPromotions} active promotions while on Trial mode.");
					return;
				}
			}

			var model = CreateCampaignModel(request);

			await _repository.CreateCampaignAsync(model, request.TimeZone, request.IsDaylight);

			await _mediator.Publish(new PromotionCreated(model));
		}

		private static Campaign CreateCampaignModel(CreatePromotionRequest request)
		{
			return new Campaign
			{
				CampaignUid = request.PromotionUid,
				ActiveInd = true,
				CampaignName = request.Name,
				Description = request.Description,
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				Account = new Models.Account
				{
					AccountUid = request.AccountUid,
				},
				CampaignType = new Models.CampaignType
				{
					CampaignTypeUid = request.CampaignTypeUid
				}
			};
		}
	}
}
