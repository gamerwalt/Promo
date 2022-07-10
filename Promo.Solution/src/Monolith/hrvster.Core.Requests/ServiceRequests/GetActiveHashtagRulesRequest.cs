using hrvster.Models;
using MediatR;
using System.Collections.Generic;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class GetActiveHashtagRulesRequest : IRequest<IEnumerable<Rule>>
	{
	}
}
