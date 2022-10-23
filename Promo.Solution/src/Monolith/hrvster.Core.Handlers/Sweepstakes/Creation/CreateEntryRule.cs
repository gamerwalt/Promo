using hrvster.Core.Requests.WebRequests.Sweeps.Creation;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes.Creation
{
	public class CreateEntryRule : AsyncRequestHandler<CreateEntryRuleRequest>
	{
		private readonly ILogger<CreateEntryRule> _logger;
		private readonly ISweepEntryRulesRepository _sweepEntryRulesRepository;

		public CreateEntryRule(ILoggerFactory loggerFactory, ISweepEntryRulesRepository sweepEntryRulesRepository)
		{
			_logger = loggerFactory.CreateLogger<CreateEntryRule>();
			_sweepEntryRulesRepository = sweepEntryRulesRepository;
		}

		protected override async Task Handle(CreateEntryRuleRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Request received to create entry rule request for {sweepName}. Request: {request}",
				request.Sweep.Name,
				request.ToJsonString());

			await _sweepEntryRulesRepository.CreateAsync(new SweepEntryRule
			{
				EntryRuleUid = Guid.NewGuid(),
				SweepUid = request.Sweep.SweepUid,
				EntryPerUser = request.EntryPerUser
			});
		}
	}
}
