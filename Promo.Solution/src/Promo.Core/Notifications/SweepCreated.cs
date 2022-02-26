using MediatR;
using Promo.Core.MessageContracts;

namespace Promo.Core.Notifications
{
	public class SweepCreated : INotification
	{
		private ISweepCreated message;

		public SweepCreated(ISweepCreated message)
		{
			this.message = message;
		}

		public ISweepCreated Sweep
		{
			get { return message; }
		}
	}
}
