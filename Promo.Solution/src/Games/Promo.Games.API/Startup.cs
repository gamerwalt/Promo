using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Promo.Configuration.Extensions;
using Promo.Core;
using Promo.Core.Exceptions;
using Promo.Core.MessageContracts;
using Promo.Games.Core.RequestHandlers;
using Promo.Games.Infrastructure;
using Serilog;
using System.Reflection;

namespace Promo.Games.API
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
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Promo.Games.API", Version = "v1" });
			});

			services.AddMassTransit(config =>
			{
				config.AddBus(provider => Bus.Factory.CreateUsingRabbitMq());

				config.AddRequestClient<ISweepCreated>();
			});
			services.AddMassTransitHostedService();

			services.AddGoogleServices(Configuration);

			var domainAssembly = typeof(CreateSweep).GetTypeInfo().Assembly;
			services.AddMediator(domainAssembly);

			AddDatabaseAndRepositories(services);
		}

		private void AddDatabaseAndRepositories(IServiceCollection services)
		{
			services.AddSingleton<IDatabase>(c =>
			{
				return new MySqlDatabase("Games", Configuration);
			});

			services.AddTransient<ISweepsRepository, SweepsRepository>();
			services.AddTransient<ISweepsEntriesRepository, SweepsEntriesRepository>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Promo.Games.API v1"));
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
