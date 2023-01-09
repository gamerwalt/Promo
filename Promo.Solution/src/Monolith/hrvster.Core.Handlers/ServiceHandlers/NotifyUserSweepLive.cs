using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Options;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class NotifyUserSweepLive : AsyncRequestHandler<NotifyUserSweepLiveRequest>
	{
		private readonly IMediator _mediator;
		private readonly IMailRepository _repository;
		private readonly HarvesterSettingsOptions _harvesterSettings;

		public NotifyUserSweepLive(IMailRepository repository, IMediator mediator, HarvesterSettingsOptions harvesterSettings)
		{
			_repository = repository;
			_mediator = mediator;
			_harvesterSettings = harvesterSettings;
		}

		protected override async Task Handle(NotifyUserSweepLiveRequest request, CancellationToken cancellationToken)
		{
			var sweepProperty = await _mediator.Send(new GetSweepPropertiesRequest(request.Sweepstake.SweepUid));

			await _repository.CreateAsync(new Models.Email
			{
				Template = Models.EmailTemplate.SweepLive,
				EmailAddress = request.Account.EmailAddress,
				EmailUid = Guid.NewGuid(),
				Status = Models.EmailStatus.New,
				Parameters = new Dictionary<string, string>()
				{
					{ EmailParameters.EmailAddress, request.Account.EmailAddress },
					{ EmailParameters.SweepstakeName, request.Sweepstake.Name },
					{ EmailParameters.StartDate, request.Sweepstake.StartDate.ToString("f") },
					{ EmailParameters.EndDate, request.Sweepstake.EndDate.ToString("f") },
					{ EmailParameters.DisplayName, request.Account.DisplayName },
					{ EmailParameters.RulesCount, sweepProperty.Rules.Count().ToString() },
					{ EmailParameters.RulesList, GetRuleList(sweepProperty) },
					{ EmailParameters.PrizesCount, sweepProperty.Prizes.Count().ToString() },
					{ EmailParameters.PrizeList, GetPrizeList(sweepProperty) },
					{ EmailParameters.WinnerCount, GetWinnerCount(sweepProperty) },
					{ EmailParameters.WinnerBackupCount, GetWinnerBackupCount(sweepProperty) },
					{ EmailParameters.EntryPerUser, GetEntryPerUser(sweepProperty) },
					{ EmailParameters.DashboardLink, new Uri(_harvesterSettings.HrvsterUrl).GenerateUrl(UrlTypes.DashboardLink).ToString()},
				}
			});
		}

		private static string GetEntryPerUser(SweepProperty property)
			=> property.EntryRule == null ?
				"0" :
				property.EntryRule?.EntryPerUser.ToString();

		private static string GetWinnerBackupCount(SweepProperty property)
			=> property.DrawRule == null ?
				"0" :
				property.DrawRule?.WinnerBackupCount.ToString();

		private static string GetWinnerCount(SweepProperty property)
			=> property.DrawRule == null ?
				"0" :
				property.DrawRule?.WinnerCount.ToString();

		private static string GetPrizeList(SweepProperty property)
			=> property.Prizes.Count() == 0 ?
				"No Prizes!" :
				string.Join(", ", property.Prizes.Select(c => c.PrizeName));

		private static string GetRuleList(SweepProperty property)
			=> property.Rules.Count() == 0 ?
				"No Hashtags!" :
				string.Join(", ", property.Rules.Select(c => c.WatchValue));
	}
}
