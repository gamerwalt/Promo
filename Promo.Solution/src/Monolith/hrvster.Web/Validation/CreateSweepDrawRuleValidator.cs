using FluentValidation;
using hrvster.Web.Models;

namespace hrvster.Web.Validation
{
	public class CreateSweepDrawRuleValidator : AbstractValidator<CreateSweepDrawRule>
	{
		public CreateSweepDrawRuleValidator()
		{
			RuleFor(x => x.WinnerCount)
				.GreaterThanOrEqualTo(1)
					.WithMessage("Please supply a valid winner count not less than or equal to 0");

			RuleFor(x => x.WinnerBackupCount)
				.GreaterThanOrEqualTo(1)
					.WithMessage("Please supply a valid winner backup count not less than or equal to 0");
		}
	}
}
