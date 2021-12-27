using FluentValidation;
using MediatR;
using Promo.Campaign.Infrastructure;
using Promo.Campaign.Infrastructure.Models;
using Promo.Clients.Internal;
using Promo.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Campaign.Core.RequestHandlers
{
    public class AddCampaignRuleValidator : AbstractValidator<AddCampaignRule>
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IRulesClient _rulesClient;

        public AddCampaignRuleValidator(ICampaignRepository campaignRepository, IRulesClient rulesClient)
        {
            _campaignRepository = campaignRepository;
            _rulesClient = rulesClient;

            RuleFor(x => x.CampaignUid).NotEmpty().NotNull().WithMessage("Please supply a valid campaign uid");
            RuleFor(x => x.CampaignRule).Must(x => RuleShouldHaveAValidWatchTypeCode(x)).WithMessage("Invalid watch type code.");
            
            RuleFor(x => x).MustAsync(async (x, cancellation) =>
            {
                var watchTypeCode = (WatchTypeCode)Enum.Parse(typeof(WatchTypeCode), x.CampaignRule.WatchTypeCode);
                var watchTypeCampaignTypes = await _rulesClient.GetWatchTypeCampaignTypesAsync();
                var campaign = await _campaignRepository.GetCampaignByUidAsync(x.CampaignUid);

                var watchTypeCampaigns = watchTypeCampaignTypes.Where(c => c.CampaignTypeUid == campaign.CampaignType.CampaignTypeUid);
                
                return watchTypeCampaigns.Any(c => c.WatchTypeCode.Equals(watchTypeCode.ToString()));
            }).WithMessage("Your watch type does not match the campaign type.");
        }

        private bool RuleShouldHaveAValidWatchTypeCode(CampaignRule campaignRule)
        {
            return Enum.TryParse(typeof(WatchTypeCode), campaignRule.WatchTypeCode, out var watchTypeCode);
        }
    }

    public class AddCampaignRule : IRequest
    {
        private Guid campaignUid;
        private CampaignRule rule;

        public AddCampaignRule(Guid campaignUid, CampaignRule rule)
        {
            this.campaignUid = campaignUid;
            this.rule = rule;
        }

        public Guid CampaignUid
        {
            get { return campaignUid; }
        }

        public CampaignRule CampaignRule
        {
            get { return rule; }
        }
    }

    public class AddCampaignRuleHandler : IRequestHandler<AddCampaignRule>
    {
        public async Task<Unit> Handle(AddCampaignRule request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
