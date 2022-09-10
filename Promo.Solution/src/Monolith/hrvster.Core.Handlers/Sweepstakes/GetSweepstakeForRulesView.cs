using AutoMapper;
using hrvster.Core.Requests.WebRequests;
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
	public class GetSweepstakeForRulesView
		: IRequestHandler<GetSweepstakeForRulesViewRequest, SweepstakeRulesAndPrizes>
	{
		private readonly ILogger _logger;
		private readonly ISweepsRepository _sweepsRepository;
		private readonly ISweepRulesRepository _rulesRepository;
		private readonly ISweepEntryRulesRepository _entryRulesRepository;
		private readonly IDrawRuleRepository _drawRuleRepository;
		private readonly ISweepPrizesRepository _sweepPrizesRepository;
		private readonly IMapper _mapper;

		public async Task<SweepstakeRulesAndPrizes> Handle(GetSweepstakeForRulesViewRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling getting sweep rules and prizes for Sweep Uid {sweepUid}",
				request.SweepUid);

			var tasks = new List<Task>();
			var sweepTask = _sweepsRepository.GetAsync(request.SweepUid);
			var ruleTask = _rulesRepository.GetSweepRuleAsync(request.SweepUid);
			var entryRuleTask = _entryRulesRepository.GetSweepEntryRuleAsync(request.SweepUid);
			var drawRuleTask = _drawRuleRepository.GetDrawRuleAsync(request.SweepUid);
			var prizesTask = _sweepPrizesRepository.GetSweepPrizesAsync(request.SweepUid);

			tasks.Add(sweepTask);
			tasks.Add(ruleTask);
			tasks.Add(entryRuleTask);
			tasks.Add(drawRuleTask);
			tasks.Add(prizesTask);

			var sweepRuleAndPrizes = default(SweepstakeRulesAndPrizes);

			if (Task.WhenAll(tasks).IsCompleted)
			{
				sweepRuleAndPrizes = new SweepstakeRulesAndPrizes
				{
					Sweepstake = _mapper.Map<Sweepstake>(sweepTask.Result),
					Rules = _mapper.Map<IEnumerable<Rule>>(ruleTask.Result.ToList()),
					EntryRule = _mapper.Map<EntryRule>(entryRuleTask.Result),
					DrawRule = _mapper.Map<DrawRule>(drawRuleTask.Result),
					Prizes = _mapper.Map<IEnumerable<SweepPrize>>(prizesTask.Result.ToList())
				};
			}

			_logger.LogInformation("Finished handling getting sweep rules and prizes for Sweep Uid {sweepUid}",
				request.SweepUid);

			return sweepRuleAndPrizes;
		}
	}
}
