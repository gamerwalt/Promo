using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers
{
	public class GetSweepProperties : IRequestHandler<GetSweepPropertiesRequest, SweepProperty>
	{
		private readonly ILogger<GetSweepProperties> _logger;
		private readonly ISweepPrizesRepository _sweepPrizesRepository;
		private readonly ISweepDrawRuleRepository _sweepDrawRulesRepository;
		private readonly ISweepRulesRepository _sweepRulesRepository;
		private readonly ISweepEntryRulesRepository _sweepEntryRulesRepository;

		public GetSweepProperties(
			ILoggerFactory loggerFactory,
			ISweepPrizesRepository sweepPrizesRepository,
			ISweepDrawRuleRepository sweepDrawRuleRepository,
			ISweepRulesRepository sweepRulesRepository,
			ISweepEntryRulesRepository sweepEntryRulesRepository)
		{
			_logger = loggerFactory.CreateLogger<GetSweepProperties>();
			_sweepDrawRulesRepository = sweepDrawRuleRepository;
			_sweepPrizesRepository = sweepPrizesRepository;
			_sweepRulesRepository = sweepRulesRepository;
			_sweepEntryRulesRepository = sweepEntryRulesRepository;
		}

		public async Task<SweepProperty> Handle(GetSweepPropertiesRequest request, CancellationToken cancellationToken)
		{
			var property = new SweepProperty();

			var prizesTask = _sweepPrizesRepository.GetSweepPrizesAsync(request.SweepUid);
			var drawRuleTask = _sweepDrawRulesRepository.GetSweepDrawRuleAsync(request.SweepUid);
			var rulesTask = _sweepRulesRepository.GetSweepRuleAsync(request.SweepUid);
			var entryRulesTask = _sweepEntryRulesRepository.GetSweepEntryRuleAsync(request.SweepUid);

			var tasks = new List<Task>();
			tasks.Add(prizesTask);
			tasks.Add(drawRuleTask);
			tasks.Add(rulesTask);
			tasks.Add(entryRulesTask);

			await Task.WhenAll(tasks);

			property.DrawRule = drawRuleTask.Result;
			property.Rules = rulesTask.Result;
			property.EntryRule = entryRulesTask.Result;
			property.Prizes = prizesTask.Result;

			return property;
		}
	}
}
