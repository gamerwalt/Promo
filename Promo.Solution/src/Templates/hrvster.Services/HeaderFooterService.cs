using hrvster.Templates.Infrastructure.Customizations;

namespace hrvster.Services
{
	public interface IHeaderFooterService
	{
		Task<string> GetHeader(string subdomain, CancellationToken cancellationToken);
		Task<string> GetFooter(string subdomain, CancellationToken cancellationToken);
	}

	public class HeaderFooterService : IHeaderFooterService
	{
		private readonly ILayoutRepository _layoutRepository;

		public HeaderFooterService(ILayoutRepository layoutRepository)
		{
			_layoutRepository = layoutRepository;
		}

		public Task<string> GetFooter(string subdomain, CancellationToken cancellationToken)
			=> _layoutRepository.GetFooterUrlStringAsync(subdomain, cancellationToken);

		public Task<string> GetHeader(string subdomain, CancellationToken cancellationToken)
			=> _layoutRepository.GetHeaderUrlStringAsync(subdomain, cancellationToken);
	}
}
