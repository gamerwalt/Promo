using Microsoft.Extensions.DependencyInjection;
using Promo.Core.Repositories.MySqlRepositories;

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
    }
}
