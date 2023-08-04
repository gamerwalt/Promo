using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.Harvesters.Facebook;
using hrvster.ApplicationServices.MatchGames;
using hrvster.ApplicationServices.Publishing;
using hrvster.ApplicationServices.Subscription;
using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Core.Clients.Publishers;
using hrvster.Extensions.Configuration;
using hrvster.Extensions.OptionsConfiguration;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Facebook;
using hrvster.Infrastructure.Harvester;
using hrvster.Infrastructure.MatchGames;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Middleware;
using hrvster.Models;
using hrvster.Models.Options;
using hrvster.Template1.Web.MapperProfiles;
using hrvster.Templates.Core.Common;
using hrvster.Templates.Infrastructure.Customizations;
using hrvster.Templates.Services;
using MediatR;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using Tweetinvi;
using Tweetinvi.Auth;
using Tweetinvi.Models;
using TwitterClient = hrvster.Core.Clients.Publishers.TwitterClient;

namespace hrvster.Template1.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			builder.Configuration
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env}.json")
				.Build();

			var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
			telemetryConfiguration.InstrumentationKey = builder.Configuration
				.GetSection("ApplicationInsights:InstrumentationKey")
				.Value;

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(builder.Configuration)
				.Enrich.FromLogContext()
				.MinimumLevel.Verbose()
				.CreateLogger();

			Log.Information("Running the hrvster.Template1.Web app.");

			// Add services to the container.
			builder.Services.AddSession();
			builder.Host
				.UseSerilog((context, configuration)
							=> configuration.ReadFrom.Configuration(builder.Configuration)
								.Enrich.FromLogContext()
								.WriteTo.Loggly()
								.WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces)
								.MinimumLevel.Verbose(), preserveStaticLogger: true);
			Log.Information("Use Serilog applied on hrvster.Template1.Web app.");

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("DynamicCorsPolicy",
				builder =>
				{
					builder.SetIsOriginAllowed(origin =>
					{
						var uri = new Uri(origin);
						return uri.Host.EndsWith(".hrvster.com");
					})
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowCredentials(); // Only needed if your client needs to send credentials with the request
				});
			});

			Log.Information("Added DynamicCorsPolicy.");

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = "/Authentication/Login";
				});

			Log.Information("Added Cookies auth scheme.");

			builder.Services.AddControllersWithViews();

			builder.Services.AddMvc(o =>
			{
				o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
			})
			.AddSessionStateTempDataProvider();

			builder.Services.AddAutoMapper(typeof(EntityMapper));

			builder.Services.AddHttpContextAccessor();
			builder.Services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();
			//builder.Services.AddSingleton<IDatabase>(c =>
			//{
			//	return new MySqlDatabase("hrvster", builder.Configuration);
			//});
			builder.Services.AddSingleton<IDatabase>(c =>
			{
				return new MySqlDatabase(DatabaseNames.Main, builder.Configuration, DatabaseNames.Harvester, DatabaseNames.Reporting);
			});

			builder.Services.Configure<TwitterAccount>(builder.Configuration)
				.Configure<TwitterAccount>(builder.Configuration.GetSection(TwitterAccount.SectionName))
				.AddTransient(p => p.GetService<IOptions<TwitterAccount>>().Value)
				.AddTransient<ITwitterClient, Tweetinvi.TwitterClient>(c => GenerateTwitterClient(c))
				.AddSingleton<IAuthenticationRequestStore, LocalAuthenticationRequestStore>();

			builder.Services.Configure<FacebookConfigOptions>(builder.Configuration)
				.Configure<FacebookConfigOptions>(builder.Configuration.GetSection(FacebookConfigOptions.SectionName))
				.AddTransient(s => s.GetService<IOptions<FacebookConfigOptions>>().Value);

			builder.Services.AddTransient<ISessionManager, SessionManager>();

			//Providers
			builder.Services.AddTransient<IWatchTypeProvider, WatchTypeProvider>();
			builder.Services.AddTransient<ITwitterWatchTypeProvider, TwitterWatchTypeProvider>();
			builder.Services.AddTransient<IFacebookWatchTypeProvider, FacebookWatchTypeProvider>();

			//Template Repositories
			builder.Services.AddTransient<ILayoutRepository, LayoutRepository>();
			builder.Services.AddTransient<IMemberProfileRepository, MemberProfileRepository>();
			builder.Services.AddTransient<IMemberRepository, MemberRepository>();


			//hrvster Repositories
			builder.Services.AddTransient<IAccountCompanyRepository, AccountCompanyRepository>();
			builder.Services.AddTransient<ISweepsRepository, SweepsRepository>();
			builder.Services.AddTransient<IAccountSubscriptionsRepository, AccountSubscriptionsRepository>();
			builder.Services.AddTransient<ISweepsEntriesRepository, SweepsEntriesRepository>();
			builder.Services.AddTransient<ISweepRulesRepository, SweepRulesRepository>();
			builder.Services.AddTransient<IMatchGamesRepository, MatchGamesRepository>();
			builder.Services.AddTransient<IMatchGameRulesRepository, MatchGameRulesRepository>();
			builder.Services.AddTransient<IMatchRulePageRepository, MatchRulePageRepository>();
			builder.Services.AddTransient<IProfilesRepository, ProfilesRepository>();
			builder.Services.AddTransient<IFacebookPageRepository, FacebookPageRepository>();
			builder.Services.AddTransient<IAccountsRepository, AccountsRepository>();
			builder.Services.AddTransient<ISweepEntryRulesRepository, SweepEntryRulesRepository>();
			builder.Services.AddTransient<IBlacklistedSweepsUsersRepository, BlacklistedSweepsUsersRepository>();
			builder.Services.AddTransient<IBlacklistedUserRepository, BlacklistedUserRepository>();
			builder.Services.AddTransient<IReportingRepository, ReportingRepository>();
			builder.Services.AddTransient<ISweepNotificationRepository, SweepNotificationRepository>();
			builder.Services.AddTransient<IMailRepository, MailRepository>();
			builder.Services.AddTransient<ICampaignRepository, CampaignRepository>();
			builder.Services.AddMatchGameRepositories();
			builder.Services.AddPostSchedulers();
			builder.Services.AddWatchTypeRepositories();

			builder.Services.AddTransient<TwitterClient>()
				.AddTransient<InstagramClient>()
				.AddTransient<Func<ExternalClientType, IExternalClient>>(sp => serviceTypeName =>
				{
					switch (serviceTypeName)
					{
						case ExternalClientType.Twitter:
							return sp.GetService<TwitterClient>();
						case ExternalClientType.Facebook:
							return sp.GetService<FacebookClient>();
						case ExternalClientType.Instagram:
							return sp.GetService<InstagramClient>();
						default:
							return sp.GetService<TwitterClient>();
					}
				})
				.AddTransient<IPublishService, PublishService>();

			builder.Services.AddMediatR(typeof(Program));
			builder.Services.AddHarvsterConfigurationOption(builder.Configuration);

			builder.Services.AddScoped<ILayoutService, LayoutService>();
			builder.Services.AddTransient<IGameService, GameService>();
			builder.Services.AddTransient<ISweepstakesService, SweepstakesService>();
			builder.Services.AddTransient<IMatchGamesService, MatchGamesService>();
			builder.Services.AddTransient<IMatchRulesService, MatchRulesService>();
			builder.Services.AddTransient<IPublishService, PublishService>();
			builder.Services.AddTransient<IAccountsService, AccountsService>();
			builder.Services.AddTransient<IAccountSubscriptionService, AccountSubscriptionService>();
			builder.Services.AddTransient<IAccountCompanyService, AccountCompanyService>();
			builder.Services.AddTransient<IFacebookPageSerivce, FacebookPageSerivce>();
			builder.Services.AddTransient<ICampaignService, CampaignService>();
			builder.Services.AddTransient<IMatchGameplayService, MatchGameplayService>();
			builder.Services.AddTransient<IMatchGameBoardService, MatchGameBoardService>();
			builder.Services.AddTransient<IMatchPrizeService, MatchPrizeService>();
			builder.Services.AddReportingService();
			builder.Services.AddMatchEngineServices();
			builder.Services.AddMatchGameService();
			builder.Services.AddCacheService();

			builder.Services.AddSignatureUtility();
			builder.Services.AddLinkBuilder()
				.AddServicesOptions(builder.Configuration);

			builder.Services.AddTransient<IPlaySweepstakeService, PlaySweepstakeService>();

			builder.Services.AddTwitterHttpService(builder.Configuration);

			Log.Information("Done with adding services.");

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error/{0}");
				app.UseStatusCodePagesWithReExecute("/Error/{0}");

				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseSession();

			app.UseRouting();

			app.UseCors("DynamicCorsPolicy");

			app.UseMiddleware<SubdomainMiddleware>();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.All
			});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");

				endpoints.MapControllers(); // This is necessary for attribute routing to work.
			});

			app.Run();
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
	}
}