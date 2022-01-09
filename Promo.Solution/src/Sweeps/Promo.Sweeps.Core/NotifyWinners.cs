using MediatR;
using Promo.Sweeps.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Core
{
	public class NotifyWinners : INotificationHandler<WinnerSaved>
	{
		public async Task Handle(WinnerSaved notification, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
