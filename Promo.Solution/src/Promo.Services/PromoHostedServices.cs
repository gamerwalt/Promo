using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.RabbitMq.Consumer.Services
{
	public class PromoHostedServices : IHostedService
	{
		private readonly ILogger<PromoHostedServices> _logger;
		private readonly IHostApplicationLifetime _appLifeTime;
		private readonly IBusControl _busControl;
		private readonly CancellationTokenSource _cancellationTokenSource;

		public PromoHostedServices(
			ILogger<PromoHostedServices> logger,
			IHostApplicationLifetime appLifeTime,
			IBusControl busControl)
		{
			_logger = logger;
			_appLifeTime = appLifeTime;
			_busControl = busControl;

			_cancellationTokenSource = new CancellationTokenSource();
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await _busControl.StartAsync(cancellationToken).ConfigureAwait(false);
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}
