using MediatR;
using Promo.Core.MessageContracts;

namespace Promo.Core.Notifications
{
	public class AccountCreated : INotification
	{
		private IAccountCreated accountCreated;

		public AccountCreated(IAccountCreated message)
		{
			accountCreated = message;
		}

		public IAccountCreated Account
		{
			get { return accountCreated; }
		}
	}
}
