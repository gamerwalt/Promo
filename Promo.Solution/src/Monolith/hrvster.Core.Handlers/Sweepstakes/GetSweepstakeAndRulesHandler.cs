using AutoMapper;
using hrvster.Core.Requests;
using hrvster.Infrastructure;
using hrvster.Models.WebModels.Sweeps;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class GetSweepstakeAndRulesHandler : IRequestHandler<GetSweepstakeAndRulesRequests, SweepstakeAndRules>
	{
		private readonly ILogger<GetSweepstakeAndRulesHandler> _logger;
		private readonly ISweepsRepository _sweepsRepository;
		private readonly ISweepRulesRepository _rulesRepository;
		private readonly ISweepEntryRulesRepository _entryRulesRepository;
		private readonly IDrawRuleRepository _drawRuleRepository;
		private readonly IMapper _mapper;

		public GetSweepstakeAndRulesHandler(
			ILoggerFactory loggerFactory,
			ISweepsRepository sweepsRepository,
			ISweepRulesRepository rulesRepository,
			ISweepEntryRulesRepository entryRulesRepository,
			IDrawRuleRepository drawRuleRepository,
			IMapper mapper)
		{
			_logger = loggerFactory.CreateLogger<GetSweepstakeAndRulesHandler>();
			_sweepsRepository = sweepsRepository;
			_rulesRepository = rulesRepository;
			_entryRulesRepository = entryRulesRepository;
			_drawRuleRepository = drawRuleRepository;
			_mapper = mapper;
		}

		public async Task<SweepstakeAndRules> Handle(GetSweepstakeAndRulesRequests request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling getting sweep rules for Sweep Uid {sweepUid}", request.SweepUid);

			var tasks = new List<Task>();
			var sweepTask = _sweepsRepository.GetAsync(request.SweepUid);
			var ruleTask = _rulesRepository.GetSweepRuleAsync(request.SweepUid);
			var entryRuleTask = _entryRulesRepository.GetSweepEntryRuleAsync(request.SweepUid);
			var drawRuleTask = _drawRuleRepository.GetSweepDrawRuleAsync(request.SweepUid);
			tasks.Add(sweepTask);
			tasks.Add(ruleTask);
			tasks.Add(entryRuleTask);
			tasks.Add(drawRuleTask);

			await Task.WhenAll(tasks);

			var sweepRule = new SweepstakeAndRules
			{
				Sweepstake = _mapper.Map<Sweepstake>(sweepTask.Result),
				Rules = _mapper.Map<IEnumerable<Rule>>(ruleTask.Result.ToList()),
				EntryRule = _mapper.Map<EntryRule>(entryRuleTask.Result),
				DrawRule = _mapper.Map<DrawRule>(drawRuleTask.Result)
			};

			_logger.LogInformation("Finished handling getting sweep rules for Sweep Uid {sweepUid}", request.SweepUid);

			return sweepRule;
		}
	}
}
