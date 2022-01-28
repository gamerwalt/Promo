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
using Promo.Harvester.Core.RequestHandlers;
using Promo.Harvester.Infrastructure;
using System;
using System.Reflection;
using Tweetinvi;
using Tweetinvi.Models;

namespace Promo.Harvester.API
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Promo.Harvester.API", Version = "v1" });
            });

            AddMediatorConfiguration(services);
            AddDatabaseConfiguration(services);
            AddGoogleServicesAuthenticationConfiguration(services);

            AddRulesClient(services);
            AddCampaignsClient(services);
            AddSweepsClient(services);
            AddTwitterClient(services);
            AddReportingClient(services);
        }

        private void AddTwitterClient(IServiceCollection services)
        {
            services.Configure<TwitterAccount>(Configuration);
            services.Configure<TwitterAccount>(Configuration.GetSection(TwitterAccount.SectionName));
            services.AddTransient(p => p.GetService<IOptions<TwitterAccount>>().Value);

            services.AddTransient<ITwitterClient, TwitterClient>(c => GenerateTwitterClient(c));
        }

        private static TwitterClient GenerateTwitterClient(IServiceProvider c)
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

        private void AddSweepsClient(IServiceCollection services)
        {
            var sweepsBaseApi = Configuration["PromoSweepsUrl"];

            services.AddHttpClient<ISweepsClient, SweepsClient>(client =>
            {
                client.BaseAddress = new Uri(sweepsBaseApi);
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

        private void AddReportingClient(IServiceCollection services)
        {
            var reportingApi = Configuration["PromoReportUrl"];

            services.AddHttpClient<IReportingClient, ReportingClient>(client =>
            {
                client.BaseAddress = new Uri(reportingApi);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

        private void AddCampaignsClient(IServiceCollection services)
        {
            var campaignBaseApi = Configuration["PromoCampaignsUrl"];

            services.AddHttpClient<ICampaignsClient, CampaignsClient>(client =>
            {
                client.BaseAddress = new Uri(campaignBaseApi);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

        private void AddGoogleServicesAuthenticationConfiguration(IServiceCollection services)
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

        private static void AddMediatorConfiguration(IServiceCollection services)
        {
            var domainAssembly = typeof(GetHarvestedUser).GetTypeInfo().Assembly;
            services.AddMediatR(domainAssembly);
        }

        private void AddDatabaseConfiguration(IServiceCollection services)
        {
            services.AddSingleton<IDatabase>(c =>
            {
                return new MySqlDatabase("Harvester", Configuration);
            });

            services.AddTransient<IHarvesterRepository, HarvesterRepository>();
			services.AddTransient<ISweepPrizeRepository, SweepPrizeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Promo.Harvester.API v1"));
            }

            app.UseHttpsRedirection();

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
