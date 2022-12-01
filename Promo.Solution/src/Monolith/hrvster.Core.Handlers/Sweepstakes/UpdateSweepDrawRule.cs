using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class UpdateSweepDrawRule : AsyncRequestHandler<UpdateSweepDrawRuleRequest>
	{
		private readonly ILogger<UpdateSweepDrawRule> _logger;
		private readonly ISweepDrawRuleRepository _repository;
		
		public UpdateSweepDrawRule(ILoggerFactory loggerFactory, ISweepDrawRuleRepository repository)
		{
			_logger = loggerFactory.CreateLogger<UpdateSweepDrawRule>();
			_repository = repository;
		}

		protected async override Task Handle(UpdateSweepDrawRuleRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling updating sweep draw rule for SweepDrawRule with Uid {sweepDrawRuleUid}", request.DrawRuleUid);

			await _repository.UpdateDrawRuleAsync(request.DrawRuleUid, request.WinnerCount, request.WinnerBackupCount);

			_logger.LogInformation("Finished handling updating sweep draw rule for SweepDrawRule with Uid {sweepDrawRuleUid}", request.DrawRuleUid);
		}
	}
}
