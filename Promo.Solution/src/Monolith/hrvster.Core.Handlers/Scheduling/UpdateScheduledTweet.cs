using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Scheduling;
using hrvster.Models;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Scheduling
{
	public class UpdateScheduledTweet : AsyncRequestHandler<UpdateScheduledTweetRequest>
	{
		private readonly ILogger<UpdateScheduledTweet> _logger;
		private readonly ISweepTweetSchedulerRepository _repository;
		private readonly IFileService _fileService;
		public UpdateScheduledTweet(
			ILoggerFactory loggerFactory,
			ISweepTweetSchedulerRepository repository,
			IFileService fileService)
		{
			_logger = loggerFactory.CreateLogger<UpdateScheduledTweet>();
			_repository = repository;
			_fileService = fileService;
		}

		protected override async Task Handle(UpdateScheduledTweetRequest request, CancellationToken cancellationToken)
		{
			try
			{
				await _repository.BeginTransaction();
				await _repository.UpdateScheduledTweetAsync(request.ScheduledUid, request.ScheduledTime, request.Text);

				if (request.PreviousMediaId1 != 0 && request.Media1 != null)
				{
					await UpdatePreviousMedia(request.Media1, request.PreviousMediaId1, request.UserUid);
				}

				if (request.PreviousMediaId2 != 0 && request.Media2 != null)
				{
					await UpdatePreviousMedia(request.Media2, request.PreviousMediaId2, request.UserUid);
				}

				if (request.PreviousMediaId3 != 0 && request.Media3 != null)
				{
					await UpdatePreviousMedia(request.Media3, request.PreviousMediaId3, request.UserUid);
				}

				if (request.PreviousMediaId4 != 0 && request.Media4 != null)
				{
					await UpdatePreviousMedia(request.Media4, request.PreviousMediaId4, request.UserUid);
				}

				await _repository.CommitTransaction();
			}
			catch (Exception ex)
			{
				_logger.LogError("There was an error handling the action {className} for the content {content}. Failure: {message}.",
					nameof(UpdateScheduledTweet),
					request.ToJsonString(),
					ex.Message);
				_logger.LogError("Stack: {stack}", ex.StackTrace);

				await _repository.RollbackTransaction();
			}
		}

		private async Task UpdatePreviousMedia(IFormFile media, int imageId, Guid userUid)
		{
			var location = await UploadMediaToServerAsync(media, userUid);
			await _repository.UpdateScheduledTweetImageAsync(imageId, location);
		}

		private async Task<string> UploadMediaToServerAsync(IFormFile media1, Guid userUid)
		{
			return await _fileService.UploadToFileServer(media1, ProfileType.Twitter.ToString(), userUid);
		}
	}
}
