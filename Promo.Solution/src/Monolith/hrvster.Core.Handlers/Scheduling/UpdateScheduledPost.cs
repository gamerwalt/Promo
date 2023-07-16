using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests.Files;
using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Scheduling;
using hrvster.Models;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Scheduling
{
	public class UpdateScheduledPost : AsyncRequestHandler<UpdateScheduledPostRequest>
	{
		private readonly ILogger<UpdateScheduledPost> _logger;
		private readonly IScheduledPostRepository _repository;
		private readonly IMediator _mediator;
		private readonly ISessionManager _sessionManager;

		private HrvsterUser _hrvsterUser;

		public UpdateScheduledPost(
			ILoggerFactory loggerFactory,
			IScheduledPostRepository repository,
			IMediator mediator,
			ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<UpdateScheduledPost>();
			_repository = repository;
			_mediator = mediator;
			_sessionManager = sessionManager;
		}

		protected override async Task Handle(UpdateScheduledPostRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Updating scheduled post for the scheduled post {scheduledPostUid}.", request.ScheduledUid);

			_hrvsterUser = _sessionManager.GetUserInformation();

			try
			{
				await _repository.BeginTransactionAsync();
				await _repository.UpdateScheduledPostAsync(
					request.ScheduledUid, 
					request.ScheduledTime, 
					request.Text, 
					request.TimeZoneId, 
					request.IsDaylight);

				if(NoPreviuosMedia(request))
				{
					var media = new List<IFormFile>();
					var mediaLocation = default(IEnumerable<string>);

					if (request.Media1 != null) media.Add(request.Media1);
					if (request.Media2 != null) media.Add(request.Media2);
					if (request.Media3 != null) media.Add(request.Media3);
					if (request.Media4 != null) media.Add(request.Media4);
					
					mediaLocation = await AddMediaAsync(request, media, mediaLocation);
				}
				else
				{
					var media = new List<IFormFile>();
					var mediaLocation = default(IEnumerable<string>);

					if (request.PreviousMediaId1 != 0 && request.Media1 != null)
					{
						await UpdatePreviousMedia(request.Media1, request.PreviousMediaId1, request.UserUid);
					}
					else if(request.Media1 != null)
					{
						media.Add(request.Media1);
						mediaLocation = await AddMediaAsync(request, media, mediaLocation);
						media.Remove(request.Media1);
					}

					if (request.PreviousMediaId2 != 0 && request.Media2 != null)
					{
						await UpdatePreviousMedia(request.Media2, request.PreviousMediaId2, request.UserUid);
					}
					else if(request.Media2 != null) //means we just added this
					{
						media.Add(request.Media2);
						mediaLocation = await AddMediaAsync(request, media, mediaLocation);
						media.Remove(request.Media2);
					}

					if (request.PreviousMediaId3 != 0 && request.Media3 != null)
					{
						await UpdatePreviousMedia(request.Media3, request.PreviousMediaId3, request.UserUid);
					}
					else if (request.Media3 != null)
					{
						media.Add(request.Media3);
						mediaLocation = await AddMediaAsync(request, media, mediaLocation);
						media.Remove(request.Media3);
					}

					if (request.PreviousMediaId4 != 0 && request.Media4 != null)
					{
						await UpdatePreviousMedia(request.Media4, request.PreviousMediaId4, request.UserUid);
					}
					else if (request.Media4 != null)
					{
						media.Add(request.Media4);
						mediaLocation = await AddMediaAsync(request, media, mediaLocation);
						media.Remove(request.Media4);
					}
				}				

				await _repository.CommitTransactionAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("There was an error handling the action {className} for the content {content}. Failure: {message}.",
					nameof(UpdateScheduledPost),
					request.ToJsonString(),
					ex.Message);
				_logger.LogError("Stack: {stack}", ex.StackTrace);

				await _repository.RollbackTransactionAsync();
			}
		}

		private async Task<IEnumerable<string>> AddMediaAsync(UpdateScheduledPostRequest request, List<IFormFile> media, IEnumerable<string> mediaLocation)
		{
			if (media.Count > 0)
				mediaLocation = await _mediator.Send(new UploadFileToFileServerRequest(media, _hrvsterUser.UserUid, ProfileType.Twitter, _hrvsterUser.Token));

			foreach (var location in mediaLocation)
			{
				await _repository.CreateScheduledPostImageAsync(
					request.ScheduledUid,
					Guid.NewGuid(),
					location);
			}

			return mediaLocation;
		}

		private bool NoPreviuosMedia(UpdateScheduledPostRequest request)
		{
			return request.PreviousMediaId1 == 0 &&
				request.PreviousMediaId2 == 0 &&
				request.PreviousMediaId3 == 0 &&
				request.PreviousMediaId4 == 0;
		}

		private async Task UpdatePreviousMedia(IFormFile media, int imageId, Guid userUid)
		{
			var location = await UploadMediaToServerAsync(media, userUid);

			await _repository.UpdateScheduledPostImageAsync(imageId, location);
		}

		private async Task<string> UploadMediaToServerAsync(IFormFile media, Guid userUid)
		{
			var mediaLocation = await _mediator.Send(new UploadFileToFileServerRequest(
				new List<IFormFile>() { media },
				_hrvsterUser.UserUid,
				ProfileType.Twitter,
				_hrvsterUser.Token));

			return mediaLocation.FirstOrDefault();
		}

		private string GetFilePath(Guid userUid, string addToPath)
		{
			//return Path.Combine(_fileServerOptions.Path, userUid.ToString(), addToPath);
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "hrvster", userUid.ToString(), addToPath);
		}
	}
}
