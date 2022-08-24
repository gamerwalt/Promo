using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class ActivateSweepRule : AsyncRequestHandler<ActivateSweepRuleRequest>
	{
		private readonly ILogger<DeactivateSweepRule> _logger;
		private readonly ISweepRulesRepository _repository;

		public ActivateSweepRule(ILoggerFactory loggerFactory, ISweepRulesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<DeactivateSweepRule>();
			_repository = repository;
		}

		protected override Task Handle(ActivateSweepRuleRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Deactivating sweep rule. Request: {request}", request.ToJsonString());

			return _repository.ActivateAsync(request.SweepUid, request.RuleUid);
		}
	}
}
