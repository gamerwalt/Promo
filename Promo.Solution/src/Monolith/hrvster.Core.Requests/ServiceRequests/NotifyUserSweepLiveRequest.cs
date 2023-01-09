using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class NotifyUserSweepLiveRequest : IRequest
	{
		private Sweep _sweep;
		private Account _account;

		public NotifyUserSweepLiveRequest(Sweep sweep, Account account)
		{
			_sweep = sweep;
			_account = account;
		}

		public Sweep Sweepstake { get { return _sweep; } }
		public Account Account { get { return _account; } }
	}
}
