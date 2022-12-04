using MediatR;
using System;

namespace hrvster.Core.Requests.Notifications
{
	public class AccountCreatedViaFirebase : INotification
	{
		private string _pin;
		private bool _termsAccepted;
		private string _verificationLink;
		private string _emailAddress;
		private Guid _userUid;
		private string _phoneNumber;
		private string _username;
		private string _firstName;
		private string _lastName;

		public AccountCreatedViaFirebase(string emailAddress, Guid userUid, string phoneNumber, string username, string firstName, string lastName, string defaultPin, bool acceptedTerms, string verificationLink)
		{
			_emailAddress = emailAddress;
			_userUid = userUid;
			_phoneNumber = phoneNumber;
			_username = username;
			_firstName = firstName;
			_lastName = lastName;
			_pin = defaultPin;
			_termsAccepted = acceptedTerms;
			_verificationLink = verificationLink;
		}

		public string EmailAddress => _emailAddress;
		public Guid UserUid => _userUid;
		public string PhoneNumber => _phoneNumber;
		public string UserName => _username;
		public string FirstName => _firstName;
		public string LastName => _lastName;
		public string Pin => _pin;
		public bool TermsAccepted => _termsAccepted;
		public string VerificationLink => _verificationLink;
	}
}
