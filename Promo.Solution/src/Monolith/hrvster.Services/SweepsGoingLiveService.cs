using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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

		public async Task ProcessService(CancellationToken token)
		{
			var hourValue = 1;
			var sweepsGoingLiveInAnHour = await _sweepstakesService.GetSweepsGoingLiveInHourAsync(hourValue, token);
			var accounts = new List<Account>();
			foreach (var sweep in sweepsGoingLiveInAnHour)
			{
				if (sweep.NotificationSentGoingLive) 
					continue;

				if (token.IsCancellationRequested) 
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

				_logger.LogInformation("Notfying the user {emailAddress} that sweepstake {sweepName} is going live.",
					account.EmailAddress,
					sweep.Name);

				await _mediator.Send(new NotifyUserSweepsGoingLiveRequest(account.AccountUid, sweep, hourValue));

				_logger.LogInformation("Updating sweepstakes {sweepName} as notification going live sent.", sweep.Name);
				await _sweepstakesService.UpdateAsNotificationGoingLiveSentAsync(sweep.SweepUid);
			}
		}
	}
}
