using MediatR;
using Promo.Membership.Core.Notifications;
using Promo.Membership.Infrastructure;
using Promo.Membership.Infrastructure.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Core
{
    public class StoreAccountInDatabase : INotificationHandler<AccountCreatedViaFirebase>
    {
        private readonly IAccountsRepository _repository;

        public StoreAccountInDatabase(IAccountsRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(AccountCreatedViaFirebase notification, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(new Account
            {
                AccountUid = Guid.Parse(notification.CreatedUser.Uid),
                EmailAddress = notification.CreatedUser.Email,
                EmailVerified = notification.CreatedUser.EmailVerified,
                PhoneNumber = notification.CreatedUser.PhoneNumber,
                DisplayName = notification.CreatedUser.DisplayName,
                PhoneNumberVerified = false,
                Pin = notification.Pin,
                TermsAccepted = notification.TermsAccepted
            });
        }
    }
}
