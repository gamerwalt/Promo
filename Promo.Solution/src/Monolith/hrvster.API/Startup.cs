using hrvster.Core.Requests;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.Notifications.Handlers;
using hrvster.Infrastructure;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Promo.Clients.External;
using Promo.Configuration.Extensions;
using Serilog;
using System.Reflection;

namespace hrvster.API
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
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "hrvster.API", Version = "v1" });
			});

			AddDatabaseAndRepositories(services);

			services.AddHttpClient<IIdentityClient, IdentityClient>();

			services.AddGoogleServices(Configuration);
			services.AddFirebaseServices(Configuration);

			var handlerAssembly = typeof(GetAccountRequest).GetTypeInfo().Assembly;
			var notificationAssembly = typeof(AccountCreatedViaFirebase).GetTypeInfo().Assembly;
			var notificationHandlerAssembly = typeof(StoreAccountInDatabase).GetTypeInfo().Assembly;
			services.AddMediatR(new[] { handlerAssembly, notificationAssembly, notificationHandlerAssembly });
			services.AddFluentValidation(new[] { handlerAssembly });
		}

		private void AddDatabaseAndRepositories(IServiceCollection services)
		{
			services.AddSingleton<IDatabase>(c =>
			{
				return new MySqlDatabase("hrvster", Configuration);
			});

			services.AddTransient<IAccountsRepository, AccountsRepository>();
			services.AddTransient<ICampaignRepository, CampaignRepository>();
			services.AddTransient<ICampaignTypesRepository, CampaignTypesRepository>();
			services.AddTransient<IRulesRepository, RulesRepository>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "hrvster.API v1"));
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
