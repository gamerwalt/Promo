using hrvster.Core.Authentication;
using hrvster.Core.Settings;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using System;
using System.Threading.Tasks;

namespace hrvster.Core
{
	public class LinkBuilder
	{
		private readonly HrvsterUser _hrvsterUser;
		private readonly IAccountsRepository _accountsRepository;
		private readonly IProfilesRepository _profilesRepository;
		private readonly Services _services;

		public LinkBuilder(HrvsterUser hrvsterUser,
			IAccountsRepository accountsRepository,
			IProfilesRepository profilesRepository,
			Services services)
		{
			_hrvsterUser = hrvsterUser;
			_accountsRepository = accountsRepository;
			_profilesRepository = profilesRepository;
			_services = services;
		}

		public async Task<Uri> BuildTwitterLink(string tweetId)
		{
			if (string.IsNullOrWhiteSpace(_hrvsterUser.TwitterUserName))
			{
				var profile = await _profilesRepository.GetProfileAsync(_hrvsterUser.UserUid, Models.ProfileType.Twitter);
				_hrvsterUser.TwitterUserName = profile?.ScreenName;
			}

			return new Uri(_services.TweetIdUrl
				.Replace("{username}", _hrvsterUser.TwitterUserName)
				.Replace("{tweetId}", tweetId));
		}
	}
}
