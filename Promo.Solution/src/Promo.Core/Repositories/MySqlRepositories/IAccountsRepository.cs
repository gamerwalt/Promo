using Promo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promo.Core.Repositories
{
    public interface IAccountsRepository
    {
        Task CreateAsync(Account account);
        Task UpdateAsync(Guid accountUid, Account account);
        Task<Account> GetAsync(Guid accountUid);
        Task<Account> GetByEmailAddressAsync(string emailAddress);
        Task<Account> GetByPhoneNumberAsync(string phoneNumber);
        Task<Account> GetByDisplayNameAsync(string displayName);
        Task RemoveAsync(Account account);
        Task<List<Account>> GetAllAccounts();
    }
}
