using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetAllCampaignsForAccount : IRequest<List<Campaign>>
	{
		private Guid accountUid;

		public GetAllCampaignsForAccount(Guid accountUid)
		{
			this.accountUid = accountUid;
		}

		public Guid AccountUid
		{
			get { return accountUid; }
		}
	}

	public class GetAllCampaignsForAccountHandler : IRequestHandler<GetAllCampaignsForAccount, List<Campaign>>
	{
		private readonly ICampaignRepository _repository;

		public GetAllCampaignsForAccountHandler(ICampaignRepository repository)
		{
			_repository = repository;
		}

		public async Task<List<Campaign>> Handle(GetAllCampaignsForAccount request, CancellationToken cancellationToken)
		{
			return await _repository.GetAccountCampaignsAsync(request.AccountUid);
		}
	}
}
