using hrvster.Core.Requests.WebRequests.Accounts;
using MediatR;

namespace hrvster.Core.Handlers.Account
{
	public class AccountLoggedIn : INotification
	{
		private UpdateLastLoginRequest _request;

		public AccountLoggedIn(UpdateLastLoginRequest request)
		{
			_request = request;
		}

		public UpdateLastLoginRequest Request { get { return _request; } }
	}
}
