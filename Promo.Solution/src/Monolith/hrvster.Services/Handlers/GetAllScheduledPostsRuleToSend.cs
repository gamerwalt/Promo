using hrvster.Infrastructure.Scheduling;
using hrvster.Models.Scheduling;
using hrvster.Services.Requests;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services.Handlers
{
	public class GetAllScheduledPostsRuleToSend
		: IRequestHandler<GetAllScheduledPostsToSendRquest, IEnumerable<ScheduledPost>>
	{
		private readonly ILogger<GetAllScheduledPostsRuleToSend> _logger;
		private readonly IScheduledPostRepository _repository;

		public GetAllScheduledPostsRuleToSend(
			ILoggerFactory loggerFactory,
			IScheduledPostRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetAllScheduledPostsRuleToSend>();
			_repository = repository;
		}

		public async Task<IEnumerable<ScheduledPost>> Handle(
			GetAllScheduledPostsToSendRquest request,
			CancellationToken cancellationToken)
		{
			var scheduledPosts = await _repository.GetAllToSendAsync();
			var scheduledPostList = scheduledPosts.ToList();

			_logger.LogInformation("Pulled {count} scheduled sweep rule tweets to send.", scheduledPostList.Count);

			foreach (var post in scheduledPosts)
			{
				var images = await _repository.GetAllImagesAsync(post.Id);
				var imageList = images.ToList();

				_logger.LogInformation("Pulled {count} images for the scheduled id {scheduledUid} to send.",
					imageList.Count,
					post.Uid);

				post.Images = imageList;
			}

			return scheduledPosts;
		}
	}
}
