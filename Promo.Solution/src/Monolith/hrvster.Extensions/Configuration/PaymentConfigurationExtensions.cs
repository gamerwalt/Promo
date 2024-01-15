using hrvster.ApplicationServices.Payments;
using hrvster.ApplicationServices.Stripe;
using hrvster.ApplicationServices.Stripe.Processors;
using hrvster.Core.Clients.Stripe.Processors;
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
				.AddStripeConfiguration(configuration)
				.AddFlutterWaveConfigurations(configuration)
				.AddFlutterwavePlans()
				.AddStripePlans()
				.AddFlutterwaveEventRepository()
				.AddStripeEventRepository()
				.AddStripeServices()
				.AddPlanRepositories()
				.AddTransient<Func<PaymentClient, IPlanRepository>>(sp => serviceTypeName =>
				{
					switch(serviceTypeName)
					{
						case PaymentClient.Flutterwave:
							return sp.GetService<FlutterwavePlanRepository>();
						case PaymentClient.Stripe:
							return sp.GetService<StripePlanRepository>();

						default:
							throw new InvalidOperationException($"Unsupported payment client - {serviceTypeName}");
					}
				})
				.AddTransient<Func<PaymentClient, IPaymentAdaptor>>(sp => serviceTypeName =>
				{
					switch (serviceTypeName)
					{
						case PaymentClient.Flutterwave:
							return sp.GetService<FlutterwavePaymentService>();
						case PaymentClient.Stripe:
							return sp.GetService<StripePaymentService>();
						default:
							throw new InvalidOperationException($"Unsupported payment service - {serviceTypeName}");
					}
				});

		public static IServiceCollection AddPlanRepositories(
			this IServiceCollection services)
				=> services
					.AddTransient<StripePlanRepository>()
					.AddTransient<FlutterwavePlanRepository>();

		public static IServiceCollection AddStripeConfiguration(
			this IServiceCollection services,
			IConfiguration configuration)
				=> services
					.Configure<StripeOptions>(configuration)
					.Configure<StripeOptions>(configuration.GetSection(StripeOptions.SectionName))
					.AddTransient(s => s.GetService<IOptions<StripeOptions>>().Value);

		public static IServiceCollection AddFlutterWaveConfigurations(
			this IServiceCollection services,
			IConfiguration configuration)
			=> services
				.Configure<FlutterwaveConfigurations>(configuration)
				.Configure<FlutterwaveConfigurations>(configuration.GetSection(FlutterwaveConfigurations.SectionName))
				.AddTransient(p => p.GetService<IOptions<FlutterwaveConfigurations>>().Value);

		public static IServiceCollection AddFlutterwavePlans(this IServiceCollection services)
			=> services.AddTransient<IPlanRepository, FlutterwavePlanRepository>();

		public static IServiceCollection AddStripePlans(this IServiceCollection services)
			=> services.AddTransient<IPlanRepository, StripePlanRepository>();

		public static IServiceCollection AddFlutterwaveEventService(this IServiceCollection services)
			=> services.AddTransient<IFlutterwaveEventService, FlutterwaveEventService>()
				.AddTransient<IFlutterwaveCardTransactionRepository, FlutterwaveCardTransactionRepository>()
				.AddTransient<IFlutterwaveCardTransactionService, FlutterwaveCardTransactionService>()
				.AddFlutterwaveEventRepository();

		public static IServiceCollection AddStripeServices(this IServiceCollection services)
			=> services.AddTransient<IStripeEventService, StripeEventService>()
				.AddTransient<IStripeEventProcessor, StripeEventProcessor>()
				.AddTransient<IPendingEventsProcessor, PendingEventsProcessor>();
	}
}
