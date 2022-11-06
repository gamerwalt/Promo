using FluentValidation;
using hrvster.Web.Models;
using System;

namespace hrvster.Web.Validation
{
	public class CreateWatchValidator : AbstractValidator<CreateWatch>
	{
		public CreateWatchValidator()
		{
			RuleFor(c => c.WatchTypeId)
				.Must(c => c > 0)
				.WithMessage("Please select a watch type for the rule.");

			RuleFor(c => c.WatchTypeId)
				.NotEmpty()
				.WithMessage("Please select a watch type for the rule.")
				.NotNull()
				.WithMessage("Please select a watch type for the rule.");

			RuleFor(c => c.Hashtag)
				.NotEmpty()
				.NotNull()
				.WithMessage("Please supply the hashtag/watch value that the system should harvest.");

			RuleFor(c => c.Hashtag)
				.Must(c => MustBeginWithAHash(c))
				.WithMessage("The watch value should begin with a hashtag.");

			RuleFor(c => c.TweetText)
				.NotEmpty()
				.NotNull()
				.WithMessage("Please supply the tweet text.");

			RuleFor(c => c.TweetText)
				.Must(c => BeMaximumTweet(c))
				.WithMessage("The text cannot go beyond 140.");

			RuleFor(c => c)
				.Must(c => WatchValueShouldBeWithinTweetText(c))
				.WithMessage("The tweet text should contain the watch value.");

			RuleFor(c => c)
				.Must(c => ScheduledTimeShouldBeInTheFuture(c))
				.WithMessage("The scheduled date for this tweet should be in the future.");
		}

		private bool ScheduledTimeShouldBeInTheFuture(CreateWatch model)
		{
			if (!model.PostNow)
				return model.ScheduledDate.ToUniversalTime() >= DateTime.UtcNow;

			return true;
		}

		private bool WatchValueShouldBeWithinTweetText(CreateWatch model)
		{
			return model.TweetText.Contains(model.Hashtag);
		}

		private bool MustBeginWithAHash(string watchValue)
		{
			return watchValue.IndexOf("#") == 0;
		}

		private bool BeMaximumTweet(string tweet)
		{
			return tweet.Length <= 140;
		}
	}
}
