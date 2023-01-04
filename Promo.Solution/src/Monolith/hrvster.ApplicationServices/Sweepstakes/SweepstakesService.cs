using hrvster.Core.Requests.ServiceRequests;
using hrvster.Models;
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
		Task<IEnumerable<Sweep>> GetSweepsGoingLiveInHourAsync(int hour, CancellationToken cancellationToken);
		Task UpdateAsNotificationGoingLiveSentAsync(Guid sweepUid);
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

		public async Task<IEnumerable<Sweep>> GetSweepsGoingLiveInHourAsync(int hour, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Retrieving sweeps going live in {hour} hour(s)", hour);

			var sweepsGoingLiveInAnHour = await _mediator.Send(new GetSweepsGoingLiveInRequest(hour));

			_logger.LogInformation("Retrieved {count} sweepstakes going live in {hour} hour(s)",
				sweepsGoingLiveInAnHour.Count(),
				hour);

			return sweepsGoingLiveInAnHour;
		}

		public async Task UpdateAsNotificationGoingLiveSentAsync(Guid sweepUid)
		{
			await _mediator.Send(new UpdateSweepsAsNotificationGoingLiveSentRequest(sweepUid));
		}
	}
}
