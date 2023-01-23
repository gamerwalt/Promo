using hrvster.Core.Authentication;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using hrvster.Models.Options;
using System;
using System.Threading.Tasks;

namespace hrvster.Core
{
	public class LinkBuilder
	{
		private HrvsterUser _hrvsterUser;

		private readonly IAccountsRepository _accountsRepository;
		private readonly IProfilesRepository _profilesRepository;
		private readonly Services _services;
		private readonly ISessionManager _sessionManager;

		public LinkBuilder(
			IAccountsRepository accountsRepository,
			IProfilesRepository profilesRepository,
			Services services,
			ISessionManager sessionManager)
		{
			_accountsRepository = accountsRepository;
			_profilesRepository = profilesRepository;
			_services = services;
			_sessionManager = sessionManager;
		}

		public async Task<Uri> BuildTwitterLink(string tweetId, Guid ruleUid)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			if (string.IsNullOrWhiteSpace(_hrvsterUser.TwitterUserName))
			{
				var profile = await _profilesRepository.GetProfileAsync(_hrvsterUser.UserUid, Models.ProfileType.Twitter);
				_hrvsterUser.TwitterUserName = profile?.ScreenName;
			}
			var twitterUserName = string.Empty;

			if(string.IsNullOrWhiteSpace(_hrvsterUser.TwitterUserName))
			{
				var profile = await _profilesRepository.GetProfileBelongingToSweepRuleAsync(ruleUid, ProfileType.Twitter);
				_hrvsterUser.TwitterUserName = profile.ScreenName;
			}

			return new Uri(_services.TweetIdUrl
				.Replace("{username}", _hrvsterUser.TwitterUserName)
				.Replace("{tweetId}", tweetId));
		}

		public async Task<Uri> BuildTwitterLink(string tweetId, Guid ruleUid, string twitterUser)
		{
			return new Uri(_services.TweetIdUrl
				.Replace("{username}", twitterUser)
				.Replace("{tweetId}", tweetId));
		}

		public Uri BuildTwitterHandleLink(string userHandle)
		{
			var uri = _services.TweetHandleUrl.Replace("{username}", userHandle.Replace("@", ""));
			return new Uri(uri);
		}
	}
}
