using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Promo.Core;
using Promo.Core.Authentication.ConfigurationOptions;
using Promo.Rules.Core.RequestHandlers;
using Promo.Rules.Infrastructure;
using System.Reflection;

namespace Promo.Rules.API
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Promo.Rules.API", Version = "v1" });
            });

            AddDataClasses(services);

            var domainAssembly = typeof(GetWatchTypeCampaignTypes).GetTypeInfo().Assembly;
            services.AddMediatR(domainAssembly);

            //Authentication
            AddGoogleServicesAuthentication(services);
        }

        private void AddDataClasses(IServiceCollection services)
        {
            services.AddSingleton<IDatabase>(c =>
            {
                return new MySqlDatabase("Rules", Configuration);
            });

            services.AddTransient<IWatchTypeCampaignTypesRepository, WatchTypeCampaignTypesRepository>();
            services.AddTransient<IRulesRepository, RulesRepository>();
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Promo.Rules.API v1"));
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
