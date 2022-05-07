using FluentValidation;
using FluentValidation.AspNetCore;
using hrvster.Core.Requests;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.Notifications.Handlers;
using hrvster.Core.Requests.Notifications.Harvester;
using hrvster.Core.Requests.Validators;
using hrvster.Core.Requests.WebRequests;
using hrvster.Core.Requests.WebRequests.Validators;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Harvester;
using hrvster.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promo.Clients.External;
using Promo.Configuration.Extensions;
using Serilog;
using System;
using System.Reflection;
using Tweetinvi;
using Tweetinvi.Models;

namespace hrvster.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddMvc().AddSessionStateTempDataProvider();
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromSeconds(30);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

			AddDatabaseAndRepositories(services);
			AddMediatorAndFluentValidation(services);
			AddTwitterClient(services);

			services.AddHttpClient<IIdentityClient, IdentityClient>();
			services.AddGoogleServices(Configuration);
			services.AddFirebaseServices(Configuration);
		}

		private static void AddMediatorAndFluentValidation(IServiceCollection services)
		{
			var handlerAssembly = typeof(GetAccountRequest).GetTypeInfo().Assembly;
			var notificationAssembly = typeof(AccountCreatedViaFirebase).GetTypeInfo().Assembly;
			var notificationHandlerAssembly = typeof(StoreAccountInDatabase).GetTypeInfo().Assembly;
			var validatorAssembly = typeof(CreateSweepValidator).GetTypeInfo().Assembly;
			var harvsterHandlerAssembly = typeof(NewHashTagTweetReceived).GetTypeInfo().Assembly;
			var webRequestAssembly = typeof(SigninForm).GetTypeInfo().Assembly;
			var webRequestValidatorAssembly = typeof(SigninFormValidator).GetTypeInfo().Assembly;

			services.AddMediatR(new[] { handlerAssembly, notificationAssembly, notificationHandlerAssembly, harvsterHandlerAssembly, webRequestAssembly });
			services.AddFluentValidation(f =>
			{
				f.RegisterValidatorsFromAssemblies(new[] { handlerAssembly, validatorAssembly, webRequestValidatorAssembly });
				f.DisableDataAnnotationsValidation = true;
			});
		}

		private void AddDatabaseAndRepositories(IServiceCollection services)
		{
			services.AddSingleton<IDatabase>(c =>
			{
				return new MySqlDatabase("hrvster", Configuration);
			});

			services.AddTransient<IAccountsRepository, AccountsRepository>();
			services.AddTransient<ICampaignRepository, CampaignRepository>();
			services.AddTransient<ICampaignTypesRepository, CampaignTypesRepository>();
			services.AddTransient<IRulesRepository, RulesRepository>();
			services.AddTransient<IWatchTypeCampaignTypesRepository, WatchTypeCampaignTypesRepository>();
			services.AddTransient<ISweepsRepository, SweepsRepository>();
			services.AddTransient<ISweepsEntriesRepository, SweepsEntriesRepository>();
			services.AddTransient<IEntryRulesRepository, EntryRulesRepository>();
			services.AddTransient<IDrawRuleRepository, DrawRuleRepository>();
			services.AddTransient<ISweepWinnersRepository, SweepWinnersRepository>();
			services.AddTransient<ISweepPrizesRepository, SweepPrizesRepository>();
			services.AddTransient<IReportingRepository, ReportingRepository>();
			services.AddTransient<IBlacklistedSweepsUsersRepository, BlacklistedSweepsUsersRepository>();
			services.AddTransient<IHarvesterRepository, HarvesterRepository>();
			services.AddTransient<ISweepPrizeRepository, SweepPrizeRepository>();
		}

		private void AddTwitterClient(IServiceCollection services)
		{
			services.Configure<TwitterAccount>(Configuration);
			services.Configure<TwitterAccount>(Configuration.GetSection(TwitterAccount.SectionName));
			services.AddTransient(p => p.GetService<IOptions<TwitterAccount>>().Value);

			services.AddTransient<ITwitterClient, TwitterClient>(c => GenerateTwitterClient(c));
		}

		private TwitterClient GenerateTwitterClient(IServiceProvider c)
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

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseSession();

			app.ConfigureExceptionHandler(app.ApplicationServices.GetRequiredService<ILogger<Startup>>());
			app.UseSerilogRequestLogging();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
