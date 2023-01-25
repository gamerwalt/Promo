using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes.Creation
{
	public class CreateSweepRequestHandler : IRequestHandler<Requests.CreateSweep, Sweep>
	{
		private readonly ILogger<CreateSweepRequestHandler> _logger;
		private readonly IMediator _mediator;
		private readonly ISweepsRepository _repository;
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly IToastNotification _notificationService;

		public CreateSweepRequestHandler(
			ILoggerFactory loggerFactory, 
			IMediator mediator, 
			ISweepsRepository repository,
			IAccountSubscriptionService accountSubscriptionService,
			IToastNotification notificationService)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepRequestHandler>();
			_mediator = mediator;
			_repository = repository;
			_accountSubscriptionService = accountSubscriptionService;
			_notificationService = notificationService;
		}

		public async Task<Sweep> Handle(Requests.CreateSweep request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("{action}... Creating sweep {sweepName} for AccountUid: {accountUid} under campaign {campaignUid}",
				nameof(CreateSweepRequestHandler),
				request.Sweep.Name,
				request.Sweep.AccountUid,
				request.Sweep.CampaignUid);

			var accountSubscription = await _accountSubscriptionService.GetAccountSubscriptionAsync(request.Sweep.AccountUid);

			if (accountSubscription == null)
			{
				_logger.LogWarning("Account with uid {accountUid} is not subscribed.", request.Sweep.AccountUid);

				return default;
			}

			var sweeps = await _repository.GetAllForAccountAsync(request.Sweep.AccountUid);
			var activeSweeps = sweeps.Where(s => s.IsActive() || s.WillBeInProgress()).ToList();

			if (accountSubscription.Plan.SweepstakesPerMonth != 0)
			{
				if (activeSweeps.Count >= accountSubscription.Plan.SweepstakesPerMonth)
				{
					_logger.LogWarning("Account with uid {accountUid} has a total of {activeCampaignCount} active campaigns and their current max is {maxPromotions}",
						request.Sweep.AccountUid,
						activeSweeps.Count,
						accountSubscription.Plan.PromotionsPerMonth);

					_notificationService.AddErrorToastMessage($"You cannot create more promotions.");
					_notificationService.AddInfoToastMessage($"You can only have {accountSubscription.Plan.PromotionsPerMonth} active promotions per month.");

					return default;
				}
			}

			var sweepUid = Guid.NewGuid();
			await _repository.CreateAsync(new Sweep
			{
				SweepUid = sweepUid,
				Campaign = new Campaign
				{
					CampaignUid = request.Sweep.CampaignUid
				},
				Account = new Models.Account
				{
					AccountUid = request.Sweep.AccountUid
				},
				Name = request.Sweep.Name,
				StartDate = request.Sweep.StartDate,
				EndDate = request.Sweep.EndDate,
				DrawDate = request.Sweep.DrawDate
			}, request.TimeZoneId, request.IsDaylight);

			var sweep = await _repository.GetAsync(sweepUid);

			await _mediator.Publish(new SweepCreated(sweep));

			return sweep;
		}
	}
}
