using FluentValidation;
using hrvster.Infrastructure;
using Microsoft.Extensions.Logging;

namespace hrvster.Core.Requests.Validators
{
	public class CreateSweepValidator : AbstractValidator<CreateSweep>
	{
		private readonly ILogger<CreateSweepValidator> _logger;
		private readonly ICampaignRepository _campaignRepository;

		public CreateSweepValidator(ILoggerFactory loggerFactory, ICampaignRepository campaignRepository)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepValidator>();
			_campaignRepository = campaignRepository;

			RuleFor(c => c.Sweep.AccountUid).NotEmpty().NotNull().WithMessage("Invalid account. A sweepstakes needs a valid account.");
			RuleFor(c => c.Sweep.CampaignUid).NotEmpty().NotNull().WithMessage("Invalid campaign/promotion. A sweepstakes needs a valid campaign/promotion.");
			RuleFor(c => c).MustAsync(async (c, cancellation) =>
			{
				_logger.LogInformation("Validaing sweepstakes {sweepName} against AccountUid: {accountUid} and CampaignUid {campaignUid}", c.Sweep.Name, c.Sweep.AccountUid, c.Sweep.CampaignUid);

				var campaign = await _campaignRepository.GetCampaignByUidAsync(c.Sweep.CampaignUid);

				return campaign.Account?.AccountUid == c.Sweep.AccountUid;

			}).WithMessage("Cannot add a sweepstakes under a campaign owned by another account.");
		}
	}
}
