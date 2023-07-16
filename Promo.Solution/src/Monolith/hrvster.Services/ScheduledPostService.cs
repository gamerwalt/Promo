using hrvster.Services.Requests;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace hrvster.Services
{
	public class ScheduledPostService : IService
	{
		private readonly ILogger<ScheduledPostService> _logger;

		private readonly IMediator _mediator;

		public ScheduledPostService(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<ScheduledPostService>();
			_mediator = mediator;
		}

		public async Task ProcessService(System.Threading.CancellationToken token)
		{
			_logger.LogInformation("Starting {serviceName}", nameof(ScheduledPostService));

			var scheduledPosts = await _mediator.Send(new GetAllScheduledPostsToSendRquest(DateTime.UtcNow));

			if (scheduledPosts.Any())
			{
				await _mediator.Send(new PublishScheduledPostsRequest(scheduledPosts));
			}
			else
			{
				_logger.LogInformation("There are no scheduled posts to be sent.");
			}
		}
	}
}
