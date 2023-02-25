using hrvster.Core.Requests.WebRequests.Files;
using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
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
		Task<IEnumerable<MatchImage>> GetForMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken);
		Task<MatchImage> GetImageAsync(Guid imageUid, CancellationToken cancellationToken);
		Task UpdateImageAsync(Guid matchImageUid, MatchGame matchGame, IFormFile image, CancellationToken cancellationToken);
	}

	public class MatchImagesService : IMatchImageService
	{
		private readonly ILogger<MatchImagesService> _logger;
		private readonly IMatchImagesRepository _repository;
		private readonly IMediator _mediator;

		public MatchImagesService(
			ILoggerFactory loggerFactory,
			IMatchImagesRepository repository,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<MatchImagesService>();
			_repository = repository;
			_mediator = mediator;
		}

		public async Task<bool> CreateImagesAsync(
			MatchGame matchGame,
			ICollection<IFormFile> images,
			CancellationToken cancellationToken)
		{
			try
			{
				var locations = await _mediator.Send(new UploadMatchImagesRequest(matchGame, images));

				var tasks = new List<Task>();

				foreach (var location in locations)
				{
					var matchImage = CreateMatchImage(matchGame, location);

					tasks.Add(_repository.CreateAsync(matchImage, cancellationToken));

					_logger.LogInformation("Saving the image {location} into the database for {gameName}",
						location,
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

				var locations = await _mediator.Send(new UploadMatchImagesRequest(matchGame, new[] { image }));

				_logger.LogInformation("Saved the image {imageName} at {location}",
					image.Name,
					locations.FirstOrDefault());

				var matchImage = CreateMatchImage(matchGame, locations.FirstOrDefault());

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

		private MatchImage CreateMatchImage(MatchGame matchGame, string location)
			=> new MatchImage
			{
				Uid = Guid.NewGuid(),
				LocationUrl = location,
				MatchGame = matchGame
			};

		public async Task<IEnumerable<MatchImage>> GetForMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken)
			=> await _repository.GetForMatchGamesAsync(matchGameUid, cancellationToken);

		public async Task<MatchImage> GetImageAsync(Guid imageUid, CancellationToken cancellationToken)
			=> await _repository.GetAsync(imageUid, cancellationToken);
	}
}
