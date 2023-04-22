using hrvster.Models;
using System.Threading.Tasks;

namespace hrvster.Core.Clients
{
	public interface IWatchTypeProvider
	{
		Task<string> GetAuthorizationUrl(string providerCode, string requestScheme, string hostValue, string urlPath);
		Task<string> GetCallbackUrl(string scheme, string value, string urlPath);
		Task<UserProviderDetails> IsValidAuthorization(string providerCode, string queryString);
	}

	public class WatchTypeProvider : IWatchTypeProvider
	{
		private readonly ITwitterWatchTypeProvider _twitterWatchTypeProvider;
		private readonly IFacebookWatchTypeProvider _facebookWatchTypeProvider;

		public WatchTypeProvider(
			ITwitterWatchTypeProvider twitterWatchTypeProvider, 
			IFacebookWatchTypeProvider facebookWatchTypeProvider)
		{
			_twitterWatchTypeProvider = twitterWatchTypeProvider;
			_facebookWatchTypeProvider = facebookWatchTypeProvider;
		}

		public async Task<string> GetAuthorizationUrl(string providerCode, string requestScheme, string hostValue, string urlPath)
		{
			switch (providerCode)
			{
				case Constants.TwitterProvider:
					return await _twitterWatchTypeProvider.GetAuthorizationUrl(requestScheme, hostValue, urlPath);
				case Constants.FaceBookProvider:
					return await _facebookWatchTypeProvider.GetAuthorizationUrl(requestScheme, hostValue, urlPath);
				case Constants.InstagramProvider:
					break;
				default:
					return await _facebookWatchTypeProvider.GetAuthorizationUrl(requestScheme, hostValue, urlPath);
			}

			return await _facebookWatchTypeProvider.GetAuthorizationUrl(requestScheme, hostValue, urlPath);
		}

		public async Task<string> GetCallbackUrl(string scheme, string value, string urlPath)
			=> $"{scheme}://{value}{urlPath}";

		public async Task<UserProviderDetails> IsValidAuthorization(string providerCode, string queryString)
		{
			switch(providerCode)
			{
				case Constants.TwitterProvider:
					return await _twitterWatchTypeProvider.GetUserDetails(queryString);
				case Constants.FaceBookProvider:
					return await _facebookWatchTypeProvider.GetUserDetails(queryString);
			}

			return await _facebookWatchTypeProvider.GetUserDetails(queryString);
		}
	}
}
