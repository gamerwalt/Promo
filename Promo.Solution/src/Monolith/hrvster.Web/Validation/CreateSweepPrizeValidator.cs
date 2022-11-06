using FluentValidation;
using hrvster.Infrastructure;
using hrvster.Web.Models;
using System.Linq;

namespace hrvster.Web.Validation
{
	public class CreateSweepPrizeValidator : AbstractValidator<CreateSweepPrize>
	{
		private readonly ISweepPrizesRepository _repository;
		public CreateSweepPrizeValidator(ISweepPrizesRepository repository)
		{
			_repository = repository;

			RuleFor(x => x.PrizeName).NotEmpty().NotNull().WithMessage("Please supply a valid prize name.");
			RuleFor(x => x.MonetaryValue).NotEmpty().NotNull().WithMessage("Please supply the value of the prize.");

			RuleFor(x => x.PrizeName)
				.Must((x, prizeName) => ShouldNotExist(x, prizeName))
				.WithMessage("This prize already exists.");
		}

		private bool ShouldNotExist(CreateSweepPrize model, string prizeName)
		{
			var prizes = _repository.GetSweepPrizes(model.SweepUid);
			if (prizes.Count() == 0) return true;

			foreach(var prize in prizes)
			{
				if (prize.PrizeName.Equals(prizeName)) return false;
			}

			return true;
		}
	}
}
