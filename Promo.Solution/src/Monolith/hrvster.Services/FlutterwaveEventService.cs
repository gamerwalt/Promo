using hrvster.ApplicationServices.Payments;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services
{
	public class FlutterwaveEventService : IService
	{
		private readonly ILogger<FlutterwaveEventService> _logger;
		private readonly IFlutterwaveEventService _service;

		public FlutterwaveEventService(ILoggerFactory loggerFactory, IFlutterwaveEventService service)
		{
			_logger = loggerFactory.CreateLogger<FlutterwaveEventService>();
			_service = service;
		}

		public async Task ProcessService(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Started processing Flutterwave event services.");

			await _service.ProcessFlutterwaveEventsAsync(cancellationToken);

			_logger.LogInformation("Finished processing Flutterwave event services.");
		}
	}
}
