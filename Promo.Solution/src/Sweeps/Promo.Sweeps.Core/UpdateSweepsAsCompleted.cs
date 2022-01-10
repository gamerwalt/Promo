using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Sweeps.Core.Notifications;
using Promo.Sweeps.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Core
{
	public class UpdateSweepsAsCompleted : INotificationHandler<SweepDrawHappened>
	{
		private readonly ISweepsRepository _repository;
		private readonly IMediator _mediator;
		private readonly ILogger<UpdateSweepsAsCompleted> _logger;

		public UpdateSweepsAsCompleted(ISweepsRepository repository, IMediator mediator, ILoggerFactory loggerFactory)
		{
			_repository = repository;
			_mediator = mediator;
			_logger = loggerFactory.CreateLogger<UpdateSweepsAsCompleted>();
		}

		public async Task Handle(SweepDrawHappened notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Updating sweep {sweepName} for campaign {campaignName} as completed.",
				notification.SweepDrawRule.SweepName,
				notification.SweepDrawRule.CampaignName);

			await _repository.UpdateSweepAsDrawnAsync(notification.SweepDrawRule.SweepUid);

			//Todo: Notify account holder via an email
		}
	}
}
