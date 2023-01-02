using hrvster.ApplicationServices.Harvesters.Twitter;
using hrvster.Models;
using hrvster.Models.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services
{
	public class HarvesterRestarterService : IService
	{
		private readonly ILogger<HarvesterRestarterService> _logger;
		private readonly HarvesterSettingsOptions _harvesterSettingsOptions;
		private readonly IHarvesterService _harvesterService;

		public HarvesterRestarterService(
			ILoggerFactory loggerFactory,
			HarvesterSettingsOptions harvesterSettingsOptions,
			IHarvesterService harvesterService)
		{
			_logger = loggerFactory.CreateLogger<HarvesterRestarterService>();
			_harvesterSettingsOptions = harvesterSettingsOptions;
			_harvesterService = harvesterService;
		}

		public async Task ProcessService(CancellationToken cancellationToken)
		{
			await _harvesterService.RestartAsync(cancellationToken, _harvesterSettingsOptions.HrvsterRestartMinutes);

			await _harvesterService.DeleteCommandsAsync(cancellationToken);
		}
	}
}
