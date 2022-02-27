using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.Notifications
{
	public class SweepCreated : INotification
	{
		private Sweep sweep;

		public SweepCreated(Sweep sweep)
		{
			this.sweep = sweep;
		}

		public Sweep Sweep
		{
			get { return sweep; }
		}
	}
}
