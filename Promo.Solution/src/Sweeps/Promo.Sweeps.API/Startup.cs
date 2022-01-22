using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Promo.Clients.Internal;
using Promo.Core;
using Promo.Core.Authentication.ConfigurationOptions;
using Promo.Core.Models;
using Promo.Sweeps.Core.RequestHandlers;
using Promo.Sweeps.Infrastructure;
using System;
using System.Reflection;
using Tweetinvi;
using Tweetinvi.Models;

namespace Promo.Sweeps.API
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Promo.Sweeps.API", Version = "v1" });
            });

            services.AddSingleton<IDatabase>(c =>
            {
                return new MySqlDatabase("Sweeps", Configuration);
            });
            services.AddTransient<ISweepsRepository, SweepsRepository>();
            services.AddTransient<ISweepsEntriesRepository, SweepsEntriesRepository>();

            //Authentication
            AddGoogleServicesAuthentication(services);
			AddClients(services);
			AddTwitterClient(services);

			var domainAssembly = typeof(CreateSweep).GetTypeInfo().Assembly;
            services.AddMediatR(domainAssembly);
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

		private void AddClients(IServiceCollection services)
		{
			AddCampaignClient(services);
			AddRulesClient(services);
			AddWinnersClient(services);
			AddHarvesterClient(services);
			AddPrizesClient(services);
		}

		private void AddPrizesClient(IServiceCollection services)
		{
			var prizesBaseUrl = Configuration["PromoPrizesUrl"];

			services.AddHttpClient<IPrizesClient, PrizesClient>(client =>
			{
				client.BaseAddress = new Uri(prizesBaseUrl);
				client.DefaultRequestHeaders.Add("Accept", "application/json");
			});
		}

		private void AddHarvesterClient(IServiceCollection services)
		{
			var harvesterBaseUrl = Configuration["PromoHarvesterUrl"];

			services.AddHttpClient<IHarvesterClient, HarvesterClient>(client =>
			{
				client.BaseAddress = new Uri(harvesterBaseUrl);
				client.DefaultRequestHeaders.Add("Accept", "application/json");
			});
		}

		private void AddWinnersClient(IServiceCollection services)
		{
			var winnersBaseApi = Configuration["PromoWinnersUrl"];

			services.AddHttpClient<IWinnersClient, WinnersClient>(client =>
			{
				client.BaseAddress = new Uri(winnersBaseApi);
				client.DefaultRequestHeaders.Add("Accept", "application/json");
			});
		}

		private void AddRulesClient(IServiceCollection services)
		{
			var rulesBaseApi = Configuration["PromoRulesUrl"];

			services.AddHttpClient<IRulesClient, RulesClient>(client =>
			{
				client.BaseAddress = new Uri(rulesBaseApi);
				client.DefaultRequestHeaders.Add("Accept", "application/json");
			});
		}

		private void AddCampaignClient(IServiceCollection services)
		{
			var campaignBaseApi = Configuration["PromoCampaignsUrl"];

			services.AddHttpClient<ICampaignsClient, CampaignsClient>(client =>
			{
				client.BaseAddress = new Uri(campaignBaseApi);
				client.DefaultRequestHeaders.Add("Accept", "application/json");
			});
		}

		private void AddGoogleServicesAuthentication(IServiceCollection services)
        {
            var googleServicesOptions = Configuration
                .GetSection(GoogleServicesOptions.GoogleServices)
                .Get<GoogleServicesOptions>();

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
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Promo.Sweeps.API v1"));
            }
			else
			{
				app.UseHttpsRedirection();
			}

			app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
