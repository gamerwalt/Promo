using FluentValidation;
using hrvster.Core.Authentication;
using hrvster.Infrastructure;
using hrvster.Models.Options;
using hrvster.Web.Models;
using System;
using System.Linq;

namespace hrvster.Web.Validation
{
	public class ViewPromotionValidation : AbstractValidator<ViewPromotion>
	{
		private readonly HrvsterUser _hrvsterUser;
		private readonly ICampaignRepository _campaignsRepository;
		private readonly PromotionConfigurationOptions _promotionConfigurationOptions;

		public ViewPromotionValidation(
			HrvsterUser hrvsterUser,
			ICampaignRepository campaignsRepository,
			PromotionConfigurationOptions promotionConfigurationOptions)
		{
			_hrvsterUser = hrvsterUser;
			_campaignsRepository = campaignsRepository;
			_promotionConfigurationOptions = promotionConfigurationOptions;

			var minDate = DateTime.Now.AddDays(_promotionConfigurationOptions.MinDaysBeforeStart);

			RuleFor(viewPromotion => viewPromotion.UpdatePromotion.CampaignName)
				.Must((viewPromotion, campaignName) => PromotionDoesNotExist(viewPromotion, campaignName))
				.WithMessage($"A promotion already exists with that name and is still active. You need to Deactivate the promotion.");

			RuleFor(viewPromotion => viewPromotion.UpdatePromotion.CampaignName)
				.NotEmpty().WithMessage("Promotion name cannot be empty.");

			RuleFor(viewPromotion => viewPromotion.UpdatePromotion.CampaignTypeUid)
				.NotNull()
				.NotEmpty().WithMessage("Please select a Promotion type.");

			RuleFor(viewPromotion => viewPromotion.UpdatePromotion.StartDate)
				.NotEmpty()
				.NotNull()
				.Must(x => BeGreaterThanConfiguredNumberOfDays(x))
					.WithMessage($"The promotion start date must be from {minDate.ToString("f")}");

			RuleFor(viewPromotion => viewPromotion.UpdatePromotion)
				.Must(updatePromotion => updatePromotion.EndDate.ToUniversalTime() > updatePromotion.StartDate.ToUniversalTime())
					.WithMessage(viewPromotion => $"The start date {viewPromotion.UpdatePromotion.StartDate.ToString("f")} must come before the end date {viewPromotion.UpdatePromotion.EndDate.ToString("f")}.");
		}

		private bool BeGreaterThanConfiguredNumberOfDays(DateTime inputDate)
		{
			var minDate = DateTime.Now.ToUniversalTime().AddDays(_promotionConfigurationOptions.MinDaysBeforeStart);

			return inputDate.ToUniversalTime() > minDate;
		}

		private bool PromotionDoesNotExist(ViewPromotion viewPromotionName, string promotionName)
		{
			var result = _campaignsRepository.GetAccountCampaigns(_hrvsterUser.UserUid);

			var returnResult = result.Any(
				c => c.CampaignName == promotionName && 
				c.CampaignUid != viewPromotionName.UpdatePromotion.CampaignUid && 
				c.ActiveInd);

			return !returnResult;
		}
	}
}
