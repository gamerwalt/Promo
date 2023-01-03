using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Harvesters.Twitter
{
	public interface IHarvesterService
	{
		Task DeleteCommandsAsync(CancellationToken cancellationToken);
		Task RestartAsync(CancellationToken cancellationToken, int numMinutes = 10);
	}

	public class HarvesterService : IHarvesterService
	{
		private readonly ILogger<HarvesterService> _logger;
		private readonly IHarvesterServiceCommandRepository _repository;
		private readonly HttpClient _httpClient;
		private readonly HarvesterSettingsOptions _harvesterSettingsOptions;

		public HarvesterService(
			ILoggerFactory loggerFactory, 
			IHarvesterServiceCommandRepository repository,
			HttpClient httpClient,
			HarvesterSettingsOptions harvesterSettingsOptions)
		{
			_logger = loggerFactory.CreateLogger<HarvesterService>();
			_repository = repository;
			_httpClient = httpClient;
			_harvesterSettingsOptions = harvesterSettingsOptions;
		}

		public async Task DeleteCommandsAsync(CancellationToken cancellationToken)
		{
			await _repository.DeleteAllAsync(cancellationToken);
		}

		public async Task RestartAsync(CancellationToken cancellationToken, int numMinutes = 10)
		{
			_logger.LogInformation("Request received to restart the harvester service.");

			_logger.LogInformation("Retrieving the status of the Harvester Service");

			var harvesterJobResponseMessage = await _httpClient.GetAsync(string.Empty, cancellationToken);

			if(harvesterJobResponseMessage.IsSuccessStatusCode)
			{
				_logger.LogInformation("Success retrieving status of the Harvester Service.");

				var harvesterJobString = await harvesterJobResponseMessage.Content.ReadAsStringAsync();

				_logger.LogInformation("Retrieved the following result. {Result}", harvesterJobString);
				_logger.LogInformation("Deserializing the result received.");

				var webJob = JsonConvert.DeserializeObject<WebjobModel>(harvesterJobString);
				if(webJob != null)
				{
					_logger.LogInformation("Deserialization successful.");

					_logger.LogInformation("Retrieving the commands we have put to restart the web job.");
					var command = await _repository.GetMostRecent(cancellationToken);

					if(webJob.Status == WebJobStatus.Running.ToString() ||
						webJob.Status == WebJobStatus.Stopped.ToString())
					{
						await RunJobAsync(numMinutes, command, cancellationToken);
					}

					return;
				}


				return;
			}
		}

		private async Task RunJobAsync(
			int numMinutes, HarvesterServiceCommandModel command, CancellationToken cancellationToken)
		{
			if (command == null || LastRunGreaterThanMinutes(command.CreatedDate, numMinutes))
			{
				await _httpClient.PostAsync(HarvesterServiceCommand.stop.ToString(), default);

				await Task.Delay(TimeSpan.FromSeconds(5));

				await _httpClient.PostAsync(HarvesterServiceCommand.start.ToString(), default);
				await _repository.CreateAsync(
					new HarvesterServiceCommandModel
					{
						Command = HarvesterServiceCommand.start.ToString(),
						Uid = Guid.NewGuid()
					},
					cancellationToken);
			}
		}

		private bool LastRunGreaterThanMinutes(DateTime createdDate, int minutes)
			=> (createdDate - DateTime.UtcNow).TotalMinutes > minutes;
	}
}
