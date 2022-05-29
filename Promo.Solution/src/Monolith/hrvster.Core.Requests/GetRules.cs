using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetRules : IRequest<IEnumerable<Rule>>
	{
		private string watchTypeCode;

		public GetRules(string watchTypeCode)
		{
			this.watchTypeCode = watchTypeCode;
		}

		public string WatchTypeCode
		{
			get { return watchTypeCode; }
		}
	}

	public class GetRulesHandler : IRequestHandler<GetRules, IEnumerable<Rule>>
	{
		private readonly ISweepRulesRepository _repository;

		public GetRulesHandler(ISweepRulesRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<Rule>> Handle(GetRules request, CancellationToken cancellationToken)
		{
			return await _repository.GetRules(request.WatchTypeCode);
		}
	}
}
