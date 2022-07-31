using FluentValidation;
using hrvster.Infrastructure;
using hrvster.Web.Models;
using Microsoft.Extensions.Logging;
using System;

namespace hrvster.Web.Validation
{
	public class RegisterValidator : AbstractValidator<Register>
	{
		private readonly ILogger<RegisterValidator> _logger;
		private readonly IAccountsRepository _accountsRepository;
		public RegisterValidator(ILoggerFactory loggerFactory, IAccountsRepository accountsRepository)
		{
			_logger = loggerFactory.CreateLogger<RegisterValidator>();
			_accountsRepository = accountsRepository;

			RuleFor(x => x.EmailAddress)
				.NotEmpty()
				.NotNull()
				.EmailAddress()
				.WithMessage("The email address must not be empty.")
				.Must(x => EmailAddressMustNotExist(x))
					.WithMessage("The email address already exists. Are you sure you don't want to Sign In.");

			RuleFor(x => x.UserName)
				.NotEmpty()
				.NotNull()
				.WithMessage("The username must not be empty.")
				.Must(c => UsernameMustNotExist(c))
					.WithMessage("The username already exists. Are you sure you don't want to Sign In.");

			RuleFor(x => x.PhoneNumber)
				.NotEmpty()
				.NotNull()
				.WithMessage("The phone number must not be empty.")
				.Must(c => PhoneNumberMustNotExist(c))
					.WithMessage("The phone number already exists. Are you sure you don't want to Sign In.")
				.Must(c => StartWithAPlus(c))
					.WithMessage("The phone number must start with a plus sign, then the country code and your phone number.");

			RuleFor(x => x.Password)
				.NotEmpty()
				.NotNull()
				.WithMessage("Please supply a password before continuing.");

			RuleFor(x => x.AcceptedTerms)
				.Must(c => BeAccepted(c))
				.WithMessage("You have to accept the terms and conditions.");
		}

		private bool BeAccepted(bool c)
		{
			return c;
		}

		private bool StartWithAPlus(string phone)
		{
			if (string.IsNullOrWhiteSpace(phone)) return false;

			return phone.StartsWith("+");
		}

		private bool EmailAddressMustNotExist(string emailAddress)
		{
			if (string.IsNullOrWhiteSpace(emailAddress)) return false;

			var existingUser = _accountsRepository.GetUserByEmail(emailAddress);

			return existingUser == null;
		}

		private bool PhoneNumberMustNotExist(string phoneNumber)
		{
			if (string.IsNullOrWhiteSpace(phoneNumber)) return false;

			var existingUser = _accountsRepository.GetByPhoneNumber(phoneNumber);

			return existingUser == null;
		}

		private bool UsernameMustNotExist(string userName)
		{
			if (string.IsNullOrWhiteSpace(userName)) return false;

			var existingUser = _accountsRepository.GetUserByUserName(userName);

			return existingUser == null;
		}
	}
}
