using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using hrvster.Models.Company;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Accounts
{
	public interface IClientMemberService
	{
		Task CreateAsync(string userId, string userName, Account account, CancellationToken cancellationToken);
		Task CreateClientMemberPoints(int pointsPerLoss, string userId, string userName, Account account);
		Task<int> GetClientMemberPointAsync(string userId, string userName, Guid accountUid);
	}

	public class ClientMemberService : IClientMemberService
	{
		private readonly ILogger<ClientMemberService> _logger;
		private readonly IClientMemberRepository _repository;
		private readonly IAccountsService _accountService;

		public ClientMemberService(
			ILoggerFactory loggerFactory, 
			IClientMemberRepository repository, 
			IAccountsService accountService)
		{
			_logger = loggerFactory.CreateLogger<ClientMemberService>();
			_repository = repository;
			_accountService = accountService;
		}

		public async Task CreateAsync(string userId, string userName, Account account, CancellationToken cancellationToken)
		{
			var clientMember = new ClientMember
			{
				Uid = Guid.NewGuid(),
				Account = account,
				ExternalUserName = userName,
				ExternalId = userId
			};

			if (await _repository.ClientMemberExistsAsync(clientMember, cancellationToken))
			{
				_logger.LogInformation("Client member with external id {ExternalId} for account with uid {AccountUid} already exists.",
					clientMember.ExternalId,
					clientMember.Account.AccountUid);

				return;
			}

			_logger.LogInformation("Creating the client member with external id {ExternalId} for account with uid {AccountUid}",
				clientMember.ExternalId,
				clientMember.Account.AccountUid);

			await _repository.CreateAsync(clientMember, cancellationToken);
		}

		public async Task CreateClientMemberPoints(int pointsPerLoss, string userId, string userName, Account account)
		{
			var clientMemberPoint = new ClientMemberPoint
			{
				ClientMember = new ClientMember
				{
					Account = account,
					ExternalId = userId,
					ExternalUserName = userName
				},
				Activity = "Pick3GameLoss",
				Points = pointsPerLoss,
				Uid = Guid.NewGuid()
			};

			await _repository.CreateClientMemberPoints(clientMemberPoint);
		}

		public async Task<int> GetClientMemberPointAsync(string userId, string userName, Guid accountUid)
		{
			var account = await _accountService.GetAccountByUidAsync(accountUid);

			if (account == null)
			{
				_logger.LogWarning("Unable to find the account for uid {AccountUid} while trying to get the client member points", accountUid);

				return 0;
			}

			var points = await _repository.GetClientMemberPointsAsync(userId, account);

			return points.Sum(c => c.Points);
		}
	}
}
