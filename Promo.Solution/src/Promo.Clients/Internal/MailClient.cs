using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Clients.Internal
{
    public interface IMailClient
    {
        Task<bool> CreateAsync(Core.Models.Email email, string accessToken);
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

        public async Task<bool> CreateAsync(Core.Models.Email email, string accessToken)
        {
            var result = new HttpResponseMessage();
            var content = JsonConvert.SerializeObject(email, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using(var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/emails/"))
            {
                requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                result = await _client.SendAsync(requestMessage);
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception("Unauthorized access to email API");
            }

            return result.IsSuccessStatusCode;
        }
    }
}
