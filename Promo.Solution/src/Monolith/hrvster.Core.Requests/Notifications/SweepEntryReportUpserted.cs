using MediatR;

namespace hrvster.Core.Requests.Notifications
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
