using MediatR;
using Promo.Games.Infrastructure.Models;

namespace Promo.Games.Core.Notifications
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
