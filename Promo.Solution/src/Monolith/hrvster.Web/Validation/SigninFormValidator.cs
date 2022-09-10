using FluentValidation;
using hrvster.Core.Requests.WebRequests;

namespace hrvster.Web.Validation
{
	public class SigninFormValidator : AbstractValidator<SigninForm>
	{
		public SigninFormValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty()
					.WithMessage("Email address is not valid.")
				.EmailAddress()
					.WithMessage("Email address is not valid.");

			RuleFor(x => x.Password)
				.NotEmpty()
				.WithMessage("Password is not valid.");
		}
	}
}
