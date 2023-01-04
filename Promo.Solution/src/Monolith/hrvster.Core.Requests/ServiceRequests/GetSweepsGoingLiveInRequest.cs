using hrvster.Models;
using MediatR;
using System.Collections;
using System.Collections.Generic;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class GetSweepsGoingLiveInRequest : IRequest<IEnumerable<Sweep>>
	{
		private int _hour;

		public GetSweepsGoingLiveInRequest(int hour)
		{
			_hour = hour;
		}

		public int HourValue { get { return _hour; } }
	}
}
