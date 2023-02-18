using hrvster.Models;
using hrvster.Models.Options;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Payments
{
	public class FlutterwavePaymentService : IPaymentAdaptor
	{
		private readonly ILogger<FlutterwavePaymentService> _logger;
		private readonly IHttpClientFactory _httpClientFactory;

		private HttpClient _client;

		public FlutterwavePaymentService(
			ILoggerFactory loggerFactory,
			IHttpClientFactory httpClientFactory)
		{
			_logger = loggerFactory.CreateLogger<FlutterwavePaymentService>();
			_httpClientFactory = httpClientFactory;

			_client = _httpClientFactory.CreateClient(HttpClients.Flutterwave);
		}

		public async Task<string> GetUserSubscriptionAsync(string emailAddress, CancellationToken cancellationToken)
		{
			var uri = $"{FlutterwaveUrls.Subscriptions}?{FlutterwaveParameters.Email}={emailAddress}&{FlutterwaveParameters.Status}=active";

			var result = await _client.GetAsync(uri, cancellationToken);

			if (result.IsSuccessStatusCode)
			{
				return await result.Content.ReadAsStringAsync();
			}

			return string.Empty;
		}

		public async Task UnsubscribeUserAsync(long id, CancellationToken cancellationToken)
		{
			var uri = $"{FlutterwaveUrls.Subscriptions}/{id}/cancel";

			var result = await _client.PutAsync(uri, null, cancellationToken);

			if (result.IsSuccessStatusCode)
			{
				_logger.LogInformation("Unsubscribing from the Id {id} successfully.", id);

				return;
			}

			_logger.LogWarning("Unsubscribing from the Id {id} did not work.", id);
		}
	}
}
