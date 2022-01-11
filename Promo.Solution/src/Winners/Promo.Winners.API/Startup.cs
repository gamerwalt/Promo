using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Promo.Core;
using Promo.Core.Authentication.ConfigurationOptions;
using Promo.Winners.Core.RequestHandlers;
using Promo.Winners.Infrastructure;
using Serilog;
using System;
using System.Reflection;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Promo.Winners.API
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
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Promo.Winners.API", Version = "v1" });
			});
			services.AddSingleton(typeof(ILogger), typeof(Logger<Startup>));
			
			AddMediatorConfiguration(services);
			AddDatabaseConfiguration(services);
			AddGoogleServicesAuthenticationConfiguration(services);
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
			var domainAssembly = typeof(CreateWinners).GetTypeInfo().Assembly;
			services.AddMediatR(domainAssembly);
		}

		private void AddDatabaseConfiguration(IServiceCollection services)
		{
			services.AddSingleton<IDatabase>(c =>
			{
				return new MySqlDatabase("Winners", Configuration);
			});

			services.AddTransient<ISweepWinnersRepository, SweepWinnersRepository>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Promo.Winners.API v1"));
			}
			else
			{
				app.UseHttpsRedirection();
			}

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
