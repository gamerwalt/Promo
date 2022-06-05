using hrvster.Infrastructure.Authentication.ConfigurationOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;

namespace hrvster.Configuration
{
	public static class AuthenticationConfigurationExtensions
	{
		private static IServiceCollection AddHrvsterAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			var googleServicesOptions = configuration.GetSection(GoogleServicesOptions.GoogleServices).Get<GoogleServicesOptions>();

			services.Configure<GoogleServicesOptions>(configuration);
			services.Configure<GoogleServicesOptions>(configuration.GetSection(GoogleServicesOptions.GoogleServices));
			services.AddTransient(p => p.GetService<IOptions<GoogleServicesOptions>>().Value);
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
				})
				.AddCookie(o =>
				{
					o.ExpireTimeSpan = TimeSpan.FromMinutes(30);
					o.SlidingExpiration = true;
					o.AccessDeniedPath = "/Denied";
					o.LoginPath = "/Session/SignIn";
				});

			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

			return services;
		}
	}
}
