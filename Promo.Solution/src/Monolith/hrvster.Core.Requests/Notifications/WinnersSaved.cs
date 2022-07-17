using hrvster.Models;
using hrvster.Models.Sweepstakes;
using MediatR;
using System.Collections.Generic;

namespace hrvster.Core.Requests.Notifications
{
	public class WinnersSaved : INotification
	{
		private List<SweepWinner> _winners;
		private SweepstakesDrawDetails _drawProperties;

		public WinnersSaved(List<SweepWinner> winners, SweepstakesDrawDetails drawProperties)
		{
			_winners = winners;
			_drawProperties = drawProperties;
		}

		public List<SweepWinner> Winners
		{
			get { return _winners; }
		}

		public SweepstakesDrawDetails DrawProperties
		{
			get { return _drawProperties; }
		}
	}
}
