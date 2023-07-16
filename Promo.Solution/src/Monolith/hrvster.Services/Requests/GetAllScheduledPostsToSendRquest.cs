using hrvster.Models.Scheduling;
using MediatR;
using System;
using System.Collections.Generic;

namespace hrvster.Services.Requests
{
	public class GetAllScheduledPostsToSendRquest : IRequest<IEnumerable<ScheduledPost>>
	{
		private DateTime _currentDate;

		public GetAllScheduledPostsToSendRquest(DateTime currentDate)
		{
			_currentDate = currentDate;
		}

		public DateTime CurrentDate
		{
			get { return _currentDate; }
		}
	}
}
