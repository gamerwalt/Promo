using FirebaseAdmin.Auth;
using MediatR;

namespace Promo.Core.CoreRequestNotifications
{
    public class AccountCreatedViaFirebase : INotification
    {
        private UserRecord _createdUser;
        private string _pin;
        private bool _termsAccepted;

        public AccountCreatedViaFirebase(UserRecord createdUser, string pin, bool termsAccepted)
        {
            _createdUser = createdUser;
            _pin = pin;
            _termsAccepted = termsAccepted;
        }

        public UserRecord CreatedUser => _createdUser;
        public string Pin => _pin;
        public bool TermsAccepted => _termsAccepted;
    }
}
