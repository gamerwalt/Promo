using hrvster.Core.Requests.ApiRequests;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class CreateSweepEntryRule : IRequest<SweepEntryRule>
	{
		private NewSweepEntryRuleRequest entryRuleRequest;

		public CreateSweepEntryRule(NewSweepEntryRuleRequest entryRule)
		{
			this.entryRuleRequest = entryRule;
		}

		public NewSweepEntryRuleRequest EntryRuleRequest
		{
			get { return entryRuleRequest; }
		}
	}

	public class CreateEntryRuleHandler : IRequestHandler<CreateSweepEntryRule, SweepEntryRule>
	{
		private readonly ISweepEntryRulesRepository _repository;

		public CreateEntryRuleHandler(ISweepEntryRulesRepository repository)
		{
			_repository = repository;
		}

		public async Task<SweepEntryRule> Handle(CreateSweepEntryRule request, CancellationToken cancellationToken)
		{
			var entryRuleUid = Guid.NewGuid();
			await _repository.CreateAsync(new SweepEntryRule
			{
				EntryRuleUid = entryRuleUid,
				CampaignUid = request.EntryRuleRequest.CampaignUid,
				SweepUid = request.EntryRuleRequest.SweepUid,
				EntryPerUser = request.EntryRuleRequest.EntryPerUser
			});

			return await _repository.GetAsync(entryRuleUid);
		}
	}
}
