using FluentValidation;
using hrvster.Web.Models.Plays;
using System.Linq;

namespace hrvster.Web.Validation
{
	public class PlayMatchGameValidator : AbstractValidator<PlayMatchGame>
	{
		public PlayMatchGameValidator()
		{
			RuleFor(x => x.SelectedTiles)
			   .NotEmpty()
			   .NotNull()
			   .WithMessage("You need to select 3 tiles.");

			RuleFor(x => x)
				.Custom((play, context) =>
				{
					var selectedTiles = play.SelectedTiles;
					var tileCount = selectedTiles.Split(',').ToList().Count;
					if (tileCount != 3)
						context.AddFailure("You need to select only 3 tiles.");
				});
		}
	}
}
