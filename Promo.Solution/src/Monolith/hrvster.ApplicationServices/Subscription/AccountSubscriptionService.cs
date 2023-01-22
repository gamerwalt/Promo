using hrvster.Infrastructure.Subscriptions;
using hrvster.Models.Subscriptions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Subscription
{
	public interface IAccountSubscriptionService
	{
		Task<AccountSubscription> GetAccountSubscriptionAsync(Guid accountUid);
	}

	public class AccountSubscriptionService : IAccountSubscriptionService
	{
		private readonly ILogger<AccountSubscriptionService> _logger;
		private readonly IAccountSubscriptionsRepository _repository;

		public AccountSubscriptionService(ILoggerFactory loggerFactory, IAccountSubscriptionsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<AccountSubscriptionService>();
			_repository = repository;
		}

		public async Task<AccountSubscription> GetAccountSubscriptionAsync(Guid accountUid)
			=> await _repository.GetAccountSubscriptionAsync(accountUid);
	}
}
