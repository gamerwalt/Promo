using hrvster.Core.Authentication;
using hrvster.Core.Settings;
using hrvster.Infrastructure;
using System;
using System.Threading.Tasks;

namespace hrvster.Core
{
	public class LinkBuilder
	{
		private readonly HrvsterUser _hrvsterUser;
		private readonly IAccountsRepository _accountsRepository;
		private readonly Services _services;

		public LinkBuilder(HrvsterUser hrvsterUser,
			IAccountsRepository accountsRepository,
			Services services)
		{
			_hrvsterUser = hrvsterUser;
			_accountsRepository = accountsRepository;
			_services = services;
		}

		public async Task<Uri> BuildTwitterLink(string tweetId)
		{
			if (string.IsNullOrWhiteSpace(_hrvsterUser.TwitterUserName))
			{
				var account = await _accountsRepository.GetAsync(_hrvsterUser.UserUid);
				_hrvsterUser.TwitterUserName = account.TwitterUserName;
			}

			return new Uri(_services.TweetIdUrl
				.Replace("{username}", _hrvsterUser.TwitterUserName)
				.Replace("{tweetId}", tweetId));
		}
	}
}
