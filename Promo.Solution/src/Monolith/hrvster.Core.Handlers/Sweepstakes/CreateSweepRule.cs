using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class CreateSweepRule : AsyncRequestHandler<CreateSweepRuleRequest>
	{
		private readonly ILogger<CreateSweepRule> _logger;
		private readonly ISweepRulesRepository _repository;

		public CreateSweepRule(
			ILoggerFactory loggerFactory,
			ISweepRulesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepRule>();
			_repository = repository;
		}

		protected override async Task Handle(CreateSweepRuleRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling {className} for SweepUid {sweepUid}",
				nameof(CreateSweepRule),
				request.SweepstakeUid);

			var sweepRule = new Rule
			{
				Sweep = new Sweep
				{
					SweepId = request.SweepstakeId,
					SweepUid = request.SweepstakeUid
				},
				WatchTypeCode = (WatchTypeCode)Convert.ToInt32(request.WatchTypeId-1),
				WatchValue = request.WatchValue,
				RuleUid = Guid.NewGuid(),
				ExternalId = request.TweetId
			};

			await _repository.CreateRuleAsync(sweepRule);

			_logger.LogInformation("Finished handling {className} for SweepUid {sweepUid}",
				nameof(CreateSweepRule),
				request.SweepstakeUid);
		}
	}
}
