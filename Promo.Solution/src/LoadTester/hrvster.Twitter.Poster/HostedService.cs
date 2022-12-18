using hrvster.Twitter.Poster.Requests;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Twitter.Poster
{
	public class HostedService : IHostedService
	{
		private readonly ILogger<HostedService> _logger;
		private readonly IHostApplicationLifetime _appLifeTime;
		private readonly IMediator _mediator;

		public HostedService(
			ILoggerFactory loggerFactory,
			IHostApplicationLifetime appLifeTime,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<HostedService>();
			_appLifeTime = appLifeTime;
			_mediator = mediator;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_appLifeTime.ApplicationStarted.Register(OnStarted);

			_appLifeTime.ApplicationStopping.Register(OnStopping);

			_appLifeTime.ApplicationStopped.Register(OnStopped);
		}

		private async void OnStarted()
		{
			_logger.LogInformation("OnStarted has been called");

			var tasks = new List<Task>();

			Task.Run(() => _mediator.Send(new StartPostingMessageToTwitterRequest())).Wait();

			_appLifeTime.StopApplication();
		}

		private void OnStopped() => _logger.LogInformation("OnStopped has been called");

		private void OnStopping()
		{
			_logger.LogInformation("OnStopping has been called.");
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}
