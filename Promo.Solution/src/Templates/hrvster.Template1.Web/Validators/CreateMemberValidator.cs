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
				.NotEmpty().WithMessage("Please supply your first name.")
				.NotNull().WithMessage("Please supply your first name.");

			RuleFor(x => x.FirstName)
				.NotEmpty().WithMessage("Please supply your first name.")
				.NotNull().WithMessage("Please supply your first name.");

			RuleFor(x => x.LastName)
				.NotEmpty().WithMessage("Please supply your last name.")
				.NotNull().WithMessage("Please supply your last name.");

			RuleFor(x => x.Address1)
				.NotEmpty().WithMessage("Please supply your Address.")
				.NotNull().WithMessage("Please supply your Address.");

			RuleFor(x => x.PhoneNumber)
				.NotEmpty().WithMessage("Please supply your Phone Number.")
				.NotNull().WithMessage("Please supply your Phone Number.");

			RuleFor(x => x.State)
				.NotEmpty().WithMessage("Please supply your State.")
				.NotNull().WithMessage("Please supply your State.");

			RuleFor(x => x.City)
				.NotEmpty().WithMessage("Please supply your City.")
				.NotNull().WithMessage("Please supply your City.");
		}
	}
}
