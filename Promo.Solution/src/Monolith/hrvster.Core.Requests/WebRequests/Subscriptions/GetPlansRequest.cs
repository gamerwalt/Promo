using hrvster.Models.Subscriptions;
using MediatR;
using System.Collections;
using System.Collections.Generic;

namespace hrvster.Core.Requests.WebRequests.Subscriptions
{
	public class GetPlansRequest : IRequest<IEnumerable<Plan>>
	{
	}
}
