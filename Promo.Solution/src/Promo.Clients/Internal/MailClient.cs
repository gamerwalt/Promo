using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Clients.Internal
{
    public interface IMailClient
    {
        Task CreateAsync(Core.Models.Email email);
    }

    public class MailClient : IMailClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public MailClient(HttpClient client, IConfiguration config)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task CreateAsync(Core.Models.Email email)
        {
            var content = JsonConvert.SerializeObject(email, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await _client.PostAsync($"v1/emails/", new StringContent(content, Encoding.UTF8, "application/json"));
        }
    }
}
