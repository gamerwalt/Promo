using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Promo.Core;
using Promo.Core.ConfigurationOptions;
using Promo.Core.Configurations;
using Promo.Core.Repositories.MySqlRepositories;
using System.Linq;
using System.Net.Http;
using System.Reflection;

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
                return new MySqlDatabase("hrvster", Configuration);
            });

            var domainAssembly = typeof(CoreRequest).GetTypeInfo().Assembly;
            services.AddMediatR(domainAssembly);

            services.AddRepositories();

            AddGoogleServicesAuthentication(services);
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Promo.Campaign.API v1"));
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
