using FluentValidation;
using hrvster.Web.Models;
using System;

namespace hrvster.Web.Validation
{
	public class UpdateMatchGameValidator : AbstractValidator<UpdateMatchGame>
	{
		public UpdateMatchGameValidator()
		{
			RuleFor(f => f.Name)
				.NotNull()
				.WithMessage("We need a match game name.")
				.NotEmpty()
				.WithMessage("We need a match game name.");

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

		private bool BeWithin(UpdateMatchGame matchGame, int winnableBoards)
		{
			return winnableBoards <= matchGame.NumberOfBoards &&
				winnableBoards > 0;
		}

		private bool GreaterThan(UpdateMatchGame matchGame, DateTime endDate)
		{
			return endDate > matchGame.StartDate;
		}
	}
}
