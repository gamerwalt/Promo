using hrvster.Models;
using hrvster.Models.Sweepstakes;
using MediatR;
using System.Collections.Generic;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class DrawBackupWinnersForSweepstakesRequest : IRequest<List<SweepWinner>>
	{
		private SweepstakesDrawDetails _drawProperties;
		private List<SweepWinner> _winners;

		public DrawBackupWinnersForSweepstakesRequest(SweepstakesDrawDetails drawProperties, List<SweepWinner> winners)
		{
			_drawProperties = drawProperties;
			_winners = winners;
		}

		public SweepstakesDrawDetails DrawProperties
		{
			get { return _drawProperties; }
		}

		public List<SweepWinner> Winners { get { return _winners; } }
	}
}
