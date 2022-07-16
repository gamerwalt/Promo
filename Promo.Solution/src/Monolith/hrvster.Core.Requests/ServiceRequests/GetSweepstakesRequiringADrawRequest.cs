using hrvster.Models.Sweepstakes;
using MediatR;
using System.Collections.Generic;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class GetSweepstakesRequiringADrawRequest : IRequest<IEnumerable<SweepDrawRule>>
	{
	}
}
