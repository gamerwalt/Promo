using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services
{
	public class TwitterHarvestingService : IService
	{
		private readonly ILogger<ScheduledTweetService> _logger;

		public TwitterHarvestingService(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<ScheduledTweetService>();
		}

		public async Task ProcessService(CancellationToken token)
		{
			_logger.LogInformation("Starting {serviceName}", nameof(TwitterHarvestingService));


		}
	}
}
