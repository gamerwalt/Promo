using MediatR;

namespace hrvster.Core.Requests.Notifications
{
	public class PasswordResetLinkReceived : INotification
	{
		private string _emailAddress;
		private string _resetPasswordLink;

		public PasswordResetLinkReceived(string emailAddress, string resetPasswordLink)
		{
			_emailAddress = emailAddress;
			_resetPasswordLink = resetPasswordLink;
		}

		public string EmailAddress { get { return _emailAddress; } }
		public string ResetPasswordLink { get { return _resetPasswordLink; } }
	}
}
