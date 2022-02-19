using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Promo.Core.Authentication.ConfigurationOptions;
using Serilog;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;

namespace Promo.Gateway.API
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
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Promo.Gateway.API", Version = "v1" });
			});

			AddGoogleServicesAuthentication(services);
			AddFirebaseServices(services, Configuration);

			services.AddOcelot(Configuration);
		}

		private void AddGoogleServicesAuthentication(IServiceCollection services)
		{
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
						ValidateLifetime = true
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
		public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Promo.Gateway.API v1"));
			}
			else
			{
				app.UseHttpsRedirection();
			}

			await app.UseOcelot();

			app.UseSerilogRequestLogging();

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
