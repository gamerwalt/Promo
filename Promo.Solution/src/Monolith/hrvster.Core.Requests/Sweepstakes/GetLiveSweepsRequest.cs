using hrvster.Models;
using MediatR;
using System.Collections.Generic;

namespace hrvster.Core.Requests.Sweepstakes
{
	public class GetLiveSweepsRequest : IRequest<IEnumerable<Sweep>>
	{
	}
}
