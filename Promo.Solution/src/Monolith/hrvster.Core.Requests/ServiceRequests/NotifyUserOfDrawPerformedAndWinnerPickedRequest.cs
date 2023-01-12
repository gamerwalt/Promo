using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class NotifyUserOfDrawPerformedAndWinnerPickedRequest : IRequest
	{
		private Sweep _sweep;

		public NotifyUserOfDrawPerformedAndWinnerPickedRequest(Sweep sweep)
		{
			_sweep = sweep;
		}

		public Sweep Sweepstake { get { return _sweep; } }
	}
}
