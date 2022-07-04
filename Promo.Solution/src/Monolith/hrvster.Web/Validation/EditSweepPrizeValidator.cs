using FluentValidation;
using hrvster.Web.Models;

namespace hrvster.Web.Validation
{
	public class EditSweepPrizeValidator : AbstractValidator<EditSweepPrize>
	{
		public EditSweepPrizeValidator()
		{
			RuleFor(x => x.PrizeName).NotEmpty().NotNull().WithMessage("Please supply a valid prize name.");
			RuleFor(x => x.MonetaryValue).NotEmpty().NotNull().WithMessage("Please supply the value of the prize.");
		}
	}
}
