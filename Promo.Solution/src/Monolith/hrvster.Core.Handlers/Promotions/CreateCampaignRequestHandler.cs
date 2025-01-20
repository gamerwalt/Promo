using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Requests;
using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using hrvster.Models.Extensions;
using NToastNotify;
using hrvster.Models.Options;

namespace hrvster.Core.Handlers.Promotions
{
	public class CreateCampaignRequestHandler : IRequestHandler<CreateCampaignRequest, Guid>
	{
		private readonly ILogger<CreateCampaignRequestHandler> _logger;
		private readonly ICampaignRepository _repository;
		private readonly IMediator _mediator;
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly IToastNotification _notificationService;
		private readonly HarvesterSettingsOptions _hrvsterSettings;

		public CreateCampaignRequestHandler(
			ILoggerFactory loggerFactory,
			ICampaignRepository repository, 
			IMediator mediator,
			IAccountSubscriptionService accountSubscriptionService,
			IToastNotification notificationService,
			HarvesterSettingsOptions hrvsterSettings)
		{
			_logger = loggerFactory.CreateLogger<CreateCampaignRequestHandler>();
			_repository = repository;
			_mediator = mediator;
			_accountSubscriptionService = accountSubscriptionService;
			_notificationService = notificationService;
			_hrvsterSettings = hrvsterSettings;
		}

		public async Task<Guid> Handle(CreateCampaignRequest request, CancellationToken cancellationToken)
		{
			var campaignUid = Guid.NewGuid();

			var accountSubscription = await _accountSubscriptionService.GetAccountSubscriptionAsync(request.AccountUid);

			if(accountSubscription == null)
			{
				_logger.LogWarning("Account with uid {accountUid} is not subscribed.", request.AccountUid);

				return Guid.Empty;
			}

			var campaigns = await _repository.GetAccountCampaignsAsync(request.AccountUid);
			var activeCampaigns = campaigns.Where(c => c.ActiveInd && c.IsActive() || c.WillBeInProgress()).ToList();

			if(accountSubscription.IsTrial)
			{
				if(activeCampaigns.Count >= _hrvsterSettings.TrialPromotions)
				{
					_logger.LogWarning("Account with uid {accountUid} has a total of {activeCampaignCount} active campaigns and their current max is {maxCampaigns}",
						request.AccountUid,
						activeCampaigns.Count,
						_hrvsterSettings.TrialPromotions);
					_notificationService.AddErrorToastMessage($"You cannot create more promotions.");
					_notificationService.AddInfoToastMessage($"You can only have {_hrvsterSettings.TrialPromotions} active promotions while on Trial mode.");

					return Guid.Empty;
				}
			}

			if(!accountSubscription.IsTrial)
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
						_notificationService.AddInfoToastMessage($"You can only have {accountSubscription.Plan.PromotionsPerMonth} active promotions per month.");

						return Guid.Empty;
					}
				}
			}

			await _repository.CreateCampaignAsync(new Campaign
			{
				CampaignUid = campaignUid,
				CampaignName = request.NewCampaign.Name,
				Description = request.NewCampaign.Description,
				StartDate = request.NewCampaign.StartDate,
				EndDate = request.NewCampaign.EndDate,
				CampaignType = new CampaignType
				{
					CampaignTypeUid = request.NewCampaign.CampaignTypeUid
				},
				Account = new Models.Account
				{
					AccountUid = request.AccountUid
				}
			}, request.TimezoneId, request.IsDaylight);

			await _mediator.Publish(new CampaignCreated(request.NewCampaign, request.AccountUid, campaignUid));

			return campaignUid;
		}
	}
}
