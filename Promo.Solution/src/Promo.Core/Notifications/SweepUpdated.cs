using MediatR;
using Promo.Core.MessageContracts;

namespace Promo.Core.Notifications
{
	public class SweepUpdated : INotification
	{
		private ISweepUpdated message;

		public SweepUpdated(ISweepUpdated message)
		{
			this.message = message;
		}

		public ISweepUpdated Sweep
		{
			get { return message; }
		}
	}
}
