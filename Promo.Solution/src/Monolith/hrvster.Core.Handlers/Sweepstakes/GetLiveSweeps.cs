using hrvster.Core.Requests.Sweepstakes;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class GetLiveSweeps : IRequestHandler<GetLiveSweepsRequest, IEnumerable<Sweep>>
	{
		private readonly ISweepsRepository _repository;

		public GetLiveSweeps(ISweepsRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<Sweep>> Handle(GetLiveSweepsRequest request, CancellationToken cancellationToken)
			=> await _repository.GetLiveSweepsThatHaveNotBeenNotifiedAsync(cancellationToken);
	}
}
