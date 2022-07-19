using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services
{
	public class EmailNotificationService : IService
	{
		private readonly ILogger<EmailNotificationService> _logger;

		public EmailNotificationService(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<EmailNotificationService>();
		}

		public async Task ProcessService(CancellationToken token)
		{
			_logger.LogInformation("Starting the {serviceName}", nameof(EmailNotificationService));
		}
	}
}
