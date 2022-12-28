using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class RemoveDrawAttemptAndNotEnoughUniqueEntrantsRequest : IRequest
	{
		private Sweep _sweep;

		public RemoveDrawAttemptAndNotEnoughUniqueEntrantsRequest(Sweep sweep)
		{
			_sweep = sweep;
		}

		public Sweep Sweep { get { return _sweep; } }
	}
}
