using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Promo.Clients.External;
using Promo.Configuration.Extensions;
using Promo.Core;
using Promo.Core.Authentication;
using Promo.Core.Exceptions;
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
				config.AddRequestClient<ICampaignCreated>();
				config.AddRequestClient<ICampaignUpdated>();
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

			services.AddGoogleServices(Configuration);
			services.AddFirebaseServices(Configuration);
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

			app.ConfigureExceptionHandler(app.ApplicationServices.GetRequiredService<ILogger<Startup>>());

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
