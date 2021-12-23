using MediatR;
using Promo.Core.Repositories.MySqlRepositories;
using Promo.Models.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.CoreRequestHandlers
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
        private readonly IAccountsRepository _accountsRepository;
        private readonly ICampaignRepository _repository;

        public GetAllCampaignsForAccountHandler(IAccountsRepository accountsRepository, ICampaignRepository repository)
        {
            _repository = repository;
            _accountsRepository = accountsRepository;
        }

        public async Task<List<Campaign>> Handle(GetAllCampaignsForAccount request, CancellationToken cancellationToken)
        {
            var account = await _accountsRepository.GetAsync(request.AccountUid);

            return await _repository.GetAccountCampaignsAsync(account.AccountId);
        }
    }
}
