using FluentValidation;
using hrvster.Core.Authentication;
using hrvster.Infrastructure;
using hrvster.Web.Models;

namespace hrvster.Web.Validation
{
	public class UpdateAccountValidator : AbstractValidator<UpdateAccount>
	{
		private hrvster.Models.HrvsterUser _hrvsterUser;

		private readonly IAccountsRepository _repository;
		private readonly ISessionManager _sessionManager;

		public UpdateAccountValidator(
			ISessionManager sessionManager, 
			IAccountsRepository repository)
		{
			_repository = repository;
			_sessionManager = sessionManager;

			RuleFor(updateAccount => updateAccount.DisplayName)
				.NotEmpty()
					.WithMessage("Please supply a display name.")
				.Must(displayName => MustNotExist(displayName))
					.WithMessage("The display name already exists. Are you sure you don't want to Sign In.");

			RuleFor(x => x.EmailAddress)
				.NotEmpty()
				.NotNull()
				.EmailAddress()
				.WithMessage("The email address must not be empty.")
				.Must(emailAddress => EmailAddressMustNotExist(emailAddress))
					.WithMessage("The email address already exists. Are you sure you don't want to Sign In.");

			RuleFor(x => x.PhoneNumber)
				.NotEmpty()
				.NotNull()
				.WithMessage("The phone number must not be empty.")
				.Must(phoneNumber => PhoneNumberMustNotExist(phoneNumber))
					.WithMessage("The phone number already exists. Are you sure you don't want to Sign In.")
				.Must(phoneNumber => StartWithAPlus(phoneNumber))
					.WithMessage("The phone number must start with a plus sign, then the country code and your phone number.");
		}

		private bool StartWithAPlus(string phone)
		{
			if (string.IsNullOrWhiteSpace(phone)) return false;

			return phone.StartsWith("+");
		}

		private bool EmailAddressMustNotExist(string emailAddress)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			if (string.IsNullOrWhiteSpace(emailAddress)) return false;

			var existingUser = _repository.GetUserByEmail(emailAddress);

			if(existingUser != null)
				return existingUser.AccountUid == _hrvsterUser.UserUid;

			return true;
		}

		private bool PhoneNumberMustNotExist(string phoneNumber)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			if (string.IsNullOrWhiteSpace(phoneNumber)) return false;

			var existingUser = _repository.GetByPhoneNumber(phoneNumber);

			if(existingUser != null)
				return existingUser.AccountUid == _hrvsterUser.UserUid;

			return true;
		}

		private bool DisplayNameMustNotExist(string userName)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			if (string.IsNullOrWhiteSpace(userName)) return false;

			var existingUser = _repository.GetUserByUserName(userName);

			if(existingUser != null)
				return existingUser.AccountUid == _hrvsterUser.UserUid;

			return true;
		}

		private bool MustNotExist(string displayName)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			var account = _repository.GetUserByUserName(displayName);

			if(account != null)
				return account.AccountUid == _hrvsterUser.UserUid;

			return true;
		}
	}
}
