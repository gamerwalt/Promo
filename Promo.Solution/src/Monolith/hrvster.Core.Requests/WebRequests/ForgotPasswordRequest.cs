using MediatR;

namespace hrvster.Core.Requests.WebRequests
{
	public class ForgotPasswordRequest : IRequest
	{
		private string _emailAddress;

		public ForgotPasswordRequest(string emailAddress)
		{
			_emailAddress = emailAddress;
		}

		public string EmailAddress
		{
			get { return _emailAddress; }
		}
	}
}
