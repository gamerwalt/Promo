using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Promo.Core.External;
using Promo.Core.Repositories.MySqlRepositories;
using System;

namespace Promo.Core.Configurations
{
    public static class Configuration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IAccountsRepository, AccountsRepository>();
            services.AddTransient<ICampaignTypesRepository, CampaignTypesRepository>();
            services.AddTransient<ICampaignRepository, CampaignRepository>();
            services.AddTransient<IMailRepository, MailRepository>();
        }

        public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var sweepsBaseApi = configuration["PromoSweepsUrl"];

            services.AddHttpClient<ISweepsClient, SweepsClient>(client =>
            {
                client.BaseAddress = new Uri(sweepsBaseApi);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddHttpClient<IIdentityClient, IdentityClient>();
        }
    }
}
