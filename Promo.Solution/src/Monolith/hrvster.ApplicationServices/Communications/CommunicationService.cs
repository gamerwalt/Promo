using hrvster.Infrastructure;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Communications
{
	public interface ICommunicationService
	{
		Task NotifyAccountOfWin(string emailAddress, Guid gameUid, string gameName, string screenName, string prizeName);
		Task NotifyMemberOfWin(string emailAddress, string gameName, Guid gameplayUid, string prizeName, decimal prizeValue);
	}

	public class CommunicationService : ICommunicationService
	{
		private readonly ILogger<CommunicationService> _logger;
		private readonly IMailRepository _mailRepository;

		public CommunicationService(ILoggerFactory loggerFactory, IMailRepository mailRepository)
		{
			_logger = loggerFactory.CreateLogger<CommunicationService>();
			_mailRepository = mailRepository;
		}

		public async Task NotifyAccountOfWin(
			string emailAddress, 
			Guid gameUid, 
			string gameName, 
			string screenName, 
			string prizeName)
		{
			await _mailRepository.CreateAsync(new Email
			{
				EmailAddress = emailAddress,
				EmailUid = Guid.NewGuid(),
				Parameters = new Dictionary<string, string>()
				{
					{ EmailParameters.GameGuid, gameUid.ToString() },
					{ EmailParameters.GameName, gameName },
					{ EmailParameters.ScreenName, screenName },
					{ EmailParameters.PrizeName, prizeName }
				},
				Status = EmailStatus.New,
				Template = EmailTemplate.NotifyAccountOfUserWinningPick3WinEmail
			});
		}

		public async Task NotifyMemberOfWin(
			string emailAddress, 
			string gameName, 
			Guid gameplayUid, 
			string prizeName, 
			decimal prizeValue)
		{
			await _mailRepository.CreateAsync(new Email
			{
				EmailAddress = emailAddress,
				EmailUid = Guid.NewGuid(),
				Parameters = new Dictionary<string, string>()
				{
					{ EmailParameters.GameName, gameName },
					{ EmailParameters.GameplayUid, gameplayUid.ToString() },
					{ EmailParameters.PrizeName, prizeName },
					{ EmailParameters.PrizeValue, prizeValue.ToString() }
				},
				Status = EmailStatus.New,
				Template = EmailTemplate.NotifyMemberOfPick3WinEmail
			});
		}
	}
}
