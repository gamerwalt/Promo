using hrvster.Core.Handlers.Scheduling;
using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Infrastructure.Scheduling;
using hrvster.Models.Extensions;
using hrvster.Models.Scheduling;
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
		private readonly IScheduledPostRepository _repository;
		public DeleteScheduledSweepRuleTweet(ILoggerFactory loggerFactory, IScheduledPostRepository repository)
		{
			_logger = loggerFactory.CreateLogger<DeleteScheduledSweepRuleTweet>();
			_repository = repository;
		}

		protected override async Task Handle(DeleteScheduledSweepRuleTweetRequest request, CancellationToken cancellationToken)
		{
			try
			{
				var scheduledPost = await _repository.GetAsync(request.ScheduledUid);

				if(scheduledPost == null)
				{
					_logger.LogWarning("Unable to locate the scheduled post with uid {scheduledPostUid}", request.ScheduledUid);
					return;
				}

				await _repository.BeginTransactionAsync();

				await _repository.DeleteScheduledPostAsync(request.ScheduledUid);

				await DeleteRuleBasedOnGameTypeAsync(scheduledPost);

				await _repository.CommitTransactionAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("There was an error handling the action {className} for the content {content}. Failure: {message}.",
					nameof(DeleteScheduledSweepRuleTweet),
					request.ToJsonString(),
					ex.Message);
				_logger.LogError("Stack: {stack}", ex.StackTrace);

				await _repository.RollbackTransactionAsync();
			}
		}

		private Task DeleteRuleBasedOnGameTypeAsync(ScheduledPost scheduledPost)
		{
			switch(scheduledPost.GameType)
			{
				case Models.GameTypesEnum.Sweeps:
					return _repository.DeleteSweepRuleByIdAsync(scheduledPost.Rule.RuleId);
				case Models.GameTypesEnum.MatchGames:
					return _repository.DeleteMatchGameRuleByIdAsync(scheduledPost.Rule.RuleId);
				default:
					return _repository.DeleteSweepRuleByIdAsync(scheduledPost.Rule.RuleId);
			}
		}
	}
}
