using hrvster.Core.Clients.Publishers;
using hrvster.Extensions.Configuration;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using Tweetinvi;
using Tweetinvi.Auth;
using Tweetinvi.Models;

namespace hrvster.Services
{
	public class Program
	{
		private static IConfiguration Configuration;

		public static void Main(string[] args)
		{
			var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			Configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env}.json")
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(Configuration)
				.CreateLogger();

			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog((context, configuration) =>
					configuration.ReadFrom.Configuration(context.Configuration)
						.WriteTo.Loggly()
						.Enrich.FromLogContext()
						.MinimumLevel.Debug(), preserveStaticLogger: true)
				.ConfigureServices((hostContext, services) =>
				{
					Log.Information("Starting Host for hrvster.Services");

					services.AddHostedService<HostedServices>();
					services.AddSingleton<IService, ScheduledTweetService>();
					services.AddSingleton<IService, TwitterHarvestingService>();
					services.AddRepositories();

					AddDatabaseAndRepositories(services);

					AddMediator(services);
					AddPublishers(services);
					AddTwitterClient(services);
				});

		private static void AddTwitterClient(IServiceCollection services)
		{
			services.Configure<TwitterAccount>(Configuration);
			services.Configure<TwitterAccount>(Configuration.GetSection(TwitterAccount.SectionName));
			services.AddTransient(p => p.GetService<IOptions<TwitterAccount>>().Value);

			services.Configure<hrvster.Models.Options.FileServerOptions>(Configuration);
			services.Configure<hrvster.Models.Options.FileServerOptions>(Configuration.GetSection(hrvster.Models.Options.FileServerOptions.SectionName));
			services.AddTransient(p => p.GetService<IOptions<Models.Options.FileServerOptions>>().Value);

			services.AddTransient<ITwitterClient, Tweetinvi.TwitterClient>(c => GenerateTwitterClient(c));

			services.AddSingleton<IAuthenticationRequestStore, LocalAuthenticationRequestStore>();
		}

		private static Tweetinvi.TwitterClient GenerateTwitterClient(IServiceProvider c)
		{
			var twitterAccount = c.GetService<TwitterAccount>();
			var userCredentials = new TwitterCredentials(
				twitterAccount.ConsumerKey,
				twitterAccount.ConsumerSecret,
				twitterAccount.AccessToken,
				twitterAccount.AccessTokenSecret);

			var client = new Tweetinvi.TwitterClient(userCredentials);
			client.Config.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

			return client;
		}

		private static void AddPublishers(IServiceCollection services)
		{
			services.AddTransient<Core.Clients.Publishers.TwitterClient>();
			services.AddTransient<FacebookClient>();
			services.AddTransient<InstagramClient>();

			services.AddTransient<Func<ExternalClientType, Core.Clients.Publishers.IExternalClient>>(sp => serviceTypeName =>
			{
				switch (serviceTypeName)
				{
					case ExternalClientType.Twitter:
						return sp.GetService<Core.Clients.Publishers.TwitterClient>();
					case ExternalClientType.Facebook:
						return sp.GetService<FacebookClient>();
					case ExternalClientType.Instagram:
						return sp.GetService<InstagramClient>();
					default:
						return sp.GetService<Core.Clients.Publishers.TwitterClient>();
				}
			});
		}

		private static void AddMediator(IServiceCollection services)
		{
			services.AddMediatR(typeof(Program));
		}

		private static void AddDatabaseAndRepositories(IServiceCollection services)
		{
			services.AddSingleton<IDatabase>(c =>
			{
				return new MySqlDatabase("hrvster", Configuration);
			});

			LoadServices(services);
		}

		private static void LoadServices(IServiceCollection services)
		{
			var sp = services.BuildServiceProvider();
			var repository = sp.GetService<IServicesRepository>();

			var serviceContainer = repository.GetServicesEnabledAsync();

			services.AddServiceContainer(serviceContainer);
		}
	}
}
