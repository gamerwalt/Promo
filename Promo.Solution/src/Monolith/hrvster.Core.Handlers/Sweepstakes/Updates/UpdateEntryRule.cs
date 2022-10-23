using hrvster.Core.Requests.WebRequests.Sweeps.Updates;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes.Updates
{
	public class UpdateEntryRule : AsyncRequestHandler<UpdateEntryRuleRequest>
	{
		private readonly ILogger<UpdateEntryRule> _logger;
		private readonly ISweepEntryRulesRepository _sweepEntryRulesRepository;

		public UpdateEntryRule(ILoggerFactory loggerFactory, ISweepEntryRulesRepository sweepEntryRulesRepository)
		{
			_logger = loggerFactory.CreateLogger<UpdateEntryRule>();
			_sweepEntryRulesRepository = sweepEntryRulesRepository;
		}

		protected override async Task Handle(UpdateEntryRuleRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Request received to update entry and winner rule request for {sweepName}. Request: {request}",
				request.Sweep.Name,
				request.ToJsonString());

			await _sweepEntryRulesRepository.UpdateSweepEntryRule(new SweepEntryRule
			{
				SweepUid = request.Sweep.SweepUid,
				SweepId = request.Sweep.SweepId,
				EntryRuleUid = request.EntryRuleUid,
				EntryPerUser = request.EntryPerUser
			});
		}
	}
}
