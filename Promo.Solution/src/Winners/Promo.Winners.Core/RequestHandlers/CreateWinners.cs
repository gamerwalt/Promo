using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Winners.Core.RequestHandlers
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
