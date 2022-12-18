using hrvster.Twitter.Poster2.Requests;
using MediatR;

namespace hrvster.Twitter.Poster2
{
	public class Poster2 : IHostedService
	{
		private readonly ILogger<Poster2> _logger;
		private readonly IHostApplicationLifetime _appLifeTime;
		private readonly IMediator _mediator;

		public Poster2(
			ILoggerFactory loggerFactory,
			IHostApplicationLifetime appLifeTime,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<Poster2>();
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