using Firebase.Auth;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using hrvster.Core.Authentication;
using hrvster.Infrastructure.Authentication.ConfigurationOptions;
using hrvster.Models.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Text.Json;
using System.Threading.Tasks;
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

			services
				.Configure<FacebookConfigOptions>(configuration)
				.Configure<FacebookConfigOptions>(configuration.GetSection(FacebookConfigOptions.SectionName))
				.AddTransient(s => s.GetService<IOptions<FacebookConfigOptions>>().Value);

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
			if(FirebaseApp.DefaultInstance == null)
			{
				return FirebaseApp.Create(new AppOptions()
				{
					Credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(sp.GetService<FirebaseConfig>()))
				});
			}

			return FirebaseApp.DefaultInstance;
		}

		public static IServiceCollection AddFirebaseAuthManager(this IServiceCollection services)
			=> services.AddTransient<IFirebaseAuthManager, FirebaseAuthManager>();

		public static IServiceCollection AddSessionManager(this IServiceCollection services)
			=> services.AddTransient<ISessionManager, SessionManager>();

		public static IServiceCollection AddHrvsterAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			var googleServicesOptions = configuration.GetSection(GoogleServicesOptions.GoogleServices).Get<GoogleServicesOptions>();

			var facebookOptions = configuration.GetSection(FacebookConfigOptions.SectionName).Get<FacebookConfigOptions>();

			services.Configure<GoogleServicesOptions>(configuration);
			services.Configure<GoogleServicesOptions>(configuration.GetSection(GoogleServicesOptions.GoogleServices));
			services.AddTransient(p => p.GetService<IOptions<GoogleServicesOptions>>().Value);
			services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Set the default sign-in scheme to 'Cookies'
				})
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
				})
				.AddFacebook(options =>
				{
					options.AppId = facebookOptions.AppId;
					options.AppSecret = facebookOptions.AppSecret;
					options.Scope.Add("email");
					options.Scope.Add("public_profile");
					options.Scope.Add("pages_manage_posts");
					options.Scope.Add("user_location");
					options.Scope.Add("user_link");

					options.Events = new OAuthEvents
					{
						OnTicketReceived = context =>
						{
							context.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30);

							return Task.CompletedTask;
						}
					};
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
