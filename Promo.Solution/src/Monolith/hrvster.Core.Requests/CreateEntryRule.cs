using hrvster.Core.Requests.ApiRequests;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class CreateEntryRule : IRequest<EntryRule>
	{
		private NewEntryRuleRequest entryRuleRequest;

		public CreateEntryRule(NewEntryRuleRequest entryRule)
		{
			this.entryRuleRequest = entryRule;
		}

		public NewEntryRuleRequest EntryRuleRequest
		{
			get { return entryRuleRequest; }
		}
	}

	public class CreateEntryRuleHandler : IRequestHandler<CreateEntryRule, EntryRule>
	{
		private readonly IEntryRulesRepository _repository;

		public CreateEntryRuleHandler(IEntryRulesRepository repository)
		{
			_repository = repository;
		}

		public async Task<EntryRule> Handle(CreateEntryRule request, CancellationToken cancellationToken)
		{
			var entryRuleUid = Guid.NewGuid();
			await _repository.CreateAsync(new EntryRule
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
