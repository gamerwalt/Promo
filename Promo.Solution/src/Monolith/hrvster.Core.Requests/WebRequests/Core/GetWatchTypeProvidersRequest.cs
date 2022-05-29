using hrvster.Models.Core;
using MediatR;
using System.Collections.Generic;

namespace hrvster.Core.Requests.WebRequests.Core
{
	public class GetWatchTypeProvidersRequest : IRequest<IEnumerable<WatchTypeProvider>>
	{
	}
}
