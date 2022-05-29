using hrvster.Models.WebModels;
using MediatR;
using System.Collections.Generic;

namespace hrvster.Core.Requests.WebRequests.Core
{
	public class GetWatchTypeRequest : IRequest<IEnumerable<WatchType>>
	{
	}
}
