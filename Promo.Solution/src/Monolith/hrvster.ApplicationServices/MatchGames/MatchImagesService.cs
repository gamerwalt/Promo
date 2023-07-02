using hrvster.Core.Requests.WebRequests.Files;
using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
using hrvster.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.MatchGames
{
	public interface IMatchImageService
	{
		Task<bool> CreateImagesAsync(MatchGame matchGame, ICollection<IFormFile> images, CancellationToken cancellationToken);
		Task DeleteImageAsync(MatchImage matchImage, MatchGame matchGame, CancellationToken cancellationToken);
		Task<IEnumerable<MatchImage>> GetForMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken);
		Task<MatchImage> GetImageAsync(Guid imageUid, CancellationToken cancellationToken);
		Task UpdateImageAsync(Guid matchImageUid, MatchGame matchGame, IFormFile image, CancellationToken cancellationToken);
		Task UpdateLossImageAsync(Guid uid, CancellationToken cancellationToken);
	}

	public class MatchImagesService : IMatchImageService
	{
		private readonly ILogger<MatchImagesService> _logger;
		private readonly IMatchImagesRepository _repository;
		private readonly IMatchPrizeService _matchPrizeService;
		private readonly IMediator _mediator;

		public MatchImagesService(
			ILoggerFactory loggerFactory,
			IMatchImagesRepository repository,
			IMatchPrizeService matchPrizeService,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<MatchImagesService>();
			_repository = repository;
			_matchPrizeService = matchPrizeService;
			_mediator = mediator;
		}

		public async Task<bool> CreateImagesAsync(
			MatchGame matchGame,
			ICollection<IFormFile> images,
			CancellationToken cancellationToken)
		{
			try
			{
				var uploadedImages = await _mediator.Send(new UploadMatchImagesRequest(matchGame, images));

				var tasks = new List<Task>();

				foreach (var uploadedImage in uploadedImages)
				{
					var matchImage = CreateMatchImage(matchGame, uploadedImage);

					tasks.Add(_repository.CreateAsync(matchImage, cancellationToken));

					_logger.LogInformation("Saving the image {location} into the database for {gameName}",
						uploadedImage.LocationUrl,
						matchGame.Name);
				}

				await Task.WhenAll(tasks);

				_logger.LogInformation("Successfully saved all the messages for {gameName}", matchGame.Name);
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occurred while trying to create images at {actionName} with error {errorMessage}",
					nameof(CreateImagesAsync),
					ex.Message);

				_logger.LogError(ex.StackTrace);

				return false;
			}

			return true;
		}

		public async Task UpdateImageAsync(Guid matchImageUid, MatchGame matchGame, IFormFile image, CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogInformation("Uploading the image {imageName} for match image uid {matchImageUid}",
					image.Name,
					matchImageUid);

				var uploadedImage = await _mediator.Send(new UploadMatchImagesRequest(matchGame, new[] { image }));

				_logger.LogInformation("Saved the image {imageName} at {location}",
					image.Name,
					uploadedImage.FirstOrDefault());

				var matchImage = CreateMatchImage(matchGame, uploadedImage.FirstOrDefault());

				await _repository.UpdateAsync(matchImageUid, matchImage, cancellationToken);

				_logger.LogInformation("Match image with uid {matchImageUid} updated successfully.", matchImageUid);
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occurred while trying to update the image at {actionName} with error {errorMessage}",
					nameof(UpdateImageAsync),
					ex.Message);

				_logger.LogError(ex.StackTrace);
			}
		}

		private MatchImage CreateMatchImage(MatchGame matchGame, UploadedImage uploadedImage)
			=> new MatchImage
			{
				Uid = Guid.NewGuid(),
				LocationUrl = uploadedImage.LocationUrl,
				Name = uploadedImage.Name,
				MatchGame = matchGame
			};

		public async Task<IEnumerable<MatchImage>> GetForMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken)
			=> await _repository.GetForMatchGamesAsync(matchGameUid, cancellationToken);

		public async Task<MatchImage> GetImageAsync(Guid imageUid, CancellationToken cancellationToken)
			=> await _repository.GetAsync(imageUid, cancellationToken);

		public async Task DeleteImageAsync(MatchImage matchImage, MatchGame matchGame, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Retrieving the image with uid {matchImageUid} with the location {locationUrl}",
				matchImage.Uid,
				matchImage.LocationUrl);

			await _mediator.Send(new DeleteMatchImageRequest(matchImage, matchGame));

			await _repository.DeleteAsync(matchImage.Uid, cancellationToken);
		}

		public async Task UpdateLossImageAsync(Guid gameUid, CancellationToken cancellationToken)
		{
			var matchImagesResult = await _repository.GetForMatchGamesAsync(gameUid, cancellationToken);
			var matchImagesList = matchImagesResult.ToList();

			_logger.LogInformation("Loaded a total of {imageCount} images for the game with uid {gameUid}", 
				matchImagesList.Count, 
				gameUid);

			var foundAndUpdated = false;

			if(!matchImagesList.Any(mi => mi.IsLoss))
			{
				_logger.LogInformation("We didn't find any images that is a loss image.");

				var matchPrizesResult = await _matchPrizeService.GetAllForMatchGameAsync(gameUid, cancellationToken);
				var matchPrizesList = matchPrizesResult.ToList();

				_logger.LogInformation("Loaded a total of {prizeCount} prizes for the game with uid {gameUid}",
					matchPrizesList.Count,
					gameUid);

				if (matchImagesList != null || matchImagesList.Count > 1)
				{
					foreach (var image in matchImagesList)
					{
						var prize = matchPrizesList.FirstOrDefault(c => c.Image.Uid == image.Uid);
						if (prize == null || prize.Value == 0.00m)
						{
							foundAndUpdated = true;
							_logger.LogInformation("Updating the image with uid {imageUid} as a loss image", image.Uid);

							await _repository.UpdateImageAsLossImageAsync(image, cancellationToken);
						}
					}
				}
			}

			if (foundAndUpdated)
				_logger.LogInformation("We found a loss image and have updated it for the game with uid {gameUid}", gameUid);
		}
	}
}
 