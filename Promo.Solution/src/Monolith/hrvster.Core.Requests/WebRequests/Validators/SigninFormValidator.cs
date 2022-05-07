using FluentValidation;

namespace hrvster.Core.Requests.WebRequests.Validators
{
	public class SigninFormValidator : AbstractValidator<SigninForm>
	{
		public SigninFormValidator()
		{
			RuleFor(r => r.Email)
				.EmailAddress().WithMessage("Email Address is not valid.")
				.NotEmpty().WithMessage("Email Address cannot be empty.")
				.NotNull().WithMessage("Email Address cannot be empty.");

			RuleFor(r => r.Password)
				.NotEmpty().WithMessage("Password cannot be empty.")
				.NotNull().WithMessage("Password cannot be empty.");
		}
	}
}
