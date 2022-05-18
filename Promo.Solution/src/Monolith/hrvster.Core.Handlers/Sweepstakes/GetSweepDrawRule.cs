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
	public class GetSweepDrawRule : IRequestHandler<GetSweepDrawRuleRequest, DrawRule>
	{
		private readonly ILogger<GetSweepDrawRule> _logger;
		private readonly IDrawRuleRepository _repository;
		private readonly IMapper _mapper;

		public GetSweepDrawRule(
			ILoggerFactory loggerFactory,
			IDrawRuleRepository repository,
			IMapper mapper)
		{
			_logger = loggerFactory.CreateLogger<GetSweepDrawRule>();
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<DrawRule> Handle(GetSweepDrawRuleRequest request, CancellationToken cancellationToken)
		{
			var drawRule = await _repository.GetDrawRuleAsync(request.DrawRuleUid);

			return _mapper.Map<DrawRule>(drawRule);
		}
	}
}
