using FirebaseAdmin.Auth;
using MediatR;

namespace hrvster.Core.Requests.Notifications
{
    public class AccountCreatedViaFirebase : INotification
    {
        private UserRecord _createdUser;
        private string _pin;
        private bool _termsAccepted;
		private string _verificationLink;

        public AccountCreatedViaFirebase(UserRecord createdUser, string pin, bool termsAccepted, string verificationLink)
        {
            _createdUser = createdUser;
            _pin = pin;
            _termsAccepted = termsAccepted;
			_verificationLink = verificationLink;
        }

        public UserRecord CreatedUser => _createdUser;
        public string Pin => _pin;
        public bool TermsAccepted => _termsAccepted;
		public string VerificationLink => _verificationLink;
    }
}
