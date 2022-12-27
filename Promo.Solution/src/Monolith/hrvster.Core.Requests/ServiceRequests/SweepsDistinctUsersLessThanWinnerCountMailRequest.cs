using MediatR;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class SweepsDistinctUsersLessThanWinnerCountMailRequest : IRequest
	{
		private string _emailAddress;
		private string _sweepstakeName;

		public SweepsDistinctUsersLessThanWinnerCountMailRequest(string emailAddress, string sweepstakeName)
		{
			_emailAddress = emailAddress;
			_sweepstakeName = sweepstakeName;
		}

		public string EmailAddress { get { return _emailAddress; } }
		public string SweepstakeName { get { return _sweepstakeName; } }
	}
}
