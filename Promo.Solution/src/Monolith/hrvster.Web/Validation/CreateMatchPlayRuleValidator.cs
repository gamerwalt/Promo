using FluentValidation;
using hrvster.Web.Models;

namespace hrvster.Web.Validation
{
	public class CreateMatchPlayRuleValidator : AbstractValidator<CreateMatchPlayRule>
	{
		private const int MaxPlayPerUserPerGame = 50;
		private const int MaxPlayPerUserPerTweet = 3;

		public CreateMatchPlayRuleValidator()
		{
			RuleFor(f => f.PlayPerUser)
				.NotNull()
				.NotEmpty()
				.WithMessage("We need the play per user per game to be more than 0.")
				.LessThan(MaxPlayPerUserPerGame + 1)
				.WithMessage($"The play per game cannot be more than {MaxPlayPerUserPerGame}")
				.GreaterThan(1)
				.WithMessage("The play per game must be greater than 1");
			RuleFor(f => f.PlayPerTweet)
				.NotNull()
				.NotEmpty()
				.WithMessage("We need the play per tweet to be more than 0.")
				.LessThan(MaxPlayPerUserPerTweet + 1)
				.WithMessage($"The play per tweet cannot be more than {MaxPlayPerUserPerTweet}")
				.Must((model, playPerTweet) =>
				{
					return model.PlayPerUser > playPerTweet;
				})
				.WithMessage("The play per game needs to be more than the play per tweet.");
			RuleFor(f => f)
				.Custom((settings, context) =>
				{
					if (settings.PlayPerUser % settings.PlayPerTweet != 0)
						context.AddFailure("The play per tweet must be a multiple of the play per game.");
				});
		}
	}
}
