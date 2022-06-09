using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services
{
	public class TestService : IService
	{
		private readonly ILogger<ScheduledTweetService> _logger;

		public TestService(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<ScheduledTweetService>();
		}

		public Task ProcessService(CancellationToken token)
		{
			_logger.LogInformation("Processing service of {serviceName}", nameof(TestService));

			return Task.CompletedTask;
		}
	}
}
