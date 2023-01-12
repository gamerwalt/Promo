using FluentValidation;
using hrvster.Web.Models;

namespace hrvster.Web.Validation
{
	public class PromoteBackupDetailsValidator : AbstractValidator<PromoteBackupDetails>
	{
		public PromoteBackupDetailsValidator() 
		{
			RuleFor(x => x.Reason)
				.NotEmpty()
				.NotNull()
				.WithMessage("Please supply a reason for promoting the next backup.");
		}
	}
}
