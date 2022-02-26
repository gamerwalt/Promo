using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetAccountRequest : IRequest<Account>
	{
		public GetAccountRequest(Guid accountUid)
		{
			AccountUid = accountUid;
		}

		public Guid AccountUid { get; set; }
	}

	public class GetAccountRequestHandler : IRequestHandler<GetAccountRequest, Account>
	{
		private readonly IAccountsRepository _repository;
		public GetAccountRequestHandler(IAccountsRepository repository)
		{
			_repository = repository;
		}

		public async Task<Account> Handle(GetAccountRequest request, CancellationToken cancellationToken)
		{
			return await _repository.GetAsync(request.AccountUid);
		}
	}
}
