
using hrvster.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace hrvster.Extensions.Configuration
{
	public static class TwitterConfigurationExtensions
	{
		//private static IServiceCollection AddTwitterClient(this IServiceCollection services, IConfiguration configuration)
		//{
		//	var sp = services.BuildServiceProvider();
		//	services.Configure<TwitterAccount>(configuration);
		//	services.Configure<TwitterAccount>(configuration.GetSection(TwitterAccount.SectionName));
		//	services.AddTransient(p => p.GetService<IOptions<TwitterAccount>>().Value);

		//	services.Configure<hrvster.Models.Options.FileServerOptions>(Configuration);
		//	services.Configure<hrvster.Models.Options.FileServerOptions>(Configuration.GetSection(hrvster.Models.Options.FileServerOptions.SectionName));
		//	services.AddTransient(p => p.GetService<IOptions<hrvster.Models.Options.FileServerOptions>>().Value);

		//	services.AddTransient<ITwitterClient, Tweetinvi.TwitterClient>(c => GenerateTwitterClient(c));

		//	services.AddSingleton<IAuthenticationRequestStore, LocalAuthenticationRequestStore>();

		//	return services;
		//}

		//private Tweetinvi.TwitterClient GenerateTwitterClient(IServiceProvider c)
		//{
		//	var twitterAccount = c.GetService<TwitterAccount>();
		//	var userCredentials = new TwitterCredentials(
		//		twitterAccount.ConsumerKey,
		//		twitterAccount.ConsumerSecret,
		//		twitterAccount.AccessToken,
		//		twitterAccount.AccessTokenSecret);

		//	var client = new Tweetinvi.TwitterClient(userCredentials);
		//	client.Config.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

		//	return client;
		//}
	}
}
