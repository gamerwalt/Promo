using hrvster.Core.Requests.ServiceRequests.Notifications;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class SaveSweepEntryIntoReporting : INotificationHandler<SweepEntryFromTweetRecorded>
	{
		private readonly ILogger<SaveSweepEntryIntoReporting> _logger;
		private readonly IReportingRepository _reportingRepository;

		public SaveSweepEntryIntoReporting(ILoggerFactory loggerFactory, IReportingRepository reportingRepository)
		{
			_logger = loggerFactory.CreateLogger<SaveSweepEntryIntoReporting>();
			_reportingRepository = reportingRepository;
		}

		public async Task Handle(SweepEntryFromTweetRecorded notification, CancellationToken cancellationToken)
		{
			await _reportingRepository.UpsertSweepEntryReportAsync(new SweepEntryReport
			{
				CampaignUid = notification.Campaign.CampaignUid,
				SweepUid = notification.Sweep.SweepUid,
				Entries = 1,
				ScreenName = notification.Tweet.CreatedBy.ScreenName,
				UserIdentifierType = UserIdentifierType.TwitterHandle.ToString(),
				ExternalId = notification.Tweet.IdStr,
				ExternalUserId = notification.Tweet.CreatedBy.IdStr,
				SweepEntryUid = Guid.NewGuid()
			});
		}
	}
}
