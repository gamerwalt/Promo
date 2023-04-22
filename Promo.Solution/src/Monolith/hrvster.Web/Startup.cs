using Facebook;
using FluentValidation;
using FluentValidation.AspNetCore;
using hrvster.Extensions.Configuration;
using hrvster.Extensions.OptionsConfiguration;
using hrvster.Models.Options;
using hrvster.Web.Mapper;
using hrvster.Web.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Net;

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
			services.AddControllersWithViews()
				.AddNToastNotifyToastr(new NToastNotify.ToastrOptions
				{
					ProgressBar = true,
					TimeOut = 10000,
					CloseButton = true,
				});
			services.AddMvc(o =>
			{
				o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
			}).AddSessionStateTempDataProvider();
			AddValidators(services);

			services.AddAntiforgeryConfig()
				.AddHrvsterAuthentication(Configuration)
				.AddDatabaseConfiguration(Configuration)
				.AddCampaignRepositories()
				.AddAccountRepositories()
				.AddAccountsService()
				.AddSubscriptionRepositories()
				.AddSweepstakesRepositories()
				.AddSweepstakesServices()
				.AddGameService()
				.AddEmailRepository()
				.AddWatchTypeRepositories()
				.AddHarvesterAndReportingRepositories()
				.AddHarvsterConfigurationOption(Configuration)
				.AddHarvesterService()
				.AddHarvesterServiceCommandRepository()
				.AddMediatorAndFluentValidation()
				.AddTwitterClient(Configuration)
				.AddServicesOptions(Configuration)
				.AddSweeepstakesOptions(Configuration)
				.AddPublishers()
				.AddSingletons()
				.AddAutomapperConfiguration(typeof(EntityMap))
				.AddHttpClients()
				.AddFirebaseServices(Configuration)
				.AddAuthenticationForWatchProviders()
				.AddTweetProcessors()
				.AddDrawSettingConfiguration(Configuration)
				.AddRandomizer()
				.AddPromotionConfigurationOptions(Configuration)
				.AddNotificationConfigurationOptions(Configuration)
				.AddUpdateValidators()
				.AddFluentValidationAutoValidation()
				.AddUserRateLimiterService()
				.AddHttpContextAccessor()
				.AddFirebaseAuthManager()
				.AddSessionManager()
				.AddSubscriptionService()
				.AddPaymentServices(Configuration)
				.AddMatchGameRepositories()
				.AddMatchGameService()
				.AddMatchEngineServices()
				.AddCampaignService()
				.AddPaymentServices(Configuration)
				.AddHttpClient()
				.AddFlutterwaveHttpClient()
				.AddSignatureUtility()
				.AddCacheService()
				.AddReportingService()
				.AddProvidersService();

			services.AddTransient(provider => new FacebookClient());
		}


		private void AddValidators(IServiceCollection services)
		{
			services.AddValidatorsFromAssemblyContaining<CreateSweepPrizeValidator>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment() || env.EnvironmentName.Equals("local"))
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

			//app.UseNotyf();
			app.UseNToastNotify();

			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.All
			});

			app.UseStatusCodePages(async context =>
			{
				var response = context.HttpContext.Response;

				if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
					response.StatusCode == (int)HttpStatusCode.Forbidden)
				{
					var url = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
					if (string.IsNullOrWhiteSpace(url) || url.Equals("/"))
						response.Redirect("/Session/SignIn");
					else
						response.Redirect($"/Session/SignIn?returnUrl={url}");
				}
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
