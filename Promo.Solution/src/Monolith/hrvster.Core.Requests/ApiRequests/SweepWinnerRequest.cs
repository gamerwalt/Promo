using hrvster.Models;
using System;
using System.Collections.Generic;

namespace hrvster.Core.Requests.ApiRequests
{
	public class SweepWinnerRequest
	{
		public List<Guid> SweepWinnersGuids { get; set; }
	}
}
