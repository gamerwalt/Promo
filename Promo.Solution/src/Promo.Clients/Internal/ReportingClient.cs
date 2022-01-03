using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Promo.Core.Authentication;
using Promo.Core.Exceptions;
using Promo.Core.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Clients.Internal
{
	public interface IReportingClient
	{
		Task ReportSweepEntryAsync(Guid sweepUid, Guid campaignUid, string screenName, string userIdentifierType, string externalUserId, string externalId);
		Task<SweepEntryReport> GetCampaignSweepUserCountAsync(Guid campaignUid, Guid sweepUid, string externalUserId, string screenName, string userIdentifierType);
	}

	public class ReportingClient : IReportingClient
	{
		private readonly HttpClient _client;
		private readonly IServiceAuthenticate _serviceAuthenticate;

		public ReportingClient(HttpClient client, IServiceAuthenticate serviceAuthenticate)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_serviceAuthenticate = serviceAuthenticate ?? throw new ArgumentNullException(nameof(serviceAuthenticate));
		}

		public async Task<SweepEntryReport> GetCampaignSweepUserCountAsync(
			Guid campaignUid, Guid sweepUid, string externalUserId, string screenName, string userIdentifierType)
		{
			var authModel = await AuthenticateService();

			var result = new HttpResponseMessage();
			string content = JsonConvert.SerializeObject(
				new
				{
					screenName,
					userIdentifierType,
					externalUserId
				}, new JsonSerializerSettings
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				}
			);

			var url = ServiceUrls.GetCampaignSweepUserEntryReport
				.Replace("{campaignUid}", campaignUid.ToString())
				.Replace("{sweepUid}", sweepUid.ToString());

			using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
			{
				requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authModel.IdToken);

				result = await _client.SendAsync(requestMessage);
				if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					throw new Exception(ExceptionMessages.UnauthroizedAccessToAPI);

				if (result.IsSuccessStatusCode)
				{
					return JsonConvert.DeserializeObject<SweepEntryReport>(await result.Content.ReadAsStringAsync());
				}
			}

			return null;
		}

		public async Task ReportSweepEntryAsync(Guid sweepUid, Guid campaignUid, string screenName, string userIdentifierType, string externalUserId, string externalId)
		{
			var authModel = await AuthenticateService();

			var result = new HttpResponseMessage();
			string content = JsonConvert.SerializeObject(
				new
				{
					sweepUid,
					campaignUid,
					screenName,
					userIdentifierType,
					externalId,
					externalUserId
				}, new JsonSerializerSettings
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				}
			);

			using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{ServiceUrls.CreateReportSweepEntry}"))
			{
				requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authModel.IdToken);

				result = await _client.SendAsync(requestMessage);
				if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					throw new Exception(ExceptionMessages.UnauthroizedAccessToAPI);
			}
		}

		private async Task<AuthenticationModel> AuthenticateService()
		{
			var authModel = await _serviceAuthenticate.Authenticate();
			if (authModel == null)
				throw new ServiceAuthenticateException();
			return authModel;
		}
	}
}
