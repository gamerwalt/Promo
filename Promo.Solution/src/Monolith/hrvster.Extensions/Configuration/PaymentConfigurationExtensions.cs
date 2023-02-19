using hrvster.ApplicationServices.Payments;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using hrvster.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace hrvster.Extensions.Configuration
{
	public static class PaymentConfigurationExtensions
	{
		public static IServiceCollection AddPaymentServices(this IServiceCollection services, IConfiguration configuration)
			=> services
				.AddTransient<IPaymentService, PaymentService>()
				.AddTransient<IPaymentServiceRepository, PaymentServiceRepository>()
				.AddTransient<FlutterwavePaymentService>()
				//.AddTransient<PaystackPaymentService>()
				.AddFlutterWaveConfigurations(configuration)
				.AddFlutterwavePlans()
				.AddFlutterwaveEventRepository()
				.AddTransient<Func<PaymentClient, IPaymentAdaptor>>(sp => serviceTypeName =>
				{
					switch (serviceTypeName)
					{
						case PaymentClient.Flutterwave:
							return sp.GetService<FlutterwavePaymentService>();
						//case PaymentClient.Paystack:
						//	return sp.GetService<PaystackPaymentService>();
						default:
							return sp.GetService<FlutterwavePaymentService>();
					}
				});

		public static IServiceCollection AddFlutterWaveConfigurations(
			this IServiceCollection services,
			IConfiguration configuration)
			=> services
				.Configure<FlutterwaveConfigurations>(configuration)
				.Configure<FlutterwaveConfigurations>(configuration.GetSection(FlutterwaveConfigurations.SectionName))
				.AddTransient(p => p.GetService<IOptions<FlutterwaveConfigurations>>().Value);

		public static IServiceCollection AddFlutterwavePlans(this IServiceCollection services)
			=> services.AddTransient<IFlutterwavePlanRepository, FlutterwavePlanRepository>();

		public static IServiceCollection AddFlutterwaveEventService(this IServiceCollection services)
			=> services.AddTransient<IFlutterwaveEventService, FlutterwaveEventService>()
				.AddTransient<IFlutterwaveCardTransactionRepository, FlutterwaveCardTransactionRepository>()
				.AddTransient<IFlutterwaveCardTransactionService, FlutterwaveCardTransactionService>()
				.AddFlutterwaveEventRepository();
	}
}
