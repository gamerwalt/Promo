using hrvster.Services.Requests;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Services
{
	public class ScheduledTweetService : IService
	{
		private readonly ILogger<ScheduledTweetService> _logger;

		private readonly IMediator _mediator;

		public ScheduledTweetService(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<ScheduledTweetService>();
			_mediator = mediator;
		}

		public async Task ProcessService(System.Threading.CancellationToken token)
		{
			_logger.LogInformation("Starting {serviceName}", nameof(ScheduledTweetService));

			var scheduledTweets = await _mediator.Send(new GetAllScheduledSweepsRuleTweetsToSendRquest(DateTime.UtcNow));

			if (scheduledTweets.Count() > 0)
			{
				await _mediator.Send(new PublishScheduledTweetsRequest(scheduledTweets));
			}
		}
	}
}
