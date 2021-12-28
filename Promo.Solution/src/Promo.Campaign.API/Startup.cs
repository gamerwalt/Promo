using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Promo.Campaign.Core.RequestHandlers;
using Promo.Campaign.Infrastructure;
using Promo.Clients.Internal;
using Promo.Core;
using Promo.Core.Authentication;
using Promo.Core.Authentication.ConfigurationOptions;
using Promo.Core.Exceptions;
using Promo.Core.Models;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Tweetinvi;

namespace Promo.Campaign.API
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Promo.Campaign.API", Version = "v1" });
            });

            services.AddSingleton<IDatabase>(c =>
            {
                return new MySqlDatabase("Campaign", Configuration);
            });

            var domainAssembly = typeof(CreateCampaignRequest).GetTypeInfo().Assembly;
            services.AddMediatR(domainAssembly);
            services.AddFluentValidation(new[] { domainAssembly });

            services.AddTransient<ICampaignTypesRepository, CampaignTypesRepository>();
            services.AddTransient<ICampaignRepository, CampaignRepository>();

            AddSweepsClient(services);
            AddRulesClient(services);
            AddHrvsterServiceAuthenticationclient(services);

            AddGoogleServicesAuthentication(services);

            AddTwitterClient(services);
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

            var client = new TwitterClient(
                twitterAccount.ConsumerKey, 
                twitterAccount.ConsumerSecret, 
                twitterAccount.AccessToken, 
                twitterAccount.AccessTokenSecret
            );
            client.Config.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            return client;
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

        private void AddHrvsterServiceAuthenticationclient(IServiceCollection services)
        {
            services.Configure<HrvsterServiceAccount>(Configuration);
            services.Configure<HrvsterServiceAccount>(Configuration
                .GetSection(HrvsterServiceAccount.SectionName));
            services.AddTransient(p => p.GetService<IOptions<HrvsterServiceAccount>>().Value);

            services.AddHttpClient<IServiceAuthenticate, ServiceAuthenticate>(client =>
            {
                client.BaseAddress = new Uri(Configuration["GoogleAuthenticateUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
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

        private void AddGoogleServicesAuthentication(IServiceCollection services)
        {
            var client = new HttpClient();
            var jsonResult = client.GetStringAsync(Configuration["GoogleSecureTokenCertificateUrl"]).Result;
            var metaData = JObject.Parse(jsonResult)
                .Children()
                .Cast<JProperty>()
                .Select(i => new x509Metadata(i.Path, i.Value.ToString()));

            var issuerSigningKeys = metaData.Select(s => s.X509SecurityKey);

            var googleServicesOptions = Configuration.GetSection(GoogleServicesOptions.GoogleServices).Get<GoogleServicesOptions>();

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
                        ValidateLifetime = true,
                        IssuerSigningKeys = issuerSigningKeys,
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Promo.Campaign.API v1"));
            }

            app.ConfigureExceptionHandler(app.ApplicationServices.GetRequiredService<ILogger<Startup>>());

            loggerFactory.AddFile(@"C:\Logs\log.txt");

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
