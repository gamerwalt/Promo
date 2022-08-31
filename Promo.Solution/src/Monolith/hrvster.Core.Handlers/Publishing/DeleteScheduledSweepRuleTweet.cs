using hrvster.Core.Handlers.Scheduling;
using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Infrastructure.Scheduling;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Publishing
{
	public class DeleteScheduledSweepRuleTweet : AsyncRequestHandler<DeleteScheduledSweepRuleTweetRequest>
	{
		private readonly ILogger<DeleteScheduledSweepRuleTweet> _logger;
		private readonly ISweepTweetSchedulerRepository _repository;
		public DeleteScheduledSweepRuleTweet(ILoggerFactory loggerFactory, ISweepTweetSchedulerRepository repository)
		{
			_logger = loggerFactory.CreateLogger<DeleteScheduledSweepRuleTweet>();
			_repository = repository;
		}

		protected override async Task Handle(DeleteScheduledSweepRuleTweetRequest request, CancellationToken cancellationToken)
		{
			try
			{
				await _repository.BeginTransaction();

				await _repository.DeleteScheduledSweepRuleTweetAsync(request.ScheduledUid);

				await _repository.DeleteRuleAsync(request.RuleUid);

				await _repository.CommitTransaction();
			}
			catch (Exception ex)
			{
				_logger.LogError("There was an error handling the action {className} for the content {content}. Failure: {message}.",
					nameof(DeleteScheduledSweepRuleTweet),
					request.ToJsonString(),
					ex.Message);
				_logger.LogError("Stack: {stack}", ex.StackTrace);

				await _repository.RollbackTransaction();
			}
		}
	}
}
