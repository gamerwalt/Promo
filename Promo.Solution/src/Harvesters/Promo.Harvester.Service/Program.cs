using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Promo.Clients.Internal;
using Promo.Core;
using Promo.Core.Authentication;
using Promo.Core.Models;
using Promo.Harvester.Core;
using Promo.Harvester.Core.Services;
using Promo.Harvester.Infrastructure;
using Serilog;
using System;
using System.Reflection;
using Tweetinvi;
using Tweetinvi.Models;

namespace Promo.Harvester.Service
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
                    services.AddHostedService<HarvesterHostedService>();
                    services.AddTransient<IStreamService, StreamService>();
                    services.AddTransient<IHarvesterService, HarvesterService>();

                    AddMediator(services);

                    AddRulesClient(services);
                    AddCampaignsClient(services);
                    AddSweepsClient(services);
                    AddTwitterClient(services);
					AddReportingClient(services);
                    AddHrvsterServiceAuthenticationclient(services);
                    AddDatabaseAndRepositories(services);
                });

		private static void AddMediator(IServiceCollection services)
        {
            var domainAssembly = typeof(ProcessReceivedTweetFromHashtagStream).GetTypeInfo().Assembly;
            services.AddMediatR(domainAssembly);
        }

        private static void AddDatabaseAndRepositories(IServiceCollection services)
        {
            services.AddSingleton<IDatabase>(c =>
            {
                return new MySqlDatabase("Harvester", Configuration);
            });

            services.AddTransient<IHarvesterRepository, HarvesterRepository>();
			services.AddTransient<IBlacklistedSweepsUsersRepository, BlacklistedSweepsUsersRepository>();
			services.AddTransient<ISweepPrizeRepository, SweepPrizeRepository>();
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

        private static void AddRulesClient(IServiceCollection services)
        {
            var rulesBaseApi = Configuration["PromoRulesUrl"];

            services.AddHttpClient<IRulesClient, RulesClient>(client =>
            {
                client.BaseAddress = new Uri(rulesBaseApi);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

        private static void AddSweepsClient(IServiceCollection services)
        {
            var sweepsBaseApi = Configuration["PromoSweepsUrl"];

            services.AddHttpClient<ISweepsClient, SweepsClient>(client =>
            {
                client.BaseAddress = new Uri(sweepsBaseApi);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

		private static void AddReportingClient(IServiceCollection services)
		{
			var reportingApi = Configuration["PromoReportUrl"];

			services.AddHttpClient<IReportingClient, ReportingClient>(client =>
			{
				client.BaseAddress = new Uri(reportingApi);
				client.DefaultRequestHeaders.Add("Accept", "application/json");
			});
		}

		private static void AddCampaignsClient(IServiceCollection services)
        {
            var campaignBaseApi = Configuration["PromoCampaignsUrl"];

            services.AddHttpClient<ICampaignsClient, CampaignsClient>(client =>
            {
                client.BaseAddress = new Uri(campaignBaseApi);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

        private static void ConfigureAppConfiguration(HostBuilderContext hostBuilder, IConfigurationBuilder configurationBuilder)
        {
            Configuration = configurationBuilder.Build();
        }

        private static void AddTwitterClient(IServiceCollection services)
        {
            services.Configure<TwitterAccount>(Configuration);
            services.Configure<TwitterAccount>(Configuration.GetSection(TwitterAccount.SectionName));
            services.AddTransient(p => p.GetService<IOptions<TwitterAccount>>().Value);

            services.AddTransient<ITwitterClient, TwitterClient>(c => GenerateTwitterClient(c));
        }

        private static TwitterClient GenerateTwitterClient(IServiceProvider c)
        {
            var twitterAccount = c.GetService<TwitterAccount>();
            var userCredentials = new TwitterCredentials(
                twitterAccount.ConsumerKey, 
                twitterAccount.ConsumerSecret, 
                twitterAccount.AccessToken, 
                twitterAccount.AccessTokenSecret);

            var client = new TwitterClient(userCredentials);
            client.Config.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            return client;
        }
    }
}
