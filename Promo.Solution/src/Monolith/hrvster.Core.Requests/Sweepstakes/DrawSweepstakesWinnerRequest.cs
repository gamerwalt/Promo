using hrvster.Models;
using hrvster.Models.Sweepstakes;
using MediatR;

namespace hrvster.Core.Requests.Sweepstakes
{
	public class DrawSweepstakesWinnerRequest : IRequest
	{
		private Sweep _sweep;
		private SweepstakesDrawDetails _drawProperties;

		public DrawSweepstakesWinnerRequest(Sweep sweep, SweepstakesDrawDetails drawProperties)
		{
			_sweep = sweep;
			_drawProperties = drawProperties;
		}

		public Sweep Sweepstake => _sweep;
		public SweepstakesDrawDetails DrawProperties => _drawProperties;
	}
}
