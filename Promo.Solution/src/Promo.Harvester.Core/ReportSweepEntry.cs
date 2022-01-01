using MediatR;
using Promo.Clients.Internal;
using Promo.Harvester.Core.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Harvester.Core
{
	public class ReportSweepEntry : INotificationHandler<SweepEntryRecorded>
	{
		private readonly IReportingClient _reportingClient;

		public ReportSweepEntry(IReportingClient reportingClient)
		{
			_reportingClient = reportingClient;
		}

		public async Task Handle(SweepEntryRecorded notification, CancellationToken cancellationToken)
		{
			await _reportingClient.ReportSweepEntryAsync(
				notification.SweepUid,
				notification.CampaignUid,
				notification.ScreenName,
				notification.UserIdentifierType,
				externalUserId: notification.TweetUserId,
				externalId: notification.TweetId
			);
		}
	}
}
