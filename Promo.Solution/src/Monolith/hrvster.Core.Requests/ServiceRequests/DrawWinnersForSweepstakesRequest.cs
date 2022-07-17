using hrvster.Models;
using hrvster.Models.Sweepstakes;
using MediatR;
using System.Collections.Generic;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class DrawWinnersForSweepstakesRequest : IRequest<List<SweepWinner>>
	{
		private SweepstakesDrawDetails _drawProperties;

		public DrawWinnersForSweepstakesRequest(SweepstakesDrawDetails drawProperties)
		{
			_drawProperties = drawProperties;
		}

		public SweepstakesDrawDetails DrawProperties { get { return _drawProperties; } }
	}
}
