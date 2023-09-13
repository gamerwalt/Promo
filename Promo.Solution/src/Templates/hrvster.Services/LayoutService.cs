using hrvster.Templates.Infrastructure.Customizations;
using hrvster.Templates.Models.Layouts;

namespace hrvster.Templates.Services
{
	public interface ILayoutService
	{
		Task<LayoutProperty> GetLayoutPropertyAsync(Guid accountUid, CancellationToken cancellationToken);
	}

	public class LayoutService : ILayoutService
	{
		private readonly ILayoutRepository _layoutRepository;

		public LayoutService(ILayoutRepository layoutRepository)
		{
			_layoutRepository = layoutRepository;
		}

		public async Task<LayoutProperty> GetLayoutPropertyAsync(Guid accountUid, CancellationToken cancellationToken)
			=> await _layoutRepository.GetLayoutProperties(accountUid, cancellationToken);
	}
}
