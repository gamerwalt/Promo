using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Exceptions;
using hrvster.Infrastructure;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class CreateSweep : AsyncRequestHandler<CreateSweepRequest>
	{
		private readonly ILogger<CreateSweep> _logger;
		private readonly ISweepsRepository _repository;
		private readonly IMediator _mediator;
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly IToastNotification _notificationService;

		public CreateSweep(
			ILoggerFactory loggerFactory, 
			ISweepsRepository repository,
			IMediator mediator,
			IAccountSubscriptionService accountSubscriptionService,
			IToastNotification notificationService)
		{
			_logger = loggerFactory.CreateLogger<CreateSweep>();
			_repository = repository;
			_mediator = mediator;
			_accountSubscriptionService = accountSubscriptionService;
			_notificationService = notificationService;
		}

		protected override async Task Handle(CreateSweepRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling the action {className} for the request {request}.",
				nameof(CreateSweep),
				request.ToJsonString());

			var accountSubscription = await _accountSubscriptionService.GetAccountSubscriptionAsync(request.AccountUid);

			if (accountSubscription == null)
			{
				_logger.LogWarning("Account with uid {accountUid} is not subscribed.", request.AccountUid);

				throw new AccountIsNotSubscribedException($"Account with uid {request.AccountUid} is not subscribed.");
			}

			var sweeps = await _repository.GetAllForAccountAsync(request.AccountUid);
			var activeSweeps = sweeps.Where(s => s.IsActive() || s.WillBeInProgress()).ToList();

			if (accountSubscription.Plan.SweepstakesPerMonth != 0)
			{
				if (activeSweeps.Count >= accountSubscription.Plan.SweepstakesPerMonth)
				{
					_logger.LogWarning("Account with uid {accountUid} has a total of {activeCampaignCount} active campaigns and their current max is {maxPromotions}",
						request.AccountUid,
						activeSweeps.Count,
						accountSubscription.Plan.PromotionsPerMonth);

					_notificationService.AddErrorToastMessage($"You cannot create more promotions.");
					_notificationService.AddInfoToastMessage($"You can only have {accountSubscription.Plan.PromotionsPerMonth} active promotions per month.");

					return;
				}
			}

			var sweepUid = Guid.NewGuid();

			var sweep = new Models.Sweep
			{
				SweepUid = sweepUid,
				Account = new Models.Account
				{
					AccountUid = request.AccountUid
				},
				Name = request.Name,
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				DrawDate = request.DrawDate,
				Campaign = new Models.Campaign
				{
					CampaignUid = request.CampaignUid
				}
			};

			if(request.SweepUid.Equals(Guid.Empty))
			{
				await _repository.CreateAsync(sweep, request.TimeZoneId, request.IsDaylight);
			}
			else
			{
				sweep.SweepUid = request.SweepUid;

				await _repository.CreateAsync(sweep, request.TimeZoneId, request.IsDaylight);
			}

			await _mediator.Publish(new SweepCreated(sweep));
		}
	}
}
