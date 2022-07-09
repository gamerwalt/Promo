using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Harvesters
{
	public interface ITwitterHarvesterService
	{
		Task StartStreamingAsync();
	}

	public class TwitterHarvesterService : ITwitterHarvesterService
	{
		private readonly ILogger<TwitterHarvesterService> _logger;

		public TwitterHarvesterService(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<TwitterHarvesterService>();
		}

		public async Task StartStreamingAsync()
		{
			throw new NotImplementedException();
		}
	}
}
