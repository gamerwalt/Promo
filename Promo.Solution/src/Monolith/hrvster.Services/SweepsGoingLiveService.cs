using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Models;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services
{
	public class SweepsGoingLiveService : IService
	{
		private readonly ILogger<SweepsGoingLiveService> _logger;
		private readonly ISweepstakesService _sweepstakesService;
		private readonly IAccountsService _accountsService;
		private readonly IMediator _mediator;

		public SweepsGoingLiveService(
			ILoggerFactory loggerFactory,
			ISweepstakesService sweepstakesService,
			IAccountsService accountsService,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SweepsGoingLiveService>();
			_sweepstakesService = sweepstakesService;
			_accountsService = accountsService;
			_mediator = mediator;
		}

		public async Task ProcessService(CancellationToken cancellationToken)
		{
			await SendNotificationForLiveSweepstakes(cancellationToken);
			await ProcessSweepstakesGoingLiveInAnHour(cancellationToken);
		}

		private async Task SendNotificationForLiveSweepstakes(CancellationToken cancellationToken)
		{
			var liveSweeps = await _sweepstakesService.GetLiveSweepsAsync(cancellationToken);
			_logger.LogInformation("Retrieved {count} sweepstakes that are already live and no notification has been given",
				liveSweeps.Count());

			foreach(var sweep in liveSweeps)
			{
				_logger.LogInformation("Processing notification for sweepstake {sweepName}", sweep.Name);

				if(!sweep.SweepNotification.SweepsLive)
				{
					var account = await _accountsService.GetAccountByUidAsync(sweep.Account.AccountUid);
					if(account == null)
					{
						_logger.LogWarning("Could not load the account for uid {accountUid}", sweep.Account.AccountUid);
						continue;
					}

					await _mediator.Send(new NotifyUserSweepLiveRequest(sweep, account));
					await _sweepstakesService.UpdateSweepNotificationAsync(sweep, Models.Sweepstakes.SweepNotificationType.SweepsLive, cancellationToken);
				}
			}
		}

		private async Task ProcessSweepstakesGoingLiveInAnHour(CancellationToken cancellationToken)
		{
			var hourValue = 1;
			var sweepsGoingLiveInAnHour = await _sweepstakesService.GetSweepsGoingLiveInHourAsync(hourValue, cancellationToken);

			_logger.LogInformation("Loaded {count} sweepstakes going live in {hourValue} hour(s)",
				sweepsGoingLiveInAnHour.Count(),
				hourValue);

			var accounts = new List<Account>();
			foreach (var sweep in sweepsGoingLiveInAnHour)
			{
				_logger.LogInformation("Retrieving sweep notification for sweepstake {sweepName}", sweep.Name);

				var sweepNotification = await _sweepstakesService.GetSweepsNotificationAsync(sweep, cancellationToken);

				_logger.LogInformation("Retrieved the sweep notification {sweepNotification} for sweepstake {sweepName}",
					sweepNotification.ToJsonString(),
					sweep.Name);

				if (sweepNotification.SweepsGoingLive)
					continue;

				if (cancellationToken.IsCancellationRequested)
					break;

				_logger.LogInformation("Sending email to user about the sweepstake {sweepName} going live in about an hour.",
					sweep.Name);

				_logger.LogInformation("Retrieving account information for uid {accountUid}", sweep.Account.AccountUid);

				var account = await _accountsService.GetAccountByUidAsync(sweep.Account.AccountUid);
				if (account == null)
				{
					_logger.LogWarning("Could not load the account for uid {accountUid}", sweep.Account.AccountUid);
					continue;
				}

				var sweepProperty = await _sweepstakesService.GetSweepDrawPropertiesAndRules(sweep.SweepUid, cancellationToken);

				_logger.LogInformation("Notfying the user {emailAddress} that sweepstake {sweepName} is going live.",
					account.EmailAddress,
					sweep.Name);

				await _mediator.Send(new NotifyUserSweepsGoingLiveRequest(account, sweep, sweepProperty, hourValue));
				//await _mediator.Send(new NotifyUserSweepMissingPropertyRequest(sweep, sweepProperty, account));

				_logger.LogInformation("Updating sweepstakes {sweepName} as notification going live sent.", sweep.Name);

				await _sweepstakesService.UpdateSweepNotificationAsync(sweep, Models.Sweepstakes.SweepNotificationType.SweepsGoingLive, cancellationToken);
			}
		}
	}
}
