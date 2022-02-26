using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.MessageContracts;
using Promo.Games.Core.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Games.Core
{
	public class PublishSweepCreated : INotificationHandler<SweepCreated>
	{
		private readonly ILogger<PublishSweepCreated> _logger;
		private readonly IRequestClient<ISweepCreated> _submitNewSweepClient;

		public PublishSweepCreated(ILoggerFactory loggerFactory, IRequestClient<ISweepCreated> submitNewSweepClient)
		{
			_logger = loggerFactory.CreateLogger<PublishSweepCreated>();
			_submitNewSweepClient = submitNewSweepClient;
		}

		public async Task Handle(SweepCreated notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("{action}. Publishing sweep with name {sweepName}.", nameof(PublishSweepCreated), notification.Sweep.Name);

			var sweepCreated = new
			{
				SweepUid = notification.Sweep.SweepUid,
				CampaignUid = notification.Sweep.Campaign.CampaignUid,
				AccountUid = notification.Sweep.Account.AccountUid,
				SweepName = notification.Sweep.Name,
				RulesUrl = notification.Sweep.RulesUrl,
				StartDate = notification.Sweep.StartDate,
				EndDate = notification.Sweep.EndDate,
				DrawDate = notification.Sweep.DrawDate
			};

			var (accepted, rejected) = await _submitNewSweepClient.GetResponse<ISweepCreatedAccepted, ISweepCreatedRejected>(sweepCreated);

			if(accepted.IsCompletedSuccessfully)
				_logger.LogInformation("Sweep with Uid {sweepUid} and name {sweepName} queued successfully.",
					notification.Sweep.SweepUid,
					notification.Sweep.Name);
			else
				_logger.LogError("Unable to queue the sweep with Uid {sweepUid} and name {sweepName} for consumers. Reason: {reason}",
					notification.Sweep.SweepUid,
					notification.Sweep.Name,
					rejected.Exception.Message);
		}
	}
}
