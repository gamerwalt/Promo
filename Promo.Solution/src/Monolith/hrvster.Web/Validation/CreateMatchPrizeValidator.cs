using FluentValidation;
using hrvster.ApplicationServices.MatchGames;
using hrvster.Web.Models;

namespace hrvster.Web.Validation
{
	public class CreateMatchPrizeValidator : AbstractValidator<CreateMatchPrize>
	{
		private readonly IMatchGamesService _matchGameService;
		private readonly IMatchPrizeService _matchPrizeService;

		public CreateMatchPrizeValidator(IMatchGamesService matchGamesService, IMatchPrizeService matchPrizeService)
		{
			_matchGameService = matchGamesService;
			_matchPrizeService = matchPrizeService;

			RuleFor(x => x.ImageUid)
				.NotEmpty()
				.WithMessage("An image needs to be selected.");

			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage("Name is required.");

			RuleFor(x => x.Description)
				.NotEmpty()
				.WithMessage("Description is required.");

			RuleFor(x => x.Value)
				.NotEmpty()
				.WithMessage("Value is required.");

			RuleFor(x => x.Quantity)
				.NotEmpty()
				.WithMessage("Quantity is required.")
				.Must((model, quantity) =>
				{
					int totalPrizes = _matchPrizeService.GetTotalPrizesForGame(model.MatchGameUid);
					var matchGame = _matchGameService.GetMatchGame(model.MatchGameUid);
					int winnableBoards = matchGame.NumberOfWinnableBoards;

					return totalPrizes + quantity <= winnableBoards;
				})
				.WithMessage("The total quantity of prizes, including the new prize, cannot exceed the number of winnable boards.");
		}
	}
}
