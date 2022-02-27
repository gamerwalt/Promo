using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetCampaignSweepEntryRule : IRequest<EntryRule>
	{
		private Guid campaignUid;
		private Guid sweepUid;

		public GetCampaignSweepEntryRule(Guid campaignUid, Guid sweepUid)
		{
			this.campaignUid = campaignUid;
			this.sweepUid = sweepUid;
		}

		public Guid CampaignUid
		{
			get { return campaignUid; }
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}
	}

	public class GetCampaignSweepEntryRuleHandler : IRequestHandler<GetCampaignSweepEntryRule, EntryRule>
	{
		private readonly IEntryRulesRepository _repository;

		public GetCampaignSweepEntryRuleHandler(IEntryRulesRepository repository)
		{
			_repository = repository;
		}

		public async Task<EntryRule> Handle(GetCampaignSweepEntryRule request, CancellationToken cancellationToken)
		{
			return await _repository.GetBySweepAndCampaignUidAsync(request.SweepUid, request.CampaignUid);
		}
	}
}
