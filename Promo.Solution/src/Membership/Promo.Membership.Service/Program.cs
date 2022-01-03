using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Promo.Clients.Internal;
using Promo.Core;
using Promo.Core.Authentication;
using Promo.Core.Models;
using Promo.Membership.Infrastructure;
using Promo.Membership.Service.Services;
using System;

namespace Promo.Membership.Service
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
                    services.AddHostedService<HostedService>();
                    services.AddSingleton<IDatabase>(c =>
                    {
                        return new MySqlDatabase("Membership", Configuration);
                    });

                    services.AddSingleton<IVerifyAccountService, VerifyAccountService>();

                    services.AddSingleton<IAccountsRepository, AccountsRepository>();

                    services.AddHttpClient<IMailClient, MailClient>(client =>
                    {
                        client.BaseAddress = new Uri(Configuration["PromoMailUrl"]);
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                    });
                    services.AddHttpClient<IServiceAuthenticate, ServiceAuthenticate>(client =>
                    {
                        client.BaseAddress = new Uri(Configuration["GoogleAuthenticateUrl"]);
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                    });

                    services.Configure<HrvsterServiceAccount>(Configuration);
                    services.Configure<HrvsterServiceAccount>(Configuration
                        .GetSection(HrvsterServiceAccount.SectionName));
                    services.AddTransient(p => p.GetService<IOptions<HrvsterServiceAccount>>().Value);
                });

        private static void ConfigureAppConfiguration(HostBuilderContext hostBuilder, IConfigurationBuilder configurationBuilder)
        {
            Configuration = configurationBuilder.Build();
        }
    }
}
