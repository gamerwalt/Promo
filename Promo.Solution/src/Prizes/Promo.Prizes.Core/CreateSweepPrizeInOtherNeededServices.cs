using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Promo.Clients.Internal;
using Promo.Prizes.Core.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Prizes.Core
{
	public class CreateSweepPrizeInOtherNeededServices : INotificationHandler<SweepPrizeCreated>
	{
		private ILogger<CreateSweepPrizeInOtherNeededServices> _logger;
		private IHarvesterClient _harvsterClient;

		public CreateSweepPrizeInOtherNeededServices(ILoggerFactory loggerFactory, IHarvesterClient harvesterClient)
		{
			_harvsterClient = harvesterClient;
			_logger = loggerFactory.CreateLogger<CreateSweepPrizeInOtherNeededServices>();
		}

		public async Task Handle(SweepPrizeCreated notification, CancellationToken cancellationToken)
		{
			var sweepPrizeInString = JsonConvert.SerializeObject(notification.SweepPrize);

			_logger.LogInformation("Sending newly created sweep prize {sweepPrize} to the harvester.", sweepPrizeInString);

			await _harvsterClient.CreateNewSweepPrizeAsync(sweepPrizeInString);

			_logger.LogInformation("Sweep prize {sweepPrize} created in the harvester.", sweepPrizeInString);
		}
	}
}
