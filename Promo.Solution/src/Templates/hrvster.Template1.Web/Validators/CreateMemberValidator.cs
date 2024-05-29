using FluentValidation;
using hrvster.Template1.Web.Models;

namespace hrvster.Template1.Web.Validators
{
	public class CreateMemberValidator : AbstractValidator<CreateMember>
	{
		public CreateMemberValidator()
		{
			RuleFor(x => x.EmailAddress)
				.EmailAddress().WithMessage("Please supply a valid email address.")
				.NotEmpty().WithMessage("Please supply your email address.")
				.NotNull().WithMessage("Please supply your email address.");

			RuleFor(x => x.FirstName)
				.NotEmpty().WithMessage("Please supply your first name.")
				.NotNull().WithMessage("Please supply your first name.");

			RuleFor(x => x.LastName)
				.NotEmpty().WithMessage("Please supply your last name.")
				.NotNull().WithMessage("Please supply your last name.");

			RuleFor(x => x.PhoneNumber)
				.NotEmpty().WithMessage("Please supply your Phone Number.")
				.NotNull().WithMessage("Please supply your Phone Number.");
		}
	}
}
