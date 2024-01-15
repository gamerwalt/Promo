using hrvster.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MySqlConnector;
using NToastNotify;
using Testcontainers.MySql;

namespace hrvster.Web.Integration.Tests
{
    public class IntegrationTestWebAppFactory<TFeature> : WebApplicationFactory<Program>, IAsyncLifetime where TFeature : class
    {
        public TestServer TestServer { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }
        public IServiceCollection ServiceCollection { get; private set; }

        private readonly MySqlContainer hrvsterDbContainerBuilder = new MySqlBuilder()
                .WithImage("mysql:latest")
                .WithDatabase("hrvster")
                .WithUsername("root")
                .WithPassword("Password!123")
            .Build();

        public Task InitializeAsync()
        {
            return hrvsterDbContainerBuilder.StartAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var hrvsterDatabaseConnectionString = hrvsterDbContainerBuilder.GetConnectionString();

                var testConfigBuilder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.test.json")
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "ConnectionStrings:hrvster", hrvsterDatabaseConnectionString },
                        { "ConnectionStrings:Harvester", hrvsterDatabaseConnectionString },
                        { "ConnectionStrings:Reporting", hrvsterDatabaseConnectionString}
                    });
                var testConfig = testConfigBuilder.Build();

                var databaseService = services.SingleOrDefault(d => d.ServiceType == typeof(IDatabase));
                var httpContext = services.SingleOrDefault(d => d.ServiceType == typeof(HttpContext));
                var notificationService = services.SingleOrDefault(d => d.ServiceType == typeof(IToastNotification));

                if (databaseService is not null)
                {
                    services.Remove(databaseService);
                    services.AddSingleton<IDatabase>(c =>
                    {
                        return new MySqlDatabase(DatabaseNames.Main, testConfig, DatabaseNames.Harvester, DatabaseNames.Reporting);
                    });
                }

                if(httpContext is not null)
                {
                    services.Remove(httpContext);
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    httpContextMock.Setup(hc => hc.Request).Returns(httpRequestMock.Object);
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    httpContextAccessorMock.Setup(hca => hca.HttpContext).Returns(httpContextMock.Object);
                    services.AddSingleton(httpContextAccessorMock.Object);
                }

                if(notificationService is not null)
                {
                    services.Remove(notificationService);
                    services.AddSingleton(c =>
                    {
                        return new Mock<IToastNotification>().Object;
                    });
                }

                SetupHrvsterDatabase();

                ServiceCollection = services;
                ServiceProvider = services.BuildServiceProvider();
            });

            TestServer = Server;
        }

        private void SetupHrvsterDatabase()
        {
            var databaseScriptPath = Path.Combine(AppContext.BaseDirectory, "hrvsterDatabase.sql");
            var procedureScriptPath = Path.Combine(AppContext.BaseDirectory, "hrvsterStoredProcedures.sql");
            var staticDataScriptPath = Path.Combine(AppContext.BaseDirectory, "hrvsterStaticData.sql");

            if (!File.Exists(databaseScriptPath))
            {
                throw new FileNotFoundException($"Expected sql file not found at {databaseScriptPath}.");
            }

            if(!File.Exists(procedureScriptPath))
            {
                throw new FileNotFoundException($"Expected sql file not found at {procedureScriptPath}.");
            }

            if(!File.Exists(staticDataScriptPath))
            {
                throw new FileNotFoundException($"Expected sql file not found at {staticDataScriptPath}.");
            }

            var databaseScript = File.ReadAllText(databaseScriptPath);
            var procedureScript = File.ReadAllText(procedureScriptPath);
            var staticDataScript = File.ReadAllText(staticDataScriptPath);

            using var connection = new MySqlConnection(hrvsterDbContainerBuilder.GetConnectionString());
            connection.Open();

            using var command = new MySqlCommand(databaseScript, connection);
            command.ExecuteNonQuery();

            using var procedureCommands = new MySqlCommand(procedureScript, connection);
            procedureCommands.ExecuteNonQuery();

            using var staticDataCommands = new MySqlCommand(staticDataScript, connection);
            staticDataCommands.ExecuteNonQuery();
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return hrvsterDbContainerBuilder.StopAsync();
        }
    }
}
