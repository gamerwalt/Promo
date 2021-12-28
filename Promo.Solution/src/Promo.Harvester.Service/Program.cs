using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Promo.Clients.Internal;
using Promo.Core;
using Promo.Core.Authentication;
using Promo.Core.Models;
using Promo.Harvester.Infrastructure;
using Promo.Harvester.Infrastruture.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;

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
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<HarvesterHostedService>();
                    services.AddTransient<IStreamService, StreamService>();
                    services.AddTransient<IHarvesterService, HarvesterService>();
                    

                    AddRulesClient(services);
                    AddTwitterClient(services);
                    AddHrvsterServiceAuthenticationclient(services);
                    AddDatabaseAndRepositories(services);
                });

        private static void AddDatabaseAndRepositories(IServiceCollection services)
        {
            services.AddSingleton<IDatabase>(c =>
            {
                return new MySqlDatabase("Harvester", Configuration);
            });

            services.AddTransient<IHarvesterRepository, HarvesterRepository>();
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

            var client = new TwitterClient(
                twitterAccount.ConsumerKey,
                twitterAccount.ConsumerSecret,
                twitterAccount.AccessToken,
                twitterAccount.AccessTokenSecret
            );
            client.Config.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            return client;
        }
    }
}
