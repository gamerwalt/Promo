using MediatR;
using Promo.Membership.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Core.RequestHandlers
{
    public class GetAllCampaignsForAccount : IRequest<List<Infrastructure.Models.Campaign>>
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

    public class GetAllCampaignsForAccountHandler : IRequestHandler<GetAllCampaignsForAccount, List<Infrastructure.Models.Campaign>>
    {
        private readonly ICampaignRepository _repository;

        public GetAllCampaignsForAccountHandler(ICampaignRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Infrastructure.Models.Campaign>> Handle(GetAllCampaignsForAccount request, CancellationToken cancellationToken)
        {
            return await _repository.GetAccountCampaignsAsync(request.AccountUid);
        }
    }
}
