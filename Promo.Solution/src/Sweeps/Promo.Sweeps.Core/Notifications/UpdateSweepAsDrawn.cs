using MediatR;
using System;

namespace Promo.Sweeps.Core.Notifications
{
	public class UpdateSweepAsDrawn : INotification
	{
		private Guid sweepUid;

		public UpdateSweepAsDrawn(Guid sweepUid)
		{
			this.sweepUid = sweepUid;
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}
	}
}
