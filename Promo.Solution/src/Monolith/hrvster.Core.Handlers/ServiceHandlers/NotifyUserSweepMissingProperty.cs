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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class NotifyUserSweepMissingProperty : AsyncRequestHandler<NotifyUserSweepMissingPropertyRequest>
	{
		private readonly ILogger<NotifyUserSweepMissingProperty> _logger;
		private readonly IMailRepository _mailRepository;
		private readonly HarvesterSettingsOptions _harvesterSettings;

		public NotifyUserSweepMissingProperty(
			ILoggerFactory loggerFactory, 
			IMailRepository mailRepository,
			HarvesterSettingsOptions harvesterSettings) 
		{ 
			_logger = loggerFactory.CreateLogger<NotifyUserSweepMissingProperty>();
			_mailRepository = mailRepository;
			_harvesterSettings= harvesterSettings;
		}

		protected override async Task Handle(NotifyUserSweepMissingPropertyRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Adding an email notification for missing sweep property {property} for Sweep {sweepName} to Account {account}",
				request.SweepProperty.ToJsonString(),
				request.Sweep.Name,
				request.Account.ToJsonString());

			await _mailRepository.CreateAsync(new Models.Email
			{
				EmailUid = Guid.NewGuid(),
				EmailAddress = request.Account.EmailAddress,
				Status = EmailStatus.New,
				Template = EmailTemplate.SweepsGoingLiveSoonMissingProperty,
				Parameters = new Dictionary<string, string>
				{
					{ EmailParameters.EmailAddress, request.Account.EmailAddress },
					{ EmailParameters.SweepstakeName, request.Sweep.Name },
					{ EmailParameters.StartDate, request.Sweep.StartDate.ToString("f") },
					{ EmailParameters.EndDate, request.Sweep.EndDate.ToString("f") },
					{ EmailParameters.DisplayName, request.Account.DisplayName },
					{ EmailParameters.RulesCount, request.SweepProperty.Rules.Count().ToString() },
					{ EmailParameters.RulesList, GetRuleList(request) },
					{ EmailParameters.PrizesCount, request.SweepProperty.Prizes.Count().ToString() },
					{ EmailParameters.PrizeList, GetPrizeList(request) },
					{ EmailParameters.WinnerCount, GetWinnerCount(request) },
					{ EmailParameters.WinnerBackupCount, GetWinnerBackupCount(request) },
					{ EmailParameters.EntryPerUser, GetEntryPerUser(request) },
					{ EmailParameters.DashboardLink, new Uri(_harvesterSettings.HrvsterUrl).GenerateUrl(UrlTypes.DashboardLink).ToString()}
				}
			});
		}

		private static string GetEntryPerUser(NotifyUserSweepMissingPropertyRequest request)
			=> request.SweepProperty.EntryRule == null ?
				"0" :
				request.SweepProperty.EntryRule?.EntryPerUser.ToString();
		
		private static string GetWinnerBackupCount(NotifyUserSweepMissingPropertyRequest request)
			=> request.SweepProperty.DrawRule == null ?
				"0" :
				request.SweepProperty.DrawRule?.WinnerBackupCount.ToString();

		private static string GetWinnerCount(NotifyUserSweepMissingPropertyRequest request)
			=> request.SweepProperty.DrawRule == null ? 
				"0" : 
				request.SweepProperty.DrawRule?.WinnerCount.ToString();

		private static string GetPrizeList(NotifyUserSweepMissingPropertyRequest request)
			=> request.SweepProperty.Prizes.Count() == 0 ?
				"No Prizes!" :
				string.Join(", ", request.SweepProperty.Prizes.Select(c => c.PrizeName));

		private static string GetRuleList(NotifyUserSweepMissingPropertyRequest request)
			=> request.SweepProperty.Rules.Count() == 0 ? 
				"No Hashtags!" : 
				string.Join(", ", request.SweepProperty.Rules.Select(c => c.WatchValue));
		
	}
}
