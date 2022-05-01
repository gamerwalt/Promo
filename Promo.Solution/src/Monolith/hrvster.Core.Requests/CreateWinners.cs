using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class CreateWinners : IRequest
	{
	}

	public class CreateWinnersRequestHandler : IRequestHandler<CreateWinners>
	{
		public async Task<Unit> Handle(CreateWinners request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
