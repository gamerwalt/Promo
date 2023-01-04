using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class NotifyUserSweepsGoingLive : AsyncRequestHandler<NotifyUserSweepsGoingLiveRequest>
	{
		private readonly ILogger<NotifyUserSweepsGoingLive> _logger;
		private readonly IMailRepository _mailRepository;
		private readonly IAccountsRepository _accountsRepository;
		private readonly HarvesterSettingsOptions _harvesterSettings;

		public NotifyUserSweepsGoingLive(
			ILoggerFactory loggerFactory,
			IMailRepository mailRepository,
			IAccountsRepository accountsRepository,
			HarvesterSettingsOptions harvesterSettings)
		{
			_logger = loggerFactory.CreateLogger<NotifyUserSweepsGoingLive>();
			_mailRepository = mailRepository;
			_accountsRepository = accountsRepository;
			_harvesterSettings = harvesterSettings;
		}

		protected override async Task Handle(NotifyUserSweepsGoingLiveRequest request, CancellationToken cancellationToken)
		{
			var account = await _accountsRepository.GetAsync(request.AccountUid);

			if (account == null)
			{
				_logger.LogInformation("Unable to locate the account with uid {uid}", request.AccountUid);

				return;
			}

			_logger.LogInformation("Request received to notify user {email} that sweepstake {sweepName} is going live in {hourValue} hour.",
				account.EmailAddress,
				request.Sweepstake.Name,
				request.HourValue);

			await _mailRepository.CreateAsync(new Models.Email
			{
				EmailUid = Guid.NewGuid(),
				EmailAddress = account.EmailAddress,
				Status = EmailStatus.New,
				Template = EmailTemplate.SweepsGoingLiveSoon,
				Parameters = new Dictionary<string, string>
				{
					{"EmailAddress", account.EmailAddress },
					{"SweepstakeName", request.Sweepstake.Name },
					{"StartDate", request.Sweepstake.StartDate.ToString("f") },
					{"EndDate", request.Sweepstake.EndDate.ToString("f") },
					{"HourValue", request.HourValue.ToString() },
					{"DisplayName", account.DisplayName },
					{"DashboardLink", new Uri(_harvesterSettings.HrvsterUrl).GenerateUrl(UrlTypes.DashboardLink).ToString()}
				}
			});

			_logger.LogInformation("Request to notify user {email} that sweepstake {sweepName} completed.",
				account.EmailAddress,
				request.Sweepstake.Name);
		}
	}
}
