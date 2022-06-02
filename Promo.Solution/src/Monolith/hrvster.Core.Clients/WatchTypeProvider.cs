using hrvster.Models;
using System.Threading.Tasks;

namespace hrvster.Core.Clients
{
	public interface IWatchTypeProvider
	{
		Task<string> GetAuthorizationUrl(string providerCode, string requestScheme, string hostValue, string urlPath);
		Task<UserProviderDetails> IsValidAuthorization(string providerCode, string queryString);
	}

	public class WatchTypeProvider : IWatchTypeProvider
	{
		private readonly ITwitterWatchTypeProvider _twitterWatchTypeProvider;

		public WatchTypeProvider(ITwitterWatchTypeProvider twitterWatchTypeProvider)
		{
			_twitterWatchTypeProvider = twitterWatchTypeProvider;
		}

		public async Task<string> GetAuthorizationUrl(string providerCode, string requestScheme, string hostValue, string urlPath)
		{
			switch (providerCode)
			{
				case Constants.TwitterProvider:
					return await _twitterWatchTypeProvider.GetAuthorizationUrl(requestScheme, hostValue, urlPath);
				case Constants.FaceBookProvider:
					break;
				case Constants.InstagramProvider:
					break;
			}

			return await _twitterWatchTypeProvider.GetAuthorizationUrl(requestScheme, hostValue, urlPath);
		}

		public async Task<UserProviderDetails> IsValidAuthorization(string providerCode, string queryString)
		{
			switch(providerCode)
			{
				case Constants.TwitterProvider:
					return await _twitterWatchTypeProvider.GetUserDetails(queryString);
			}

			return await _twitterWatchTypeProvider.GetUserDetails(queryString);
		}
	}
}
