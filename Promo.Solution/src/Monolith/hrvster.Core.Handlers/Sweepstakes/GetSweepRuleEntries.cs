using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class GetSweepRuleEntries : IRequestHandler<GetSweepRuleEntriesRequest, IEnumerable<SweepEntry>>
	{
		private readonly ILogger<GetSweepRuleEntries> _logger;
		private readonly ISweepsEntriesRepository _repository;

		public GetSweepRuleEntries(ILoggerFactory loggerFactory, ISweepsEntriesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetSweepRuleEntries>();
			_repository = repository;
		}

		public async Task<IEnumerable<SweepEntry>> Handle(GetSweepRuleEntriesRequest request, CancellationToken cancellationToken)
		{
			return await _repository.GetSweepEntriesAsync(request.SweepUid);
		}
	}
}
