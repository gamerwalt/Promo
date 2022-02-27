using FluentValidation;
using hrvster.Infrastructure;
using hrvster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.Validators
{
	public class AddCampaignRuleValidator : AbstractValidator<AddCampaignRule>
	{
		private readonly ICampaignRepository _campaignRepository;
		private readonly IWatchTypeCampaignTypesRepository _watchTypeCampaignTypesRepository;

		public AddCampaignRuleValidator(ICampaignRepository campaignRepository, IWatchTypeCampaignTypesRepository watchTypeCampaignTypesRepository)
		{
			_campaignRepository = campaignRepository;
			_watchTypeCampaignTypesRepository = watchTypeCampaignTypesRepository;

			RuleFor(x => x.CampaignUid).NotEmpty().NotNull().WithMessage("Please supply a valid campaign uid");
			RuleFor(x => x.CampaignRule).Must(x => RuleShouldHaveAValidWatchTypeCode(x)).WithMessage("Invalid watch type code.");
			RuleFor(x => x.SweepUid).NotEmpty().NotNull().WithMessage("Please supply a valid sweep uid");

			RuleFor(x => x).MustAsync(async (x, cancellation) =>
			{
				var watchTypeCode = x.CampaignRule.WatchTypeCode;
				var watchTypeCampaignTypes = await _watchTypeCampaignTypesRepository.GetWatchTypeCampaignTypesAsync();
				var campaign = await _campaignRepository.GetCampaignByUidAsync(x.CampaignUid);

				var watchTypeCampaigns = watchTypeCampaignTypes.Where(c => c.CampaignTypeUid == campaign.CampaignType.CampaignTypeUid);

				return watchTypeCampaigns.Any(c => c.WatchTypeCode.Equals(watchTypeCode.ToString()));
			}).WithMessage("Your watch type does not match the campaign type.");
		}

		private static bool RuleShouldHaveAValidWatchTypeCode(CampaignRule campaignRule)
		{
			return Enum.TryParse(typeof(WatchTypeCode), campaignRule.WatchTypeCode.ToString(), out var watchTypeCode);
		}
	}
}
