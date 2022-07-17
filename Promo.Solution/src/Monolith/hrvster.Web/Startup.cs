using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using hrvster.ApplicationServices.Processors;
using hrvster.Core;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Core.Clients.Publishers;
using hrvster.Core.Handlers.Profiles;
using hrvster.Core.Mapper;
using hrvster.Core.Requests;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.Notifications.Handlers;
using hrvster.Core.Requests.Notifications.Harvester;
using hrvster.Core.Requests.Validators;
using hrvster.Core.Requests.WebRequests;
using hrvster.Core.Requests.WebRequests.Validators;
using hrvster.Core.Settings;
using hrvster.Extensions.Configuration;
using hrvster.Extensions.OptionsConfiguration;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Core;
using hrvster.Infrastructure.Harvester;
using hrvster.Infrastructure.Scheduling;
using hrvster.Models;
using hrvster.Web.Models;
using hrvster.Web.Validation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Promo.Clients.External;
using Promo.Configuration.Extensions;
using Promo.Core.Authentication.ConfigurationOptions;
using Serilog;
using System;
using System.Net;
using System.Reflection;
using Tweetinvi;
using Tweetinvi.Auth;
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
			services.AddMvc(o =>
			{
				o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
			}).AddSessionStateTempDataProvider();
			AddValidators(services);

			AddAuthentication(services);
			AddDatabaseAndRepositories(services);
			AddMediatorAndFluentValidation(services);
			AddTwitterClient(services);
			AddServicesOptions(services);
			ConfigureSweeepstakes(services);
			AddPublishers(services);
			
			services.AddTransient<IFileService, FileService>();
			services.AddSingleton<HrvsterUser>();
			services.AddSingleton<LinkBuilder>();
			services.AddAutoMapper(typeof(EntityMap));

			services.AddHttpClient<IIdentityClient, IdentityClient>();

			services.AddFirebaseServices(Configuration);

			services.AddTransient<IWatchTypeProvider, WatchTypeProvider>();
			services.AddTransient<ITwitterWatchTypeProvider, TwitterWatchTypeProvider>();

			services.AddNotyf(config =>
			{
				config.DurationInSeconds = 10;
				config.IsDismissable = true;
				config.Position = NotyfPosition.BottomCenter;
			});

			services.AddTransient<ITweetProcessor, TweetProcessor>();
			services.AddDrawSettingConfiguration(Configuration);
			services.AddRandomizer();
			services.AddEmailRepository();
		}


		private void AddValidators(IServiceCollection services)
		{
			services.AddValidatorsFromAssemblyContaining<CreateSweepPrizeValidator>();
		}

		private void AddPublishers(IServiceCollection services)
		{
			services.AddScoped<Core.Clients.Publishers.TwitterClient>();
			services.AddScoped<FacebookClient>();
			services.AddScoped<InstagramClient>();

			services.AddTransient<Func<ExternalClientType, IExternalClient>>(sp => serviceTypeName =>
			{
				return serviceTypeName switch
				{
					ExternalClientType.Twitter => sp.GetService<Core.Clients.Publishers.TwitterClient>(),
					ExternalClientType.Facebook => sp.GetService<FacebookClient>(),
					ExternalClientType.Instagram => sp.GetService<InstagramClient>(),
					_ => sp.GetService<Core.Clients.Publishers.TwitterClient>(),
				};
			});
		}

		private void AddAuthentication(IServiceCollection services)
		{
			var googleServicesOptions = Configuration.GetSection(GoogleServicesOptions.GoogleServices).Get<GoogleServicesOptions>();

			services.Configure<GoogleServicesOptions>(Configuration);
			services.Configure<GoogleServicesOptions>(Configuration.GetSection(GoogleServicesOptions.GoogleServices));
			services.AddTransient(p => p.GetService<IOptions<GoogleServicesOptions>>().Value);
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.Authority = googleServicesOptions.FirebaseAppId;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = googleServicesOptions.FirebaseAppId,
						ValidateAudience = true,
						ValidAudience = googleServicesOptions.ProjectId,
						ValidateLifetime = true
					};
				})
				.AddCookie(o =>
				{
					o.ExpireTimeSpan = TimeSpan.FromMinutes(30);
					o.SlidingExpiration = true;
					o.AccessDeniedPath = "/Denied";
					o.LoginPath = "/Session/SignIn";
				});

			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});
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
			var getUserProfileHandler = typeof(GetUserProfileHandler).GetTypeInfo().Assembly;

			services.AddMediatR(new[] { handlerAssembly, notificationAssembly, notificationHandlerAssembly, harvsterHandlerAssembly, webRequestAssembly, getUserProfileHandler });
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
			services.AddTransient<ISweepRulesRepository, SweepRulesRepository>();
			services.AddTransient<IWatchTypeCampaignTypesRepository, WatchTypeCampaignTypesRepository>();
			services.AddTransient<ISweepsRepository, SweepsRepository>();
			services.AddTransient<ISweepsEntriesRepository, SweepsEntriesRepository>();
			services.AddTransient<ISweepEntryRulesRepository, SweepEntryRulesRepository>();
			services.AddTransient<IDrawRuleRepository, DrawRuleRepository>();
			services.AddTransient<ISweepWinnersRepository, SweepWinnersRepository>();
			services.AddTransient<ISweepPrizesRepository, SweepPrizesRepository>();
			services.AddTransient<IReportingRepository, ReportingRepository>();
			services.AddTransient<IBlacklistedSweepsUsersRepository, BlacklistedSweepsUsersRepository>();
			services.AddTransient<IHarvesterRepository, HarvesterRepository>();
			services.AddTransient<ISweepPrizesRepository, SweepPrizesRepository>();
			services.AddTransient<IWatchTypesRepository, WatchTypesRepository>();
			services.AddTransient<IWatchTypeProvidersRepository, WatchTypeProvidersRepository>();
			services.AddTransient<IProfilesRepository, ProfilesRepository>();
			services.AddTransient<ISweepTweetSchedulerRepository, SweepTweetSchedulerRepository>();
		}

		private void AddServicesOptions(IServiceCollection services)
		{
			services.Configure<Services>(Configuration);
			services.Configure<Services>(Configuration.GetSection(Services.SectionName));
			services.AddTransient(p => p.GetService<IOptions<Services>>().Value);
		}

		private void ConfigureSweeepstakes(IServiceCollection services)
		{
			services.Configure<SweepConfiguration>(Configuration);
			services.Configure<SweepConfiguration>(Configuration.GetSection(SweepConfiguration.SectionName));
			services.AddTransient(p => p.GetService<IOptions<SweepConfiguration>>().Value);
		}

		private void AddTwitterClient(IServiceCollection services)
		{
			services.Configure<TwitterAccount>(Configuration);
			services.Configure<TwitterAccount>(Configuration.GetSection(TwitterAccount.SectionName));
			services.AddTransient(p => p.GetService<IOptions<TwitterAccount>>().Value);

			services.Configure<hrvster.Models.Options.FileServerOptions>(Configuration);
			services.Configure<hrvster.Models.Options.FileServerOptions>(Configuration.GetSection(hrvster.Models.Options.FileServerOptions.SectionName));
			services.AddTransient(p => p.GetService<IOptions<hrvster.Models.Options.FileServerOptions>>().Value);

			services.AddTransient<ITwitterClient, Tweetinvi.TwitterClient>(c => GenerateTwitterClient(c));

			services.AddSingleton<IAuthenticationRequestStore, LocalAuthenticationRequestStore>();
		}

		private Tweetinvi.TwitterClient GenerateTwitterClient(IServiceProvider c)
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

			//app.ConfigureExceptionHandler(app.ApplicationServices.GetRequiredService<ILogger<Startup>>());
			app.UseSerilogRequestLogging();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseNotyf();

			app.UseStatusCodePages(async context =>
			{
				var response = context.HttpContext.Response;

				if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
					response.StatusCode == (int)HttpStatusCode.Forbidden)
					response.Redirect("/Session/SignIn");
			});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
