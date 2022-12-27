using MediatR;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class SweepEntriesNotEnoughMailRequest : IRequest
	{
		private string _emailAddress;
		private string _sweepName;

		public SweepEntriesNotEnoughMailRequest(string emailAddress, string sweepName)
		{
			_emailAddress = emailAddress;
			_sweepName = sweepName;
		}

		public string EmailAddress { get { return _emailAddress; } }
		public string SweepName { get { return _sweepName; } }
	}
}
