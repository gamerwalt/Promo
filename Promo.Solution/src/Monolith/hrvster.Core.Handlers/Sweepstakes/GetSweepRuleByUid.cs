using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class GetSweepRuleByUid : IRequestHandler<GetSweepRuleByUidRequest, Rule>
	{
		private readonly ILogger<GetSweepRuleByUid> _logger;
		private readonly ISweepRulesRepository _repository;

		public GetSweepRuleByUid(ILoggerFactory loggerFactory, ISweepRulesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetSweepRuleByUid>();
			_repository = repository;
		}

		public async Task<Rule> Handle(GetSweepRuleByUidRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Getting Sweep Rule for {ruleUid}", request.RuleUid);

			return await _repository.GetAsync(request.RuleUid);
		}
	}
}
