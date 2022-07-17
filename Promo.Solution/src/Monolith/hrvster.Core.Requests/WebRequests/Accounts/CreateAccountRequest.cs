using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Accounts
{
	public class CreateAccountRequest : IRequest
	{
		private Guid _userUid;
		private string _emailAddress;
		private string _userName;
		private string _phoneNumber;
		private string _firstName;
		private string _lastName;
		private string _password;
		private bool _acceptedTerms;

		public CreateAccountRequest(Guid userUid, string emailAddress, string userName, string phoneNumber, string firstName, string lastName, string password, bool acceptedTerms)
		{
			_userUid = userUid;
			_emailAddress = emailAddress;
			_userName = userName;
			_phoneNumber = phoneNumber;
			_firstName = firstName;
			_lastName = lastName;
			_password = password;
			_acceptedTerms = acceptedTerms;
		}

		public Guid UserUid { get { return _userUid; } }
		public string EmailAddress { get { return _emailAddress; } }
		public string Username { get { return _userName; } }
		public string PhoneNumber { get { return _phoneNumber; } }
		public string FirstName { get { return _firstName; } }
		public string LastName { get { return _lastName; } }
		public string Password { get { return _password; } }
		public bool AcceptedTerms { get { return _acceptedTerms; } }
	}
}
