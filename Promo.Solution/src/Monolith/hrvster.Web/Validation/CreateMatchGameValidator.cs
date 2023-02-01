using FluentValidation;
using hrvster.Core.Authentication;
using hrvster.Infrastructure;
using hrvster.Infrastructure.MatchGames;
using hrvster.Web.Models;
using System;
using System.Linq;

namespace hrvster.Web.Validation
{
	public class CreateMatchGameValidator : AbstractValidator<CreateMatchGame>
	{
		private readonly ISessionManager _sessionManager;
		private readonly IMatchGamesRepository _matchGamesRepository;
		private readonly ICampaignRepository _campaignRepository;

		private hrvster.Models.HrvsterUser _hrvsterUser;

		public CreateMatchGameValidator(
			ISessionManager sessionManager,
			IMatchGamesRepository matchGamesRepository,
			ICampaignRepository campaignRepository)
		{
			_sessionManager = sessionManager;
			_matchGamesRepository = matchGamesRepository;
			_campaignRepository = campaignRepository;

			RuleFor(f => f.PromotionUid)
				.NotEmpty()
				.WithMessage("You need to select a promotion.")
				.NotNull()
				.WithMessage("You need to select a promotion.");

			RuleFor(f => f.PromotionUid)
				.Must(p => PromotionShouldExist(p))
				.WithMessage("The promotion does not exist.");

			RuleFor(f => f.Name)
				.NotNull()
				.WithMessage("We need a match game name.")
				.NotEmpty()
				.WithMessage("We need a match game name.");

			RuleFor(c => c.Name)
				.Must(name => ShouldNotExistAlready(name))
				.WithMessage("The match game name already exists.");

			RuleFor(f => f.NumberOfBoards)
				.NotEmpty()
				.WithMessage("The number of boards need to be more than 0.")
				.NotNull()
				.WithMessage("The number of boards need to be more than 0.")
				.GreaterThan(0)
				.WithMessage("The number of boards need to be more than 0.")
				.LessThan(1000)
				.WithMessage("The number of boards need to be less than 1000.");
			RuleFor(f => f.WinnableBoards)
				.NotEmpty()
				.WithMessage("The number of winnable boards should be within 1 and the number of boards.")
				.NotNull()
				.WithMessage("The number of winnable boards should be within 1 and the number of boards.")
				.GreaterThan(0)
				.Must((matchGame, winnableBoards) => BeWithin(matchGame, winnableBoards))
				.WithMessage("The number of winnable boards should be within 1 and the number of boards.");
			RuleFor(f => f.WinProbability)
				.NotEmpty()
				.WithMessage("The win probability should be greater than 0.")
				.NotNull()
				.WithMessage("The win probability should be greater than 0.")
				.GreaterThan(0)
				.WithMessage("The win probability should be greater than 0.")
				.LessThanOrEqualTo(9)
				.WithMessage("The win probability should be less than or equal to 9");
			RuleFor(f => f.StartDate)
				.NotEmpty()
				.NotNull()
				.GreaterThan(DateTime.Now.AddHours(2))
				.WithMessage($"The start date should be greater than {DateTime.Now.AddHours(2).ToString("f")}.");
			RuleFor(f => f.EndDate)
				.NotEmpty()
				.NotNull()
				.Must((matchGame, endDate) => GreaterThan(matchGame, endDate))
				.WithMessage("The end date should be greater than the start date.");

		}

		private bool PromotionShouldExist(Guid promotionUid)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var promotionsForAccont = _campaignRepository.GetAccountCampaigns(_hrvsterUser.UserUid);

			return promotionsForAccont.Any(p => p.CampaignUid == promotionUid);
		}

		private bool GreaterThan(CreateMatchGame matchGame, DateTime endDate)
		{
			return endDate > matchGame.StartDate;
		}

		private bool BeWithin(CreateMatchGame matchGame, int winnableBoards)
		{
			return winnableBoards <= matchGame.NumberOfBoards &&
				winnableBoards > 0;
		}

		private bool ShouldNotExistAlready(string name)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var matachGames = _matchGamesRepository.GetAllForAccount(_hrvsterUser.UserUid);

			var result = matachGames.Any(s => s.Name == name);

			return !result;
		}
	}
}
