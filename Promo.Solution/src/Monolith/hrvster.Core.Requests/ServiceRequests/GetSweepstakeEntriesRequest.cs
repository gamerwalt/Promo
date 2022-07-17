using hrvster.Models;
using MediatR;
using System.Collections.Generic;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class GetSweepstakeEntriesRequest : IRequest<IEnumerable<SweepEntry>>
	{
		private Sweep _sweep;

		public GetSweepstakeEntriesRequest(Sweep sweep)
		{
			_sweep = sweep;
		}

		public Sweep Sweep
		{
			get { return _sweep; }
		}
	}
}
