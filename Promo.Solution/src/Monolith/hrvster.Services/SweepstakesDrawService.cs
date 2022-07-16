using hrvster.ApplicationServices.Sweepstakes;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services
{
	public class SweepstakesDrawService : IService
	{
		private readonly ILogger<SweepstakesDrawService> _logger;
		private readonly IDrawService _drawService;

		public SweepstakesDrawService(
			ILoggerFactory loggerFactory,
			IDrawService drawService)
		{
			_logger = loggerFactory.CreateLogger<SweepstakesDrawService>();
			_drawService = drawService;
		}

		public async Task ProcessService(CancellationToken token)
		{
			_logger.LogDebug("Sweepstakes draw service running...");

			await _drawService.ProcessDrawsAsync(token);

			_logger.LogDebug("Sweepstakes draw service completed...");
		}
	}
}
