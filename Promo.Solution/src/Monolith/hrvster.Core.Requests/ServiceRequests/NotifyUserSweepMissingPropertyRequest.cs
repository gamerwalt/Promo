using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class NotifyUserSweepMissingPropertyRequest : IRequest
	{
		private Sweep _sweep;
		private SweepProperty _sweepProperty;
		private Account _account;

		public NotifyUserSweepMissingPropertyRequest(Sweep sweep, SweepProperty sweepProperty, Account account)
		{
			_sweep = sweep;
			_sweepProperty = sweepProperty;
			_account = account;
		}

		public Sweep Sweep { get { return _sweep; } }
		public Account Account { get { return _account; } }
		public SweepProperty SweepProperty { get { return _sweepProperty; } }
	}
}
