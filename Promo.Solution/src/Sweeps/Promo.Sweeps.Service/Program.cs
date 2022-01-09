using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promo.Clients.Internal;
using Promo.Core;
using Promo.Core.Authentication;
using Promo.Core.Models;
using Promo.Sweeps.Core;
using Promo.Sweeps.Core.RequestHandlers;
using Promo.Sweeps.Draw.Service;
using Promo.Sweeps.Infrastructure;
using Serilog;
using System;
using System.Reflection;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Promo.Sweeps.Service
{
	public class Program
	{
		private static IConfiguration Configuration;

		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(ConfigureAppConfiguration)
				.UseSerilog((context, configuration) => 
					configuration.ReadFrom.Configuration(context.Configuration)
						.WriteTo.Console()
						.Enrich.FromLogContext()
						.MinimumLevel.Debug(), preserveStaticLogger: true)
				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<DrawHostedService>();

					services.AddTransient<IDrawService, DrawService>();

					AddMediator(services);
					AddClients(services);
					AddDataAccess(services);
					AddHrvsterServiceAuthenticationclient(services);

					AddOptions(services);
					services.AddSingleton(typeof(ILogger), typeof(Logger<Program>));
				});

		private static void AddOptions(IServiceCollection services)
		{
			services.Configure<DrawSettingOptions>(Configuration.GetSection(DrawSettingOptions.DrawSettings));
		}

		private static void AddHrvsterServiceAuthenticationclient(IServiceCollection services)
		{
			services.Configure<HrvsterServiceAccount>(Configuration);
			services.Configure<HrvsterServiceAccount>(Configuration
				.GetSection(HrvsterServiceAccount.SectionName));
			services.AddTransient(p => p.GetService<IOptions<HrvsterServiceAccount>>().Value);

			services.AddHttpClient<IServiceAuthenticate, ServiceAuthenticate>(client =>
			{
				client.BaseAddress = new Uri(Configuration["GoogleAuthenticateUrl"]);
				client.DefaultRequestHeaders.Add("Accept", "application/json");
			});
		}

		private static void ConfigureAppConfiguration(HostBuilderContext hostBuilder, IConfigurationBuilder configurationBuilder)
		{
			Configuration = configurationBuilder.Build();
		}

		private static void AddDataAccess(IServiceCollection services)
		{
			services.AddSingleton<IDatabase>(c =>
			{
				return new MySqlDatabase("Sweeps", Configuration);
			});

			services.AddTransient<ISweepsRepository, SweepsRepository>();
			services.AddTransient<ISweepsEntriesRepository, SweepsEntriesRepository>();
		}

		private static void AddClients(IServiceCollection services)
		{
			AddCampaignClient(services);
			AddRulesClient(services);
		}

		private static void AddRulesClient(IServiceCollection services)
		{
			var rulesBaseApi = Configuration["PromoRulesUrl"];

			services.AddHttpClient<IRulesClient, RulesClient>(client =>
			{
				client.BaseAddress = new Uri(rulesBaseApi);
				client.DefaultRequestHeaders.Add("Accept", "application/json");
			});
		}

		private static void AddCampaignClient(IServiceCollection services)
		{
			var campaignBaseApi = Configuration["PromoCampaignsUrl"];

			services.AddHttpClient<ICampaignsClient, CampaignsClient>(client =>
			{
				client.BaseAddress = new Uri(campaignBaseApi);
				client.DefaultRequestHeaders.Add("Accept", "application/json");
			});
		}

		private static void AddMediator(IServiceCollection services)
		{
			var domainAssembly = typeof(GetSweepDrawRulesHandler).GetTypeInfo().Assembly;
			services.AddMediatR(domainAssembly);
		}
	}
}
