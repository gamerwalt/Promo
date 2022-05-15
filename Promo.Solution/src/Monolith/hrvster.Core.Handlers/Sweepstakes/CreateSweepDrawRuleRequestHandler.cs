using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class CreateSweepDrawRuleRequestHandler : AsyncRequestHandler<CreateSweepDrawRuleRequest>
	{
		private readonly ILogger<CreateSweepDrawRuleRequestHandler> _logger;
		private readonly IDrawRuleRepository _repository;

		public CreateSweepDrawRuleRequestHandler(
			ILoggerFactory loggerFactory,
			IDrawRuleRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepDrawRuleRequestHandler>();
			_repository = repository;
		}

		protected async override Task Handle(CreateSweepDrawRuleRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling handler for {handlerName} for sweep with uid: {sweepUid}",
				nameof(CreateSweepEntryRuleRequestHandler),
				request.SweepUid);

			await _repository.Create(new Models.DrawRule
			{
				SweepDrawRuleUid = Guid.NewGuid(),
				SweepUid = request.SweepUid,
				WinnerCount = request.WinnerCount,
				WinnerBackupCount = request.WinnerBackupCount
			});

			_logger.LogInformation("Finished handling handler for {handlerName} for sweep with uid: {sweepUid}",
				nameof(CreateSweepEntryRuleRequestHandler),
				request.SweepUid);
		}
	}
}
