using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Facebook;
using hrvster.Infrastructure.MatchGames;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.MatchGames
{
	public interface IMatchGamesService
	{
		Task CreateMatchGameAsync(MatchGame matchGame, string timeZone, bool isDaylight, CancellationToken cancellationToken);
		Task DeleteGameRuleAsync(Guid ruleUid, Guid gameUid, Guid userUid, ExternalClientType externalClientType, string externalId, CancellationToken cancellationToken);
		Task<bool> GamePlayableAsync(Guid uid, CancellationToken cancellationToken);
		Task<IEnumerable<MatchGame>> GetAccountMatchGamesAsync(Guid userUid, CancellationToken cancellationToken);
		Task<IEnumerable<MatchGame>> GetCampaignMatchGamesAsync(Guid campaignUid, CancellationToken cancellationToken);
		Task<IEnumerable<MatchGame>> GetGamesReadyForTileCreationAsync(CancellationToken token);
		MatchGame GetMatchGame(Guid matchGameUid);
		Task<MatchGame> GetMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken);
		Task<IEnumerable<MatchGame>> GetMatchGamesToGenerateBoardAsync(CancellationToken token);
		Task<MatchPrize> GetPrizeFromGameplayUidAsync(Guid gameplayUid, CancellationToken cancellationToken);
		Task<IEnumerable<Rule>> GetRulesAsync(CancellationToken cancellationToken);
		Task<IEnumerable<Rule>> GetRulesAsync(Guid gameUid, CancellationToken cancellationToken);
		IEnumerable<Rule> GetRulesMatching(string watchValue);
		Task<IEnumerable<Rule>> GetRulesMatchingAsync(string watchValue, CancellationToken cancellationToken);
		Task SetAsReadyForTilesToBeProcessedAsync(MatchGame matchGame, CancellationToken token);
		Task SetAsReadyToBeProcessedAsync(MatchGame matchGame, CancellationToken cancellationToken);
		Task SetMatchGameAsProcessedAsync(MatchGame matchGame, CancellationToken cancellationToken);
		Task UpdateMatchGameAsync(MatchGame matchGame, string timeZone, bool isDaylight, CancellationToken cancellationToken);
	}

	public class MatchGamesService : IMatchGamesService
	{
		private readonly ILogger<MatchGamesService> _logger;
		private readonly IMatchGamesRepository _repository;
		private readonly IMatchGameRulesRepository _matchGameRulesRepository;
		private readonly IMatchRulesService _matchRulesService;
		private readonly IMatchRulePageRepository _matchRulePageRepository;
		private readonly IProfilesRepository _profilesRepository;
		private readonly IFacebookPageRepository _facebookPageRepository;
		private readonly Func<ExternalClientType, Core.Clients.Publishers.IExternalClient> _publisherResolver;
		private readonly IMatchPrizeService _matchPrizeService;
		private readonly IMatchGamePlayRepository _matchGamePlayRepository;

		public MatchGamesService(
			ILoggerFactory loggerFactory,
			IMatchGamesRepository repository,
			IMatchGameRulesRepository matchGameRulesRepository,
			IMatchRulesService matchRulesService,
			IMatchRulePageRepository matchRulePageRepository,
			IProfilesRepository profilesRepository,
			IFacebookPageRepository facebookPageRepository,
			Func<ExternalClientType, Core.Clients.Publishers.IExternalClient> publisherResolver,
			IMatchGamePlayRepository matchGamePlayRepository,
			IMatchPrizeService matchPrizeService)
		{
			_logger = loggerFactory.CreateLogger<MatchGamesService>();
			_repository = repository;
			_matchGameRulesRepository = matchGameRulesRepository;
			_matchRulesService = matchRulesService;
			_matchRulePageRepository = matchRulePageRepository;
			_profilesRepository = profilesRepository;
			_facebookPageRepository = facebookPageRepository;
			_publisherResolver = publisherResolver;
			_matchGamePlayRepository = matchGamePlayRepository;
			_matchPrizeService = matchPrizeService;
		}

		public async Task<MatchPrize> GetPrizeFromGameplayUidAsync(Guid gameplayUid, CancellationToken cancellationToken)
		{
			var gameplay = await _matchGamePlayRepository.GetAsync(gameplayUid, cancellationToken);

			return await _matchPrizeService.GetPrizeFromBoardAsync(gameplay.MatchBoard.Uid, cancellationToken);
		}

		public async Task CreateMatchGameAsync(
			MatchGame matchGame, string timeZone, bool isDaylight, CancellationToken cancellationToken)
		{
			if (matchGame == null) throw new ArgumentNullException(nameof(matchGame));
			if (matchGame.CampaignId == 0 || matchGame.CampaignUid == Guid.Empty)
				throw new ArgumentException("Match game campaign needs to be supplied.");

			await _repository.CreateAsync(matchGame, timeZone, isDaylight, cancellationToken);
		}

		public async Task<IEnumerable<Rule>> GetRulesAsync(Guid gameUid, CancellationToken cancellationToken)
			=> await _matchGameRulesRepository.GetAllRulesAsync(gameUid, cancellationToken);

		public async Task<IEnumerable<MatchGame>> GetGamesReadyForTileCreationAsync(CancellationToken token)
			=> await _repository.GetGamesReadyForTileCreationAsync(token);

		public async Task DeleteGameRuleAsync(
			Guid ruleUid, 
			Guid gameUid, 
			Guid userUid,
			ExternalClientType externalClientType, 
			string externalId, 
			CancellationToken cancellationToken)
		{
			var rule = await _matchGameRulesRepository.GetAsync(ruleUid);

			if(rule == null)
			{
				_logger.LogError("Unable to delete the game rule with uid {ruleUid} because we could not locate the rule.",
					ruleUid);

				return;
			}

			var profile = await _profilesRepository.GetProfileAsync(userUid, GetProfileType(externalClientType));
			if (profile == null)
			{
				_logger.LogError("Could not load the profile with AccountUid: {userUid}", userUid);
				return;
			}

			var accessToken = profile.AccessToken;

			if (rule.ExternalClientType == ExternalClientType.Facebook)
			{
				var rulePage = await _matchRulePageRepository.GetByRuleId(rule.RuleId);

				if (rulePage == null)
				{
					_logger.LogError("Unable to delete the game rule with uid {ruleUid} because we could not locate the rule page for rule with id of {ruleId}",
						ruleUid,
						rule.RuleId);

					return;
				}

				var page = await _facebookPageRepository.GetPageAsync(rulePage.PageId);
				if (page == null)
				{
					_logger.LogError("Unable to load the page to delete properly.");
				}
				else
				{
					accessToken = page.PageAccessToken;
				}
			}

			var publisher = _publisherResolver(rule.ExternalClientType);

			await publisher.DeleteAsync(externalId.ToString(), accessToken, profile.AccessTokenSecret);

			await _matchGameRulesRepository.DeleteAsync(ruleUid, cancellationToken);
		}

		private ProfileType GetProfileType(ExternalClientType externalClientType)
		{
			if (externalClientType == ExternalClientType.Twitter)
				return ProfileType.Twitter;
			else if (externalClientType == ExternalClientType.Facebook)
				return ProfileType.Facebook;

			return ProfileType.Facebook;
		}

		public async Task<bool> GamePlayableAsync(Guid uid, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Verifying if the game {uid} is playable", uid);

			var game = await _repository.GetAsync(uid, cancellationToken);
			if (game == null)
			{
				_logger.LogWarning("Game does not exist.");
				return false;
			}

			if (DateTime.UtcNow > game.EndDateUtc || DateTime.UtcNow < game.StartDateUtc)
			{
				_logger.LogWarning("Current date not within game start and end dates.");
				return false;
			}

			if(!game.HasBeenGenerated)
			{
				_logger.LogWarning("Gameboards for the game has not been generated.");
				return false;
			}

			_logger.LogInformation("Game is playable.");

			return true;
		}

		public async Task<IEnumerable<MatchGame>> GetAccountMatchGamesAsync(Guid userUid, CancellationToken cancellationToken)
		{
			if (userUid == Guid.Empty)
				throw new ArgumentNullException(nameof(userUid));

			return await _repository.GetByAccountAsync(userUid, cancellationToken);
		}

		public async Task<IEnumerable<MatchGame>> GetCampaignMatchGamesAsync(Guid campaignUid, CancellationToken cancellationToken)
		{
			if (campaignUid == Guid.Empty)
				throw new ArgumentNullException(nameof(campaignUid));

			return await _repository.GetByCampaignAsync(campaignUid, cancellationToken);
		}

		public MatchGame GetMatchGame(Guid matchGameUid)
		{
			if (matchGameUid == Guid.Empty)
				throw new ArgumentNullException(nameof(matchGameUid));

			return _repository.Get(matchGameUid, default(CancellationToken));
		}

		public async Task<MatchGame> GetMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken)
		{
			if (matchGameUid == Guid.Empty)
				throw new ArgumentNullException(nameof(matchGameUid));

			return await _repository.GetAsync(matchGameUid, cancellationToken);
		}

		public async Task<IEnumerable<MatchGame>> GetMatchGamesToGenerateBoardAsync(CancellationToken token)
			=> await _repository.GetGameReadyForGenerationAsync(token);

		public async Task<IEnumerable<Rule>> GetRulesAsync(CancellationToken cancellationToken)
			=> await _repository.GetRulesAsync(cancellationToken);

		public IEnumerable<Rule> GetRulesMatching(string watchValue)
			=> _matchRulesService.GetRulesMatching(watchValue);

		public async Task<IEnumerable<Rule>> GetRulesMatchingAsync(string watchValue, CancellationToken cancellationToken)
		{
			return await _matchRulesService.GetRulesMatchingAsync(watchValue, cancellationToken);
		}

		public async Task SetAsReadyToBeProcessedAsync(MatchGame matchGame, CancellationToken cancellationToken)
		{
			if (matchGame == null) throw new ArgumentNullException(nameof(matchGame));

			await _repository.SetAsReadyToBeGeneratedAsync(matchGame, cancellationToken);
		}

		public Task SetMatchGameAsProcessedAsync(MatchGame matchGame, CancellationToken cancellationToken)
			=> _repository.SetAsGeneratedAsync(matchGame, cancellationToken);

		public async Task UpdateMatchGameAsync(
			MatchGame matchGame, string timeZone, bool isDaylight, CancellationToken cancellationToken)
		{
			if (matchGame == null) throw new ArgumentNullException(nameof(matchGame));
			if (matchGame.CampaignId == 0 || matchGame.CampaignUid == Guid.Empty)
				throw new ArgumentException("Match game campaign needs to be supplied.");

			await _repository.UpdateAsync(matchGame, timeZone, isDaylight, cancellationToken);
		}

		public Task SetAsReadyForTilesToBeProcessedAsync(MatchGame matchGame, CancellationToken token)
		{
			return _repository.SetAsReadyForTilesToBeProcessedAsync(matchGame.Uid, token);
		}
	}
}
