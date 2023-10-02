using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using hrvster.Models.Subscriptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Subscription
{
	public interface IAccountSubscriptionService
	{
		AccountSubscription GetAccountSubscription(Guid accountUid);
		Task<AccountSubscription> GetAccountSubscriptionAsync(Guid accountUid);
		Task<IEnumerable<AccountSubscription>> GetAccountSubscriptionsByStatysAsync(SubscriptionStatus status, int pendingHours);
		Task SetAccountSubscriptionStatusAsync(Guid subscriptionUid, SubscriptionStatus newStatus);
	}

	public class AccountSubscriptionService : IAccountSubscriptionService
	{
		private readonly IAccountSubscriptionsRepository _repository;

		public AccountSubscriptionService(IAccountSubscriptionsRepository repository)
		{
			_repository = repository;
		}

		public AccountSubscription GetAccountSubscription(Guid accountUid)
			=> _repository.GetAccountSubscription(accountUid);

		public async Task<AccountSubscription> GetAccountSubscriptionAsync(Guid accountUid)
			=> await _repository.GetAccountSubscriptionAsync(accountUid);

		public async Task<IEnumerable<AccountSubscription>> GetAccountSubscriptionsByStatysAsync(
			SubscriptionStatus status,
			int pendingHours)
			=> await _repository.GetAccountSubscriptionsByStatusAsync(status, pendingHours);

		public Task SetAccountSubscriptionStatusAsync(Guid subscriptionUid, SubscriptionStatus newStatus)
			=> _repository.UpdateAccountSubscriptionStatusAsync(subscriptionUid, newStatus);
	}
}
