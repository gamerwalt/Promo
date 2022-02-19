using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MassTransit;
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
using Newtonsoft.Json;
using Promo.Clients.External;
using Promo.Core;
using Promo.Core.Authentication;
using Promo.Core.Authentication.ConfigurationOptions;
using Promo.Core.MessageContracts;
using Promo.Core.Models;
using Promo.Membership.Core.RequestHandlers;
using Promo.Membership.Infrastructure;
using Serilog;
using System;
using System.Reflection;

namespace Promo.Membership.API
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

			services.AddMassTransit(config =>
			{
				config.AddBus(provider => Bus.Factory.CreateUsingRabbitMq());

				config.AddRequestClient<IRulesAdded>();
				config.AddRequestClient<IAccountCreated>();
			});
			services.AddMassTransitHostedService();

			services.AddSingleton<IDatabase>(c =>
			{
				return new MySqlDatabase("Identity", Configuration);
			});

			services.AddTransient<IAccountsRepository, AccountsRepository>();
			services.AddTransient<ICampaignRepository, CampaignRepository>();
			services.AddTransient<ICampaignTypesRepository, CampaignTypesRepository>();

			services.AddHttpClient<IIdentityClient, IdentityClient>();

			//Service Authentication
			AddHrvsterServiceAuthenticationclient(services);

			var domainAssembly = typeof(GetAccountRequest).GetTypeInfo().Assembly;
			services.AddMediatR(domainAssembly);
			//services.AddFluentValidation(new[] { domainAssembly });

			AddGoogleServicesAuthentication(services);
			AddFirebaseServices(services, Configuration);
		}

		private void AddHrvsterServiceAuthenticationclient(IServiceCollection services)
		{
			services.Configure<HrvsterServiceAccount>(Configuration);
			services.Configure<HrvsterServiceAccount>(Configuration
				.GetSection(HrvsterServiceAccount.SectionName));
			services.AddTransient(p => p.GetService<IOptions<HrvsterServiceAccount>>().Value);

			services.AddHttpClient<IServiceAuthenticate, ServiceAuthenticate>(client =>
			{
				client.BaseAddress = new Uri("https://identitytoolkit.googleapis.com");
				client.DefaultRequestHeaders.Add("Accept", "application/json");
			});
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
