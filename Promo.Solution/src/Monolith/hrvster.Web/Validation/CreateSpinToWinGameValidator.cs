using FluentValidation;
using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.SpinToWin;
using hrvster.Core.Authentication;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Web.Models.SpinToWin;
using System;
using System.Linq;

namespace hrvster.Web.Validation
{
	public class CreateSpinToWinGameValidator : AbstractValidator<CreateSpinToWinGame>
	{
		private readonly ISessionManager _sessionManager;
		private readonly ICampaignRepository _campaignRepository;
		private readonly ISpinToWinService _spinToWinService;

		public CreateSpinToWinGameValidator(
			ISessionManager sessionManager,
			ICampaignRepository campaignRepository,
			ISpinToWinService spinToWinService)
		{
			_sessionManager = sessionManager;
			_spinToWinService = spinToWinService;
			_campaignRepository = campaignRepository;

			RuleFor(x => x.Name)
				.NotNull()
				.WithMessage("We need a Spin To Win game name.")
				.NotEmpty()
				.WithMessage("We need a Spin To Win game name.");

			RuleFor(c => c.Name)
				.Must(name => ShouldNotExistAlready(name))
				.WithMessage("The match game name already exists.");

			RuleFor(x => x.PromotionUid)
				.NotNull()
				.NotEmpty()
				.WithMessage("You need to select a promotion.");

			RuleFor(f => f.PromotionUid)
				.Must(p => PromotionShouldExist(p))
				.WithMessage("The promotion does not exist.");
		}

		private bool PromotionShouldExist(Guid promotionUid)
		{
			var hrvsterUser = _sessionManager.GetUserInformation();

			var promotionsForAccont = _campaignRepository.GetAccountCampaigns(hrvsterUser.UserUid);

			return promotionsForAccont.Exists(p => p.CampaignUid == promotionUid);
		}

		private bool ShouldNotExistAlready(string name)
		{
			var hrvsterUser = _sessionManager.GetUserInformation();

			var games = _spinToWinService.GetAllForAccount(hrvsterUser.UserUid);

			var result = games.Any(s => s.Name == name);

			return !result;
		}
	}
}
