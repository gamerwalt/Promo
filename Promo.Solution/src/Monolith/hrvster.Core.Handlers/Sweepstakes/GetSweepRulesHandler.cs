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
	public class GetSweepRulesHandler : IRequestHandler<GetSweepRules, SweepRule>
	{
		private readonly ILogger<GetSweepRulesHandler> _logger;
		private readonly ISweepsRepository _sweepsRepository;
		private readonly ISweepRulesRepository _rulesRepository;
		private readonly ISweepEntryRulesRepository _entryRulesRepository;
		private readonly IDrawRuleRepository _drawRuleRepository;
		private readonly IMapper _mapper;

		public GetSweepRulesHandler(
			ILoggerFactory loggerFactory,
			ISweepsRepository sweepsRepository,
			ISweepRulesRepository rulesRepository,
			ISweepEntryRulesRepository entryRulesRepository,
			IDrawRuleRepository drawRuleRepository,
			IMapper mapper)
		{
			_logger = loggerFactory.CreateLogger<GetSweepRulesHandler>();
			_sweepsRepository = sweepsRepository;
			_rulesRepository = rulesRepository;
			_entryRulesRepository = entryRulesRepository;
			_drawRuleRepository = drawRuleRepository;
			_mapper = mapper;
		}

		public async Task<SweepRule> Handle(GetSweepRules request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling getting sweep rules for Sweep Uid {sweepUid}", request.SweepUid);

			var sweep = await _sweepsRepository.GetAsync(request.SweepUid);
			var rules = await _rulesRepository.GetSweepRuleAsync(request.SweepUid);
			var entryRule = await _entryRulesRepository.GetSweepEntryRuleAsync(request.SweepUid);
			var drawRule = await _drawRuleRepository.GetSweepDrawRuleAsync(request.SweepUid);

			var sweepRule = new SweepRule
			{
				Sweepstake = _mapper.Map<Sweepstake>(sweep),
				Rules = _mapper.Map<IEnumerable<Rule>>(rules.ToList()),
				EntryRule = _mapper.Map<EntryRule>(entryRule),
				DrawRule = _mapper.Map<DrawRule>(drawRule)
			};

			_logger.LogInformation("Finished handling getting sweep rules for Sweep Uid {sweepUid}", request.SweepUid);

			return sweepRule;
		}
	}
}
