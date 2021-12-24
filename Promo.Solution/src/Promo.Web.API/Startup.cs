using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Promo.Clients.External;
using Promo.Core;
using Promo.Core.Authentication.ConfigurationOptions;
using Promo.Membership.Core.RequestHandlers;
using Promo.Membership.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace Promo.Web.API
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Promo.Membership.API", Version = "v1" });
            });

            services.AddSingleton<IDatabase>(c =>
            {
                return new MySqlDatabase("Identity", Configuration);
            });

            services.AddTransient<IAccountsRepository, AccountsRepository>();

            services.AddHttpClient<IIdentityClient, IdentityClient>();

            var domainAssembly = typeof(GetAccountRequest).GetTypeInfo().Assembly;
            services.AddMediatR(domainAssembly);
            services.AddFluentValidation(new[] { domainAssembly });

            AddGoogleServicesAuthentication(services);
            AddFirebaseServices(services, Configuration);
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

        private void AddFirebaseServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<FirebaseApp>(c => BuildFirebaseApp(configuration));
        }

        private FirebaseApp BuildFirebaseApp(IConfiguration configuration)
        {
            var firebaseConfig = new FirebaseConfig();
            configuration.Bind(FirebaseConfig.Firebase, firebaseConfig);

            var firebaseConfigSerialized = JsonConvert.SerializeObject(firebaseConfig);

            return FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(firebaseConfigSerialized),
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Promo.Web.API v1"));
            }
            else
            {
                //setup ExceptionHandler
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
