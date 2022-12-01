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
	public class GetDrawRuleForSweeps : IRequestHandler<GetDrawRuleForSweepsRequest, DrawRule>
	{
		private readonly ILogger<GetDrawRuleForSweeps> _logger;
		private readonly ISweepDrawRuleRepository _repository;
		private readonly IMapper _mapper;

		public GetDrawRuleForSweeps(ILoggerFactory loggerFactory, ISweepDrawRuleRepository repository, IMapper mapper)
		{
			_logger = loggerFactory.CreateLogger<GetDrawRuleForSweeps>();
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<DrawRule> Handle(GetDrawRuleForSweepsRequest request, CancellationToken cancellationToken)
		{
			return _mapper.Map<DrawRule>(await _repository.GetSweepDrawRuleAsync(request.SweepUid));
		}
	}
}
