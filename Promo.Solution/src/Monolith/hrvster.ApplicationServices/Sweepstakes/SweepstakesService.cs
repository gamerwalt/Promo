using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Core.Requests.Sweepstakes;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Sweepstakes;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Sweepstakes
{
	public interface ISweepstakesService
	{
		Task<IEnumerable<Sweep>> GetLiveSweepsAsync(CancellationToken cancellationToken);
		Task<SweepProperty> GetSweepDrawPropertiesAndRules(Guid sweepUid, CancellationToken cancellationToken);
		Task<IEnumerable<Sweep>> GetSweepsGoingLiveInHourAsync(int hour, CancellationToken cancellationToken);
		Task<SweepNotification> GetSweepsNotificationAsync(Sweep sweep, CancellationToken cancellationToken);
		Task<bool> IsMaxedOut(Sweep sweep, CancellationToken cancellationToken);
		Task UpdateSweepNotificationAsync(Sweep sweep, SweepNotificationType notificationType, CancellationToken cancellationToken);
	}

	public class SweepstakesService : ISweepstakesService
	{
		private readonly ILogger<SweepstakesService> _logger;
		private readonly IMediator _mediator;
		private readonly ISweepsRepository _sweepRepository;
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly ISweepsEntriesRepository _sweepsEntriesRepository;

		public SweepstakesService(
			ILoggerFactory loggerFactory, 
			IMediator mediator,
			ISweepsRepository sweepsRepository,
			IAccountSubscriptionService accountSubscriptionService,
			ISweepsEntriesRepository sweepsEntriesRepository)
		{
			_logger = loggerFactory.CreateLogger<SweepstakesService>();
			_mediator = mediator;
			_sweepRepository = sweepsRepository;
			_accountSubscriptionService = accountSubscriptionService;
			_sweepsEntriesRepository = sweepsEntriesRepository;
		}

		public async Task<IEnumerable<Sweep>> GetLiveSweepsAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Retrieving sweepstakes that are live and we haven't notified their account holders.");

			var sweepsLive = await _mediator.Send(new GetLiveSweepsRequest());

			_logger.LogInformation("Retrieved a total of {count} sweepstakes that are live and we haven't notified their account holders",
				sweepsLive.Count());

			return sweepsLive;
		}

		public async Task<SweepProperty> GetSweepDrawPropertiesAndRules(Guid sweepUid, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Retrieving sweep properties for sweep with uid {sweepUid}", sweepUid);

			var properties = await _mediator.Send(new GetSweepPropertiesRequest(sweepUid));

			_logger.LogInformation("Sweep properties for sweep with uid {sweepUid} retrieved.", sweepUid);
			_logger.LogInformation("Properties received for sweep : {properties}", properties.ToJsonString());

			return properties;
		}
			

		public async Task<IEnumerable<Sweep>> GetSweepsGoingLiveInHourAsync(int hour, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Retrieving sweeps going live in {hour} hour(s)", hour);

			var sweepsGoingLiveInAnHour = await _mediator.Send(new GetSweepsGoingLiveInRequest(hour));

			_logger.LogInformation("Retrieved {count} sweepstakes going live in {hour} hour(s)",
				sweepsGoingLiveInAnHour.Count(),
				hour);

			return sweepsGoingLiveInAnHour;
		}

		public async Task<SweepNotification> GetSweepsNotificationAsync(Sweep sweep, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return default(SweepNotification);

			_logger.LogInformation("Request received to get the sweep notification for sweepstake {sweepName}",
				sweep.Name);

			return await _mediator.Send(new GetSweepNotificationRequest(sweep));
		}

		public async Task<bool> IsMaxedOut(Sweep sweep, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return false;

			if(sweep.Account == null)
			{
				sweep = await _sweepRepository.GetAsync(sweep.SweepUid);
			}

			if(sweep.Account == null)
			{
				_logger.LogWarning("Could not find out if the Sweepstake with name {sweepName} and uid {sweepUid} is not maxed out. Because we could not retrieve the account.",
					sweep.Name,
					sweep.SweepUid);

				return true;
			}

			var accountSubscription = await _accountSubscriptionService
				.GetAccountSubscriptionAsync(sweep.Account.AccountUid);

			if (accountSubscription == null)
			{
				_logger.LogWarning("Could not find out if the Sweepstake with name {sweepName} and uid {sweepUid} is not maxed out. Because we could not retrieve the account subscription.",
					sweep.Name,
					sweep.SweepUid);

				return true;
			}

			//get total entries
			var totalEntries = await _sweepsEntriesRepository.GetSweepEntriesAsync(sweep.SweepUid);

			return totalEntries.Count() >= accountSubscription.Plan.SweepstakesMaximumEntries;
		}

		public async Task UpdateSweepNotificationAsync(
			Sweep sweep, SweepNotificationType notificationType, CancellationToken cancellationToken)
		{
			if(cancellationToken.IsCancellationRequested) return;

			_logger.LogInformation("Request received to update the sweep notification {sweepNotification} for {sweepName}",
				notificationType.ToString(),
				sweep.Name);

			await _mediator.Send(new UpsertSweepNotificationRequest(sweep, notificationType));
		}
	}
}
