using hrvster.Core.Requests.WebRequests.Sweeps.Gets;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes.Gets
{
	public class GetSweepEntryRuleForSweep : IRequestHandler<GetSweepEntryRuleForSweepRequest, SweepEntryRule>
	{
		private readonly ILogger<GetSweepEntryRuleForSweep> _logger;
		private readonly ISweepEntryRulesRepository _repository;

		public GetSweepEntryRuleForSweep(ILoggerFactory loggerFactory, ISweepEntryRulesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetSweepEntryRuleForSweep>();
			_repository = repository;
		}

		public async Task<SweepEntryRule> Handle(GetSweepEntryRuleForSweepRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Request to get the sweep entry rule for sweepstake with uid {sweepUid}", request.SweepUid);

			return await _repository.GetSweepEntryRuleAsync(request.SweepUid);
		}
	}
}
