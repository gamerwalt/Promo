using hrvster.Core.Requests.Notifications.Harvester;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.Notifications.Handlers
{
	public class ReportSweepEntry : INotificationHandler<SweepEntryRecorded>
	{
		private readonly IReportingRepository _reportingRepository;

		public ReportSweepEntry(IReportingRepository reportingRepository)
		{
			_reportingRepository = reportingRepository;
		}

		public async Task Handle(SweepEntryRecorded notification, CancellationToken cancellationToken)
		{
			await _reportingRepository.UpsertSweepEntryReportAsync(
				new SweepEntryReport
				{
					SweepUid = notification.SweepUid,
					CampaignUid = notification.CampaignUid,
					ScreenName = notification.ScreenName,
					UserIdentifierType = notification.UserIdentifierType,
					ExternalUserId = notification.TweetUserId,
					ExternalId = notification.TweetId
				}
			);
		}
	}
}
