using FluentValidation;
using hrvster.Web.Models;

namespace hrvster.Web.Validation
{
	public class CreateSweepRuleTweetValidation : AbstractValidator<CreateSweepRuleTweet>
	{
		public CreateSweepRuleTweetValidation()
		{
			RuleFor(x => x.WatchValue)
				.NotNull()
				.NotEmpty().WithMessage("The watch value cannot be empty.");
		}
	}
}
