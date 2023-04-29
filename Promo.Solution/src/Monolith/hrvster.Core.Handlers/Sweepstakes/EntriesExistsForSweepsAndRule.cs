using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class EntriesExistsForSweepsAndRule : IRequestHandler<EntriesExistsForSweepsAndRuleRequest, bool>
	{
		private readonly ILogger<EntriesExistsForSweepsAndRule> _logger;
		private readonly ISweepsEntriesRepository _repository;

		public EntriesExistsForSweepsAndRule(ILoggerFactory loggerFactory, ISweepsEntriesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<EntriesExistsForSweepsAndRule>();
			_repository = repository;
		}


		public async Task<bool> Handle(EntriesExistsForSweepsAndRuleRequest request, CancellationToken cancellationToken)
		{
			var entries = await _repository.GetSweepEntriesAsync(request.SweepUid);

			foreach (var entry in entries)
			{
				if (entry.Rule?.RuleUid == request.RuleUid)
					return true;
			}

			return false;
		}
	}
}
