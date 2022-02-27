using FluentValidation;
using hrvster.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace hrvster.Core.Requests.Validators
{
	public class GetSweepsForCampaignValidator : AbstractValidator<GetSweepsForCampaignUid>
	{
		private ILogger<GetSweepsForCampaignValidator> _logger;
		private readonly ICampaignRepository _campaignRepository;

		public GetSweepsForCampaignValidator(ILoggerFactory loggerFactory, ICampaignRepository campaignRepository)
		{
			_logger = loggerFactory.CreateLogger<GetSweepsForCampaignValidator>();
			_campaignRepository = campaignRepository;

			RuleFor(x => x.AccountVerifierUid).NotEmpty().NotNull().WithMessage("Account Uid making this request is missing");
			RuleFor(x => x.CampaignUid).NotEmpty().NotNull().WithMessage("Campaign Uid required to make this request.");
			RuleFor(x => x).MustAsync(async (x, cancellation) =>
			{
				var campaigns = await _campaignRepository.GetAccountCampaignsAsync(x.AccountVerifierUid);

				return campaigns.Any(c => c.CampaignUid == x.CampaignUid);
			}).WithMessage("Campaign does not belong to this account.");
		}
	}
}
