using FluentValidation;
using hrvster.Core.Authentication;
using hrvster.Infrastructure;
using hrvster.Web.Models;
using System.Linq;

namespace hrvster.Web.Validation
{
	public class ViewPromotionValidation : AbstractValidator<ViewPromotion>
	{
		private hrvster.Models.HrvsterUser _hrvsterUser;

		private readonly ICampaignRepository _campaignsRepository;
		private readonly ISweepsRepository _sweepsRepository;
		private readonly ISessionManager _sessionManager;

		public ViewPromotionValidation(
			ICampaignRepository campaignsRepository,
			ISweepsRepository sweepsRepository,
			ISessionManager sessionManager)
		{
			_campaignsRepository = campaignsRepository;
			_sweepsRepository = sweepsRepository;
			_sessionManager = sessionManager;

			RuleFor(viewPromotion => viewPromotion.UpdatePromotion.CampaignName)
				.Must((viewPromotion, campaignName) => PromotionDoesNotExist(viewPromotion, campaignName))
				.WithMessage($"A promotion already exists with that name and is still active. You need to Deactivate the promotion.");

			RuleFor(viewPromotion => viewPromotion.UpdatePromotion.CampaignName)
				.NotEmpty().WithMessage("Promotion name cannot be empty.");

			RuleFor(viewPromotion => viewPromotion.UpdatePromotion.CampaignTypeUid)
				.NotNull()
				.NotEmpty().WithMessage("Please select a Promotion type.");

			RuleFor(viewPromotion => viewPromotion.UpdatePromotion)
				.Must(updatePromotion => updatePromotion.EndDate > updatePromotion.StartDate)
					.WithMessage(viewPromotion => $"The start date {viewPromotion.UpdatePromotion.StartDate.ToString("MMM dd, yyyy, hh:mm:ss tt")} must come before the end date {viewPromotion.UpdatePromotion.EndDate.ToString("MMM dd, yyyy, hh:mm:ss tt")}.")
				.Must(updatePromotion => StartDateMustNotComeAfterGames(updatePromotion))
					.WithMessage("The promotion's start date cannot come after any game's start date. Update the start date of your games before updating your promotion's start date.")
				.Must(updatePromotion => EndDateMustNotBeAfterGames(updatePromotion))
					.WithMessage("The promotion's end date cannot be before any game's end date. Update the end date of your games before updating your promotion's end date.");
		}

		private bool EndDateMustNotBeAfterGames(UpdatePromotion updatePromotion)
		{
			var games = _sweepsRepository.GetAllForCampaign(updatePromotion.CampaignUid);

			if (games.Count() == 0) { return true; }

			var result = games.Any(g => g.EndDate < updatePromotion.EndDate);

			return result;
		}

		private bool StartDateMustNotComeAfterGames(UpdatePromotion updatePromotion)
		{
			var games = _sweepsRepository.GetAllForCampaign(updatePromotion.CampaignUid);

			if (games.Count() == 0) { return true; }

			var result = games.Any(g => g.StartDate > updatePromotion.StartDate);

			return result;
		}

		private bool PromotionDoesNotExist(ViewPromotion viewPromotionName, string promotionName)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			var result = _campaignsRepository.GetAccountCampaigns(_hrvsterUser.UserUid);

			var returnResult = result.Any(
				c => c.CampaignName == promotionName &&
				c.CampaignUid != viewPromotionName.UpdatePromotion.CampaignUid &&
				c.ActiveInd);

			return !returnResult;
		}
	}
}
