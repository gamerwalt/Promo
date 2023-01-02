using FluentValidation;
using hrvster.Infrastructure;
using hrvster.Web.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace hrvster.Web.Validation
{
	public class CreateSweepRuleTweetValidator : AbstractValidator<CreateSweepRuleTweet>
	{
		private readonly ISweepRulesRepository _sweepRulesRepository;

		private const int FiveMegaBytes = 5 * 1024 * 1024;
		//private string[] AllowedExtensions = { ".jpeg", ".jpg", ".png", ".gif", ".mp4" };
		private string[] AllowedExtensions = { ".jpeg", ".JPEG", ".jpg", ".JPG", ".png", ".PNG", ".gif", ".GIF" };

		public CreateSweepRuleTweetValidator(ISweepRulesRepository sweepRulesRepository)
		{
			_sweepRulesRepository = sweepRulesRepository;

			RuleFor(c => c.WatchTypeId)
				.Must(c => c > 0)
				.WithMessage("Please select a watch type for the rule.");

			RuleFor(c => c.WatchTypeId)
				.NotEmpty()
				.WithMessage("Please select a watch type for the rule.")
				.NotNull()
				.WithMessage("Please select a watch type for the rule.");

			RuleFor(c => c.WatchValue)
				.NotEmpty()
				.NotNull()
				.WithMessage("Please supply the watch value that the system should harvest for.");

			RuleFor(c => c.WatchValue)
				.Must(c => MustBeginWithAHash(c))
				.WithMessage("The watch value should begin with a hashtag.");

			RuleFor(c => c.WatchValue)
				.Must(c => MustNotExistAlready(c))
				.WithMessage("We're sorry. That hashtag is already in use.");

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

			RuleFor(c => c.Media1)
				.Must(c => BeOfTheAllowedExtension(c))
				.WithMessage("Media 1's extension is not allowed.");

			RuleFor(c => c.Media1)
				.Must(c => BeOfTheAllowedSize(c))
				.WithMessage("Media 1's size is too large. It has to be less than 5mb in size");

			RuleFor(c => c.Media2)
				.Must(c => BeOfTheAllowedExtension(c))
				.WithMessage("Media 2's extension is not allowed.");

			RuleFor(c => c.Media2)
				.Must(c => BeOfTheAllowedSize(c))
				.WithMessage("Media 2's size is too large. It has to be less than 5mb in size");

			RuleFor(c => c.Media3)
				.Must(c => BeOfTheAllowedExtension(c))
				.WithMessage("Media 3's extension is not allowed.");

			RuleFor(c => c.Media3)
				.Must(c => BeOfTheAllowedSize(c))
				.WithMessage("Media 3's size is too large. It has to be less than 5mb in size");

			RuleFor(c => c.Media4)
				.Must(c => BeOfTheAllowedExtension(c))
				.WithMessage("Media 4's extension is not allowed.");

			RuleFor(c => c.Media4)
				.Must(c => BeOfTheAllowedSize(c))
				.WithMessage("Media 4's size is too large. It has to be less than 5mb in size");
		}

		private bool MustNotExistAlready(string watchValue)
		{
			var existingRecord = _sweepRulesRepository.GetRuleMatching(watchValue);
			
			return !existingRecord.Any(c => c.ActiveInd);
		}

		private bool BeOfTheAllowedSize(IFormFile media)
		{
			if(media != null)
				return media.Length <= FiveMegaBytes;

			return true;
		}

		private bool MustBeginWithAHash(string watchValue)
		{
			return watchValue?.IndexOf("#") == 0;
		}

		private bool BeOfTheAllowedExtension(IFormFile c)
		{
			if(c != null)
			{
				var extension = Path.GetExtension(c.FileName);

				return AllowedExtensions.Contains(extension);
			}

			return true;
		}

		private bool ScheduledTimeShouldBeInTheFuture(CreateSweepRuleTweet model)
		{
			if (!model.PostNow)
				return model.ScheduledDate >= DateTime.Now;

			return true;
		}

		private bool WatchValueShouldBeWithinTweetText(CreateSweepRuleTweet model)
		{
			if(!string.IsNullOrWhiteSpace(model.WatchValue) && !string.IsNullOrWhiteSpace(model.TweetText))
				return model.TweetText.Contains(model.WatchValue);

			return false;
		}

		private bool BeMaximumTweet(string tweet)
		{
			return tweet?.Length <= 140;
		}
	}
}
