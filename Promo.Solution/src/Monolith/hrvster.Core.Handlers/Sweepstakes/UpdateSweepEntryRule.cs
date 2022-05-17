using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models;
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
	public class UpdateSweepEntryRule : AsyncRequestHandler<UpdateSweepEntryRuleRequest>
	{
		private readonly ILogger<UpdateSweepEntryRule> _logger;
		private readonly IEntryRulesRepository _repository;

		public UpdateSweepEntryRule(ILoggerFactory loggerFactory, IEntryRulesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<UpdateSweepEntryRule>();
			_repository = repository;
		}

		protected override async Task Handle(UpdateSweepEntryRuleRequest request, CancellationToken cancellationToken)
		{
			await _repository.UpdateSweepEntryRule(new SweepEntryRule
			{
				SweepUid = request.SweepUid,
				SweepId = request.SweepId,
				EntryPerUser = request.SweepEntryPerUser,
				EntryRuleUid = request.SweepEntryRuleUid
			});
		}
	}
}
