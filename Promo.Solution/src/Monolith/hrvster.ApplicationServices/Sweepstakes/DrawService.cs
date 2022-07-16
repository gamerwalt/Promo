using hrvster.Core.Requests.ServiceRequests;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Sweepstakes
{
	public interface IDrawService
	{
		Task ProcessDrawsAsync(CancellationToken token);
	}

	public class DrawService : IDrawService
	{
		private readonly ILogger<DrawService> _logger;
		private readonly IMediator _mediator;

		public DrawService(
			ILoggerFactory loggerFactory,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<DrawService>();
			_mediator = mediator;
		}

		public async Task ProcessDrawsAsync(CancellationToken token)
		{
			var sweepsDrawRules = await _mediator.Send(new GetSweepstakesRequiringADrawRequest(), token);
		}
	}
}
