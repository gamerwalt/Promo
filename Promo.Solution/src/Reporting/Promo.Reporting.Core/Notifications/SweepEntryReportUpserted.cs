using MediatR;
using Promo.Reporting.Core.RequestHandlers;

namespace Promo.Reporting.Core.Notifications
{
	public class SweepEntryReportUpserted : INotification
	{
		private AddSweepEntryReport request;

		public SweepEntryReportUpserted(AddSweepEntryReport request)
		{
			this.request = request;
		}

		public AddSweepEntryReport Request
		{
			get { return request; }
		}
	}
}
