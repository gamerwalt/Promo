using Promo.Core.Authentication;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Promo.Clients.Internal
{
	public interface IReportingClient
	{
		Task ReportSweepEntryAsync(Guid sweepUid, Guid campaignUid, string screenName, string userIdentifierType, string externalUserId, string externalId);
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

		public async Task ReportSweepEntryAsync(Guid sweepUid, Guid campaignUid, string screenName, string userIdentifierType, string externalUserId, string externalId)
		{
			
		}
	}
}
