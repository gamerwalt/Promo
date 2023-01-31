using FluentValidation;
using hrvster.Web.Models;
using System;

namespace hrvster.Web.Validation
{
	public class CreateMatchGameValidator : AbstractValidator<CreateMatchGame>
	{
		public CreateMatchGameValidator() 
		{
			RuleFor(f => f.Name)
				.NotNull()
				.NotEmpty()
				.WithMessage("We need a match game name.");
			RuleFor(f => f.NumberOfBoards)
				.NotEmpty()
				.NotNull()
				.GreaterThan(0)
				.WithMessage("The number of boards need to be more than 0.")
				.LessThan(1000)
				.WithMessage("The number of boards need to be less than 1000.");
			RuleFor(f => f.WinnableBoards)
				.NotEmpty()
				.NotNull()
				.GreaterThan(0)
				.Must((matchGame, winnableBoards) => BeWithin(matchGame, winnableBoards))
				.WithMessage("The number of winnable boards should be within 1 and the number of boards.");
			RuleFor(f => f.WinProbability)
				.NotEmpty()
				.NotNull()
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

		private bool GreaterThan(CreateMatchGame matchGame, DateTime endDate)
		{
			return endDate > matchGame.StartDate;
		}

		private bool BeWithin(CreateMatchGame matchGame, int winnableBoards)
		{
			return winnableBoards <= matchGame.NumberOfBoards &&
				winnableBoards > 0;
		}
	}
}
