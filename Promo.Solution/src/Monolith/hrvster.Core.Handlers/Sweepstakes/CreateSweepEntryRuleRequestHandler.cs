using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class CreateSweepEntryRuleRequestHandler : AsyncRequestHandler<CreateSweepEntryRuleRequest>
	{
		private readonly ILogger<CreateSweepEntryRuleRequestHandler> _logger;
		private readonly IEntryRulesRepository _repository;

		public CreateSweepEntryRuleRequestHandler(
			ILoggerFactory loggerFactory,
			IEntryRulesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepEntryRuleRequestHandler>();
			_repository = repository;
		}

		protected override async Task Handle(CreateSweepEntryRuleRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling handler for {handlerName} for sweep with uid: {sweepUid}",
				nameof(CreateSweepEntryRuleRequestHandler),
				request.SweepUid);

			await _repository.CreateAsync(new Models.SweepEntryRule
			{
				EntryRuleUid = Guid.NewGuid(),
				SweepUid = request.SweepUid,
				EntryPerUser = request.EntryPerUser
			});

			_logger.LogInformation("Finished handling handler for {handlerName} for sweep with uid: {sweepUid}",
				nameof(CreateSweepEntryRuleRequestHandler),
				request.SweepUid);
		}
	}
}
