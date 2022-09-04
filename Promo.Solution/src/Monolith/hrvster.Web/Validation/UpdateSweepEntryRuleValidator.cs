using FluentValidation;
using hrvster.Web.Models;

namespace hrvster.Web.Validation
{
	public class UpdateSweepEntryRuleValidator : AbstractValidator<UpdateSweepEntryRule>
	{
		public UpdateSweepEntryRuleValidator()
		{
			RuleFor(x => x.EntryPerUser)
				.GreaterThanOrEqualTo(1)
					.WithMessage("Please supply a valid entry that is not less than or equal to 0");
		}
	}
}
