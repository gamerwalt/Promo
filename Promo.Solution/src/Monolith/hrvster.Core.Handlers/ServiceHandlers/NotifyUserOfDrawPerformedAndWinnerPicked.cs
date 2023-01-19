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
	public class NotifyUserOfDrawPerformedAndWinnerPicked : AsyncRequestHandler<NotifyUserOfDrawPerformedAndWinnerPickedRequest>
	{
		private readonly ILogger<NotifyUserOfDrawPerformedAndWinnerPicked> _logger;
		private readonly IAccountsRepository _accountsRepository;
		private readonly IMailRepository _mailRepository;
		private readonly ISweepsRepository _sweepsRepository;
		private readonly HarvesterSettingsOptions _harvesterSettings;

		public NotifyUserOfDrawPerformedAndWinnerPicked(
			ILoggerFactory loggerFactory,
			IAccountsRepository accountsRepository,
			IMailRepository mailRepository,
			ISweepsRepository sweepsRepository,
			HarvesterSettingsOptions harvesterSettings)
		{
			_logger = loggerFactory.CreateLogger<NotifyUserOfDrawPerformedAndWinnerPicked>();
			_accountsRepository = accountsRepository;
			_mailRepository = mailRepository;
			_sweepsRepository = sweepsRepository;
			_harvesterSettings = harvesterSettings;
		}

		protected override async Task Handle(NotifyUserOfDrawPerformedAndWinnerPickedRequest request, CancellationToken cancellationToken)
		{
			var accountUid = Guid.Empty;
			if(request.Sweepstake.Account == null)
			{
				var sweep = await _sweepsRepository.GetAsync(request.Sweepstake.SweepUid);
				accountUid = sweep.Account.AccountUid;
			}

			_logger.LogInformation("Retrieving account with uid {accountUid} for the sweepstake {sweepName}",
				accountUid == Guid.Empty ? request.Sweepstake.Account.AccountUid : accountUid,
				request.Sweepstake.Name);

			var account = await _accountsRepository.GetAsync(accountUid == Guid.Empty ? request.Sweepstake.Account.AccountUid : accountUid);

			if (account == null)
			{
				_logger.LogWarning("Unable to retrieve the account for account uid {accountUid}",
					request.Sweepstake.Account.AccountUid);

				return;
			}

			await _mailRepository.CreateAsync(new Models.Email
			{
				Template = Models.EmailTemplate.DrawCompleted,
				EmailAddress = account.EmailAddress,
				Status = Models.EmailStatus.New,
				EmailUid = Guid.NewGuid(),
				Parameters = new Dictionary<string, string>
				{
					{ EmailParameters.EmailAddress, account.EmailAddress },
					{ EmailParameters.DisplayName, account.DisplayName },
					{ EmailParameters.SweepstakeName, request.Sweepstake.Name },
					{ EmailParameters.StartDate, request.Sweepstake.StartDate.ToString("f") },
					{ EmailParameters.EndDate, request.Sweepstake.EndDate.ToString("f") },
					{ EmailParameters.DrawDate, request.Sweepstake.DrawDate.GetValueOrDefault().ToString("f") },
					{ EmailParameters.DashboardLink, new Uri(_harvesterSettings.HrvsterUrl).GenerateUrl(UrlTypes.DashboardLink).ToString()},
				}
			});
		}
	}
}
