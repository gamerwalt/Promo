using Microsoft.Extensions.Configuration;
using Promo.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Clients.Internal
{
    public interface IAccountsClient
    {
        Task<IEnumerable<Account>> GetUnverifiedAccountsAsync(CancellationToken token);
        Task VerifyAccountEmailAsync(Guid accountUid);
    }

    public class AccountsClient : IAccountsClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public AccountsClient(HttpClient client, IConfiguration config)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<IEnumerable<Account>> GetUnverifiedAccountsAsync(CancellationToken token)
        {
            var result = await _client.GetAsync("v1/accounts/unverified");

            return new List<Account>();
        }

        public async Task VerifyAccountEmailAsync(Guid accountUid)
        {
            await _client.PostAsync($"v1/accounts/{accountUid}/verify", default);
        }
    }
}
