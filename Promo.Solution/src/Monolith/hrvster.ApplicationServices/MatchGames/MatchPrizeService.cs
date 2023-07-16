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
	public interface IMatchPrizeService
	{
		Task CreateAsync(Guid uid, string name, string description, decimal value, int quantity, Guid imageUid, bool isFiller, CancellationToken cancellationToken);
		Task DeleteAsync(Guid prizeUid, CancellationToken cancellationToken);
		Task<IEnumerable<MatchPrize>> GetAllForMatchGameAsync(Guid uid, CancellationToken cancellationToken);
		MatchPrize GetPrize(Guid matchPrizeUid);
		Task<MatchPrize> GetPrizeAsync(Guid prizeUid, CancellationToken cancellationToken);
		Task<MatchPrize> GetPrizeFromBoardAsync(Guid gameboardUid, CancellationToken cancellationToken);
		int GetTotalPrizesForGame(Guid matchGameUid);
		Task UpdateAsync(Guid prizeUid, Guid uid, string name, string description, int quantity, decimal value, Guid imageUid, bool isFiller, CancellationToken cancellationToken);
	}

	public class MatchPrizeService : IMatchPrizeService
	{
		private readonly ILogger<MatchPrizeService> _logger;
		private readonly IMatchPrizesRepository _repository;

		public MatchPrizeService(ILoggerFactory loggerFactory, IMatchPrizesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<MatchPrizeService>();
			_repository = repository;
		}

		public async Task CreateAsync(
			Guid uid, 
			string name, 
			string description, 
			decimal value, 
			int quantity, 
			Guid imageUid, 
			bool isFiller,
			CancellationToken cancellationToken)
				=> await _repository.CreateAsync(new MatchPrize
				{
					Uid = Guid.NewGuid(),
					MatchGame = new MatchGame { Uid = uid },
					Name = name,
					Description = description,
					Value = value,
					Quantity = quantity,
					IsFiller = isFiller,
					Image = new MatchImage { Uid = imageUid }
				}, cancellationToken);

		public async Task DeleteAsync(Guid prizeUid, CancellationToken cancellationToken)
			=> await _repository.DeleteAsync(prizeUid, cancellationToken);

		public async Task<IEnumerable<MatchPrize>> GetAllForMatchGameAsync(Guid uid, CancellationToken cancellationToken)
			=> await _repository.GetForMatchGameAsync(uid, cancellationToken);

		public MatchPrize GetPrize(Guid matchPrizeUid)
			=> _repository.Get(matchPrizeUid);

		public async Task<MatchPrize> GetPrizeAsync(Guid prizeUid, CancellationToken cancellationToken)
			=> await _repository.GetAsync(prizeUid, cancellationToken);

		public int GetTotalPrizesForGame(Guid matchGameUid)
		{
			var prizes = _repository.GetForMatchGame(matchGameUid, default);
			var allPrizes = prizes.ToList();

			if (allPrizes.Count == 0) return 0;

			return allPrizes.Sum(p => p.Quantity);
		}

		public async Task<MatchPrize> GetPrizeFromBoardAsync(Guid gameboardUid, CancellationToken cancellationToken)
			=> await _repository.GetByGameboardUidAsync(gameboardUid, cancellationToken);

		/// <summary>
		/// Update a match prize
		/// </summary>
		/// <param name="prizeUid"></param>
		/// <param name="uid"></param>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <param name="quantity"></param>
		/// <param name="value"></param>
		/// <param name="imageUid"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task UpdateAsync(
			Guid prizeUid,
			Guid uid,
			string name,
			string description,
			int quantity,
			decimal value,
			Guid imageUid,
			bool isFiller,
			CancellationToken cancellationToken)
			=> await _repository.UpdateAsync(prizeUid, name, description, quantity, value, imageUid, isFiller, cancellationToken);
	}
}
