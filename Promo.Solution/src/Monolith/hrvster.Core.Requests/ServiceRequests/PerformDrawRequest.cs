using hrvster.Models.Sweepstakes;
using MediatR;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class PerformDrawRequest : IRequest
	{
		private SweepstakesDrawDetails _drawProperties;

		public PerformDrawRequest(SweepstakesDrawDetails drawProperties)
		{
			_drawProperties = drawProperties;
		}

		public SweepstakesDrawDetails DrawProperties { get { return _drawProperties; } }
	}
}
