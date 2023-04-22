using hrvster.Models.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace hrvster.Core.Clients
{
	public interface IFacebookWatchTypeProvider
	{
		Task<string> GetAuthorizationUrl(string requestScheme, string hostValue, string urlPath);
		Task<UserProviderDetails> GetUserDetails(string queryString);
	}

	public class FacebookWatchTypeProvider : IFacebookWatchTypeProvider
	{
		private readonly ILogger<FacebookWatchTypeProvider> _logger;
		private readonly FacebookConfigOptions _facebookOptions;

		public FacebookWatchTypeProvider(
			ILoggerFactory loggerFactory,
			FacebookConfigOptions facebookOptions)
		{
			_logger = loggerFactory.CreateLogger<FacebookWatchTypeProvider>();
			_facebookOptions = facebookOptions;
		}

		public Task<string> GetAuthorizationUrl(string requestScheme, string hostValue, string urlPath)
		{
			var callbackUrl = $"{requestScheme}://{hostValue}{urlPath}";

			return Task.FromResult($"https://www.facebook.com/v12.0/dialog/oauth?client_id={_facebookOptions.AppId}&redirect_uri={callbackUrl}&scope=email,user_posts,user_friends,user_posts,user_videos,pages_show_list,pages_manage_posts");
		}

		public Task<UserProviderDetails> GetUserDetails(string queryString)
		{
			throw new NotImplementedException();
		}
	}
}
