using hrvster.Models.Scheduling;
using MediatR;
using System;
using System.Collections.Generic;

namespace hrvster.Services.Requests
{
	public class GetAllScheduledSweepsRuleTweetsToSendRquest : IRequest<IEnumerable<ScheduledSweepRuleTweet>>
	{
		private DateTime _currentDate;

		public GetAllScheduledSweepsRuleTweetsToSendRquest(DateTime currentDate)
		{
			_currentDate = currentDate;
		}

		public DateTime CurrentDate
		{
			get { return _currentDate; }
		}
	}
}
