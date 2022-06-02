using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Auth;
using Tweetinvi.Parameters;

namespace hrvster.Core.Clients
{
	public interface ITwitterWatchTypeProvider
	{
		Task<string> GetAuthorizationUrl(string requestScheme, string hostValue, string urlPath);
		Task<UserProviderDetails> GetUserDetails(string queryString);
	}

	public class TwitterWatchTypeProvider : ITwitterWatchTypeProvider
	{
		private readonly IAuthenticationRequestStore _authenticationRequestStore;
		private readonly ITwitterClient _twitterClient;
		public TwitterWatchTypeProvider(
			IAuthenticationRequestStore authenticationRequestStore,
			ITwitterClient twitterClient)
		{
			_authenticationRequestStore = authenticationRequestStore;
			_twitterClient = twitterClient;
		}

		public async Task<string> GetAuthorizationUrl(string requestScheme, string hostValue, string urlPath)
		{
			var requestId = Guid.NewGuid().ToString();
			var redirectPath = $"{requestScheme}://{hostValue}{urlPath}";
			
			var requestToken = await _twitterClient.Auth.RequestAuthenticationUrlAsync(_authenticationRequestStore.AppendAuthenticationRequestIdToCallbackUrl(redirectPath, requestId));

			await _authenticationRequestStore.AddAuthenticationTokenAsync(requestId, requestToken);

			return requestToken.AuthorizationURL;
		}

		public async Task<UserProviderDetails> GetUserDetails(string queryString)
		{
			var requestParameters = await RequestCredentialsParameters.FromCallbackUrlAsync(queryString, _authenticationRequestStore);

			var userCredentials = await _twitterClient.Auth.RequestCredentialsAsync(requestParameters);

			var userClient = new TwitterClient(userCredentials);
			var user = await userClient.Users.GetAuthenticatedUserAsync();

			return new UserProviderDetails
			{
				Name = user.Name,
				ScreenName = user.ScreenName,
				Description = user.Description,
				AccessToken = user.Credentials.AccessToken,
				AccessTokenSecret = user.Credentials.AccessTokenSecret,
				BearerToken = user.Credentials.BearerToken,
				Location = user.Location,
				ProfileImage = user.ProfileImageUrl400x400
			};
		}
	}
}
