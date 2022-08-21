using FluentValidation;
using hrvster.Web.Models;

namespace hrvster.Web.Validation
{
	public class SelectWatchTypeProviderValidator : AbstractValidator<SelectWatchTypeProvider>
	{
		public SelectWatchTypeProviderValidator()
		{
			RuleFor(x => x)
				.Must(x => ValueMustBeSet(x))
				.WithMessage("Please select a provider to continue.");
		}

		private bool ValueMustBeSet(SelectWatchTypeProvider form)
		{
			return form.ProviderCode != "0";
		}
	}
}
