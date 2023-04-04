using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.MatchGames
{
	public interface IMatchGameplayService
	{
		Task<bool> GameBeenPlayedAsync(Guid gameplayUid, CancellationToken cancellationToken);
		Task<MatchPlay> GetAsync(Guid gameplayUid, CancellationToken cancellationToken);
		Task<bool> IsAWinAsync(Guid gameUid, Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken);
		Task MarkAsPlayedAndLostAsync(Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken);
		Task MarkAsPlayedAndWinAsync(Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken);
	}

	public class MatchGameplayService : IMatchGameplayService
	{
		private readonly ILogger<MatchGameplayService> _logger;
		private readonly IMatchGamePlayRepository _repository;
		private readonly IMatchTilesRepository _tilesRepository;
		private readonly IMatchBoardsRepository _boardsRepository;

		public MatchGameplayService(
			ILoggerFactory loggerFactory,
			IMatchGamePlayRepository repository,
			IMatchTilesRepository tilesRepository,
			IMatchBoardsRepository boardsRepository)
		{
			_logger = loggerFactory.CreateLogger<MatchGameplayService>();
			_repository = repository;
			_tilesRepository = tilesRepository;
			_boardsRepository = boardsRepository;
		}

		public async Task<bool> GameBeenPlayedAsync(Guid gameplayUid, CancellationToken cancellationToken)
		{
			var gameplay = await GetAsync(gameplayUid, cancellationToken);

			return gameplay.PlayedIndicator;
		}

		public async Task<MatchPlay> GetAsync(Guid gameplayUid, CancellationToken cancellationToken)
			=> await _repository.GetAsync(gameplayUid, cancellationToken);

		public async Task<bool> IsAWinAsync(
			Guid gameUid, Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken)
		{
			
			var gameplay = await GetAsync(gameplayUid, cancellationToken);

			if (gameplay == null) return false;

			if (!gameplay.MatchBoard.IsWinnable) return false;

			var listOfTiles = new List<MatchTile>();

			var selectedTilesInGuid = selectedTiles.Split(',').ToList();

			foreach(var tileUid in selectedTilesInGuid)
			{
				listOfTiles.Add(await _tilesRepository.GetTileAsync(tileUid, cancellationToken));
			}

			if(!HaveTheSamePrize(listOfTiles))
			{
				return false;
			}

			return true;
		}

		public async Task MarkAsPlayedAndLostAsync(Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken)
		{
			await _repository.MarkAsPlayedAndLossAsync(gameplayUid, selectedTiles, cancellationToken);

			var gameplay = await GetAsync(gameplayUid, cancellationToken);

			await _boardsRepository.UpdateAsPlayedAsync(gameplay.MatchBoard.Uid, cancellationToken);
		}

		public async Task MarkAsPlayedAndWinAsync(Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken)
		{
			await _repository.MarkAsPlayedAndWonAsync(gameplayUid, selectedTiles, cancellationToken);

			var gameplay = await GetAsync(gameplayUid, cancellationToken);

			await _boardsRepository.UpdateAsPlayedAsync(gameplay.MatchBoard.Uid, cancellationToken);
		}

		private bool HaveTheSamePrize(List<MatchTile> listOfTiles)
		{
			var same = true;

			var firstTile = listOfTiles.FirstOrDefault();
			var firstPrize = firstTile.Prize;

			foreach(var tile in listOfTiles)
			{
				if (tile.Prize.Id != firstPrize.Id)
				{
					same = false;
					break;
				}
			}

			return same;
		}
	}
}
