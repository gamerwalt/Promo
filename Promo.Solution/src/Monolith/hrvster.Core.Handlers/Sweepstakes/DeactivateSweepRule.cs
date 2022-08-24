using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class DeactivateSweepRule : AsyncRequestHandler<DeactivateSweepRuleRequest>
	{
		private readonly ILogger<DeactivateSweepRule> _logger;
		private readonly ISweepRulesRepository _repository;

		public DeactivateSweepRule(ILoggerFactory loggerFactory, ISweepRulesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<DeactivateSweepRule>();
			_repository = repository;
		}

		protected override Task Handle(DeactivateSweepRuleRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Deactivating sweep rule. Request: {request}", request.ToJsonString());

			return _repository.DeactivateAsync(request.SweepUid, request.RuleUid);
		}
	}
}
