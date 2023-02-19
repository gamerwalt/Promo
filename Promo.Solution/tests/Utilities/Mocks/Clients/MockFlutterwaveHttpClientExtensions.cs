using hrvster.ApplicationServices.Payments;
using hrvster.Core.Clients.Publishers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TestUtilities.Data;

namespace TestUtilities.Mocks.Clients
{
	public static class MockFlutterwaveHttpClientExtensions
	{
		public static IServiceCollection MockPaymentAdaptor(
			this IServiceCollection services,
			Action<Mock<IPaymentAdaptor>> mockSetup = null)
		{
			services.RemoveAll<IPaymentAdaptor>();

			var mock = new Mock<IPaymentAdaptor>();
			mockSetup?.Invoke(mock);

			services.AddSingleton(mock);
			services.AddSingleton(mock.Object);

			return services;
		}

		public static IServiceCollection MockFlutterwaveHttpClient(
			this IServiceCollection services,
			ILoggerFactory loggerFactory,
			IHttpClientFactory httpClientFactory)
		{
			services.RemoveAll<FlutterwavePaymentService>();

			var mock = new Mock<FlutterwavePaymentService>(loggerFactory, httpClientFactory);

			mock.Setup(x => x.GetUserSubscriptionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(GetSamplePayload());
			mock.Setup(x => x.UnsubscribeUserAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
				.Returns(Task.CompletedTask);

			services.AddSingleton(mock);
			services.AddSingleton(mock.Object);

			return services;
		}

		public static Mock<FlutterwavePaymentService> MockGetUserSubscriptionAsync(this Mock<FlutterwavePaymentService> mock)
		{
			mock.Setup(x => x.GetUserSubscriptionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(GetSamplePayload());

			return mock;
		}

		public static Mock<FlutterwavePaymentService> MockUnsubscribeUser(this Mock<FlutterwavePaymentService> mock)
		{
			mock.Setup(x => x.UnsubscribeUserAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
				.Returns(Task.CompletedTask);

			return mock;
		}

		private static string GetSamplePayload()
		{
			return "{\"status\":\"success\",\"message\":\"Plan subscriptions fetched\",\"meta\":{\"page_info\":{\"total\":1,\"current_page\":1,\"total_pages\":1}},\"data\":[{\"id\":19795,\"amount\":100,\"customer\":{\"id\":1981817,\"customer_email\":\"" + TestData.EmailAddress + "\"},\"plan\":32878,\"status\":\"active\",\"created_at\":\"2023-02-18T04:07:13.000Z\"}]}";
		}
	}
}
