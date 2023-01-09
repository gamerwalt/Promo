using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class NotifyUserSweepsGoingLive : AsyncRequestHandler<NotifyUserSweepsGoingLiveRequest>
	{
		private readonly ILogger<NotifyUserSweepsGoingLive> _logger;
		private readonly IMailRepository _mailRepository;
		private readonly HarvesterSettingsOptions _harvesterSettings;

		public NotifyUserSweepsGoingLive(
			ILoggerFactory loggerFactory,
			IMailRepository mailRepository,
			HarvesterSettingsOptions harvesterSettings)
		{
			_logger = loggerFactory.CreateLogger<NotifyUserSweepsGoingLive>();
			_mailRepository = mailRepository;
			_harvesterSettings = harvesterSettings;
		}

		protected override async Task Handle(NotifyUserSweepsGoingLiveRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Request received to notify user {email} that sweepstake {sweepName} is going live in {hourValue} hour.",
				request.Account.EmailAddress,
				request.Sweepstake.Name,
				request.HourValue);

			await _mailRepository.CreateAsync(new Models.Email
			{
				EmailUid = Guid.NewGuid(),
				EmailAddress = request.Account.EmailAddress,
				Status = EmailStatus.New,
				Template = EmailTemplate.SweepsGoingLiveSoon,
				Parameters = new Dictionary<string, string>
				{
					{ EmailParameters.EmailAddress, request.Account.EmailAddress },
					{ EmailParameters.SweepstakeName, request.Sweepstake.Name },
					{ EmailParameters.StartDate, request.Sweepstake.StartDate.ToString("f") },
					{ EmailParameters.EndDate, request.Sweepstake.EndDate.ToString("f") },
					{ EmailParameters.DisplayName, request.Account.DisplayName },
					{ EmailParameters.RulesCount, request.SweepProperty.Rules.Count().ToString() },
					{ EmailParameters.RulesList, GetRuleList(request) },
					{ EmailParameters.PrizesCount, request.SweepProperty.Prizes.Count().ToString() },
					{ EmailParameters.PrizeList, GetPrizeList(request) },
					{ EmailParameters.WinnerCount, GetWinnerCount(request) },
					{ EmailParameters.WinnerBackupCount, GetWinnerBackupCount(request) },
					{ EmailParameters.EntryPerUser, GetEntryPerUser(request) },
					{ EmailParameters.DashboardLink, new Uri(_harvesterSettings.HrvsterUrl).GenerateUrl(UrlTypes.DashboardLink).ToString()},
					{EmailParameters.HourValue, request.HourValue.ToString() },
				}
			});

			_logger.LogInformation("Request to notify user {email} that sweepstake {sweepName} completed.",
				request.Account.EmailAddress,
				request.Sweepstake.Name);
		}

		private static string GetEntryPerUser(NotifyUserSweepsGoingLiveRequest request)
			=> request.SweepProperty.EntryRule == null ?
				"0" :
				request.SweepProperty.EntryRule?.EntryPerUser.ToString();

		private static string GetWinnerBackupCount(NotifyUserSweepsGoingLiveRequest request)
			=> request.SweepProperty.DrawRule == null ?
				"0" :
				request.SweepProperty.DrawRule?.WinnerBackupCount.ToString();

		private static string GetWinnerCount(NotifyUserSweepsGoingLiveRequest request)
			=> request.SweepProperty.DrawRule == null ?
				"0" :
				request.SweepProperty.DrawRule?.WinnerCount.ToString();

		private static string GetPrizeList(NotifyUserSweepsGoingLiveRequest request)
			=> request.SweepProperty.Prizes.Count() == 0 ?
				"No Prizes!" :
				string.Join(", ", request.SweepProperty.Prizes.Select(c => c.PrizeName));

		private static string GetRuleList(NotifyUserSweepsGoingLiveRequest request)
			=> request.SweepProperty.Rules.Count() == 0 ?
				"No Hashtags!" :
				string.Join(", ", request.SweepProperty.Rules.Select(c => c.WatchValue));
	}
}
