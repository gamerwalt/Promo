using AutoMapper;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models.WebModels.Sweeps;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class GetSweepEntryRuleByUidHandler : IRequestHandler<GetSweepEntryRuleByUid, EntryRule>
	{
		private readonly ILogger<GetSweepEntryRuleByUidHandler> _logger;
		private readonly IEntryRulesRepository _repository;
		private readonly IMapper _mapper;

		public GetSweepEntryRuleByUidHandler(
			ILoggerFactory loggerFactory,
			IEntryRulesRepository repository,
			IMapper mapper)
		{
			_logger = loggerFactory.CreateLogger<GetSweepEntryRuleByUidHandler>();
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<EntryRule> Handle(GetSweepEntryRuleByUid request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling handler for {handlerName} for sweep entry rule with uid: {sweepEntryRuleUid}",
				nameof(CreateSweepEntryRuleRequestHandler),
				request.SweepEntryUid);

			var entryRule = await _repository.GetAsync(request.SweepEntryUid);

			_logger.LogInformation("Finished handling handler for {handlerName} for sweep entry rule with uid: {sweepEntryRuleUid}",
				nameof(CreateSweepEntryRuleRequestHandler),
				request.SweepEntryUid);

			return _mapper.Map<EntryRule>(entryRule);
		}
	}
}
