using hrvster.Core.Requests.ServiceRequests;
using hrvster.Core.Requests.Sweepstakes;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Sweepstakes;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Sweepstakes
{
	public interface ISweepstakesService
	{
		Task<IEnumerable<Sweep>> GetLiveSweepsAsync(CancellationToken cancellationToken);
		Task<SweepProperty> GetSweepDrawPropertiesAndRules(Guid sweepUid, CancellationToken cancellationToken);
		Task<IEnumerable<Sweep>> GetSweepsGoingLiveInHourAsync(int hour, CancellationToken cancellationToken);
		Task<SweepNotification> GetSweepsNotificationAsync(Sweep sweep, CancellationToken cancellationToken);
		Task UpdateSweepNotificationAsync(Sweep sweep, SweepNotificationType notificationType, CancellationToken cancellationToken);
	}

	public class SweepstakesService : ISweepstakesService
	{
		private readonly ILogger<SweepstakesService> _logger;
		private readonly IMediator _mediator;

		public SweepstakesService(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SweepstakesService>();
			_mediator = mediator;
		}

		public async Task<IEnumerable<Sweep>> GetLiveSweepsAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Retrieving sweepstakes that are live and we haven't notified their account holders.");

			var sweepsLive = await _mediator.Send(new GetLiveSweepsRequest());

			_logger.LogInformation("Retrieved a total of {count} sweepstakes that are live and we haven't notified their account holders",
				sweepsLive.Count());

			return sweepsLive;
		}

		public async Task<SweepProperty> GetSweepDrawPropertiesAndRules(Guid sweepUid, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Retrieving sweep properties for sweep with uid {sweepUid}", sweepUid);

			var properties = await _mediator.Send(new GetSweepPropertiesRequest(sweepUid));

			_logger.LogInformation("Sweep properties for sweep with uid {sweepUid} retrieved.", sweepUid);
			_logger.LogInformation("Properties received for sweep : {properties}", properties.ToJsonString());

			return properties;
		}
			

		public async Task<IEnumerable<Sweep>> GetSweepsGoingLiveInHourAsync(int hour, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Retrieving sweeps going live in {hour} hour(s)", hour);

			var sweepsGoingLiveInAnHour = await _mediator.Send(new GetSweepsGoingLiveInRequest(hour));

			_logger.LogInformation("Retrieved {count} sweepstakes going live in {hour} hour(s)",
				sweepsGoingLiveInAnHour.Count(),
				hour);

			return sweepsGoingLiveInAnHour;
		}

		public async Task<SweepNotification> GetSweepsNotificationAsync(Sweep sweep, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return default(SweepNotification);

			_logger.LogInformation("Request received to get the sweep notification for sweepstake {sweepName}",
				sweep.Name);

			return await _mediator.Send(new GetSweepNotificationRequest(sweep));
		}

		public async Task UpdateSweepNotificationAsync(
			Sweep sweep, SweepNotificationType notificationType, CancellationToken cancellationToken)
		{
			if(cancellationToken.IsCancellationRequested) return;

			_logger.LogInformation("Request received to update the sweep notification {sweepNotification} for {sweepName}",
				notificationType.ToString(),
				sweep.Name);

			await _mediator.Send(new UpsertSweepNotificationRequest(sweep, notificationType));
		}
	}
}
