
using hrvster.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Tweetinvi;
using Tweetinvi.Auth;
using Tweetinvi.Models;

namespace hrvster.Extensions.Configuration
{
	public static class TwitterConfigurationExtensions
	{
		public static IServiceCollection AddTwitterClient(this IServiceCollection services, IConfiguration configuration)
			=> services.Configure<TwitterAccount>(configuration)
				.Configure<TwitterAccount>(configuration.GetSection(TwitterAccount.SectionName))
				.AddTransient(p => p.GetService<IOptions<TwitterAccount>>().Value)
				.AddTransient<ITwitterClient, TwitterClient>(c => GenerateTwitterClient(c))
				.AddSingleton<IAuthenticationRequestStore, LocalAuthenticationRequestStore>();
		

		private static TwitterClient GenerateTwitterClient(IServiceProvider c)
		{
			var twitterAccount = c.GetService<TwitterAccount>();
			var userCredentials = new TwitterCredentials(
				twitterAccount.ConsumerKey,
				twitterAccount.ConsumerSecret,
				twitterAccount.AccessToken,
				twitterAccount.AccessTokenSecret);

			var client = new TwitterClient(userCredentials);
			client.Config.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

			return client;
		}
	}
}
