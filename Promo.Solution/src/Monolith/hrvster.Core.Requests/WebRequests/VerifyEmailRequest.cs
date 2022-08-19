using MediatR;

namespace hrvster.Core.Requests.WebRequests
{
	public class VerifyEmailRequest : IRequest
	{
		private string _emailAddress;

		public VerifyEmailRequest(string emailAddress)
		{
			_emailAddress = emailAddress;
		}

		public string EmailAddress { get { return _emailAddress; } }
	}
}
