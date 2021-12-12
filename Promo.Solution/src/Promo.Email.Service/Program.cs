using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Promo.Core;
using Promo.Core.Repositories.MySqlRepositories;
using Promo.Email.Service.HostedServices;
using Promo.Email.Service.Services;

namespace Promo.Email.Service
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
                    services.AddHostedService<PromoHostedService>();
                    services.AddSingleton<IDatabase>(c =>
                    {
                        return new MySqlDatabase("Identity", Configuration);
                    });

                    services.AddSingleton<IMailService, MailService>();
                    services.AddSingleton<IVerifyAccountService, VerifyAccountService>();

                    services.AddSingleton<IMailRepository, MailRepository>();
                    services.AddSingleton<IAccountsRepository, AccountsRepository>();

                    services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
                });

        private static void ConfigureAppConfiguration(HostBuilderContext hostBuilder, IConfigurationBuilder configurationBuilder)
        {
            Configuration = configurationBuilder.Build();
        }
    }
}
