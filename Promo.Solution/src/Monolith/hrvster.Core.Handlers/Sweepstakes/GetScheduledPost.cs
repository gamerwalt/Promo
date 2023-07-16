using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure.Scheduling;
using hrvster.Models.Extensions;
using hrvster.Models.Scheduling;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class GetScheduledPost : IRequestHandler<GetScheduledPostRequest, ScheduledPost>
	{
		private readonly ILogger<GetScheduledPost> _logger;
		private readonly IScheduledPostRepository _repository;

		public GetScheduledPost(
			ILoggerFactory loggerFactory, 
			IScheduledPostRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetScheduledPost>();
			_repository = repository;
		}

		public async Task<ScheduledPost> Handle(GetScheduledPostRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Getting scheduled post for {rule}", request.Rule.ToJsonString());

			var scheduledPost = await _repository.GetScheduledPostAsync(request.Rule.RuleId);

			var images = await _repository.GetAllImagesAsync(scheduledPost.Id);

			scheduledPost.Images = images.ToList();

			return scheduledPost;
		}
	}
}
