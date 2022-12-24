using FluentValidation;
using hrvster.Core.Authentication;
using hrvster.Infrastructure;
using hrvster.Models.Options;
using hrvster.Web.Models;
using System;
using System.Linq;

namespace hrvster.Web.Validation
{
	public class CreatePromotionValidation : AbstractValidator<CreatePromotion>
	{
		private readonly hrvster.Models.HrvsterUser _hrvsterUser;
		private readonly ICampaignRepository _campaignsRepository;
		private readonly PromotionConfigurationOptions _promotionConfigurationOptions;
		public CreatePromotionValidation(
			hrvster.Models.HrvsterUser hrvsterUser,
			ICampaignRepository campaignsRepository,
			PromotionConfigurationOptions promotionConfigurationOptions)
		{
			_hrvsterUser = hrvsterUser;
			_campaignsRepository = campaignsRepository;
			_promotionConfigurationOptions = promotionConfigurationOptions;

			var minDate = DateTime.Now.AddDays(1);

			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Promotion name cannot be empty.")
				.Must(c => PromotionDoesNotExist(c))
					.WithMessage("The promotion already exists.");

			RuleFor(x => x.StartDate)
				.NotEmpty()
				.NotNull()
				.Must(x => BeGreaterThanConfiguredNumberOfDays(x))
					.WithMessage($"The promotion start date must be from {minDate.ToString("MMM dd, yyyy, hh:mm:ss tt")}");

			RuleFor(x => x)
				.Must(x => x.EndDate > x.StartDate)
					.WithMessage(x => $"The start date {x.StartDate.ToString("MMM dd, yyyy, hh:mm:ss tt")} must come before the end date {x.EndDate.ToString("MMM dd, yyyy, hh:mm:ss tt")}.");

			RuleFor(x => x.CampaignTypeUid)
				.NotEmpty()
				.WithMessage("You need to supply what type of promotion this is.")
				.NotNull()
				.WithMessage("You need to supply what type of promotion this is.");
		}

		private bool BeGreaterThanConfiguredNumberOfDays(DateTime inputDate)
		{
			var minDate = DateTime.Now;

			return inputDate > minDate;
		}

		private bool PromotionDoesNotExist(string promotionName)
		{
			var result = _campaignsRepository.GetAccountCampaigns(_hrvsterUser.UserUid);

			return !result.Any(c => c.CampaignName == promotionName);
		}
	}
}
