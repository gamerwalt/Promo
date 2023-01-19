using hrvster.Core.Requests.ServiceRequests;
using hrvster.Core.Requests.Sweepstakes;
using hrvster.Infrastructure;
using hrvster.Models.Sweepstakes;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class PerformDraw : AsyncRequestHandler<PerformDrawRequest>
	{
		private readonly ILogger<PerformDraw> _logger;
		private readonly IMediator _mediator;
		private readonly ISweepsRepository _sweepsRepository;

		public PerformDraw(ILoggerFactory loggerFactory, IMediator mediator, ISweepsRepository sweepsRepository)
		{
			_logger = loggerFactory.CreateLogger<PerformDraw>();
			_mediator = mediator;
			_sweepsRepository = sweepsRepository;
		}

		protected override async Task Handle(PerformDrawRequest request, CancellationToken cancellationToken)
		{
			var sweepNotifications = await _mediator.Send(new GetSweepNotificationRequest(request.DrawProperties.SweepDrawPropertiesAndRule.Sweep));

			if (sweepNotifications != null && sweepNotifications.DrawHappened)
			{
				_logger.LogWarning("It looks like the sweepstake {sweepName}'s draw has happened.",
					request.DrawProperties.SweepDrawPropertiesAndRule.Sweep.Name);

				return;
			}

			await _mediator.Send(new DrawSweepstakesWinnerRequest(request.DrawProperties.SweepDrawPropertiesAndRule.Sweep, request.DrawProperties));

			await _mediator.Send(new NotifyUserOfDrawPerformedAndWinnerPickedRequest(request.DrawProperties.SweepDrawPropertiesAndRule.Sweep));

			await _mediator.Send(new UpsertSweepNotificationRequest(request.DrawProperties.SweepDrawPropertiesAndRule.Sweep, SweepNotificationType.DrawHappened));
		}
	}
}
