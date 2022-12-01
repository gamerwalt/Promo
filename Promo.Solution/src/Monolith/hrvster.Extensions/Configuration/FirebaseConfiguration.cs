using Firebase.Auth;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using hrvster.Infrastructure.Authentication.ConfigurationOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using FirebaseAuth = FirebaseAdmin.Auth.FirebaseAuth;
using FirebaseConfig = hrvster.Infrastructure.Authentication.ConfigurationOptions.FirebaseConfig;

namespace hrvster.Extensions.Configuration
{
	public static class FirebaseConfiguration
	{
		public static IServiceCollection AddFirebaseServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<FirebaseConfig>(configuration);
			services.Configure<FirebaseConfig>(configuration.GetSection(FirebaseConfig.Firebase));
			services.AddTransient(p => p.GetService<IOptions<FirebaseConfig>>().Value);

			services.AddSingleton(c => BuildFirebaseApp(c, configuration));
			services.AddSingleton(c => BuildFirebaseAuth(c, configuration));
			services.AddSingleton(c => BuildFirebaseAuthProvider(c, configuration));

			return services;
		}

		private static IFirebaseAuthProvider BuildFirebaseAuthProvider(IServiceProvider sp, IConfiguration configuration)
		{
			var googleServicesOption = sp.GetService<GoogleServicesOptions>();
			return new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(googleServicesOption.WebApiKey));
		}

		private static FirebaseAuth BuildFirebaseAuth(IServiceProvider c, IConfiguration configuration)
		{
			return FirebaseAuth.GetAuth(c.GetService<FirebaseApp>());
		}

		private static FirebaseApp BuildFirebaseApp(IServiceProvider sp, IConfiguration configuration)
		{
			return FirebaseApp.Create(new AppOptions()
			{
				Credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(sp.GetService<FirebaseConfig>()))
			});
		}

		public static IServiceCollection AddHrvsterAuthentication(this IServiceCollection services, IConfiguration configuration)
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
					o.AccessDeniedPath = "/Session/Denied";
					o.ForwardForbid = o.AccessDeniedPath;
					o.LoginPath = "/Session/SignIn";
					o.ReturnUrlParameter = "returnUrl";
				});

			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
				options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
			});

			return services;
		}
	}
}
