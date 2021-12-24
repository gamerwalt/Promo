using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Promo.Core.Authentication;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Clients.External
{
    public interface IIdentityClient
    {
        Task<SignInResponse> SignIn(AuthenticateRequest request);
    }

    public class IdentityClient : IIdentityClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public IdentityClient(HttpClient client, IConfiguration config)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<SignInResponse> SignIn(AuthenticateRequest request)
        {
            var content = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var url = $"{_config["IdentityUrl"]}{_config["GoogleServices:WebApiKey"]}";

            var result = await _client.PostAsync(url, stringContent);
            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<SignInResponse>(await result.Content.ReadAsStringAsync());
            }

            return null;
        }
    }

}
