using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Templates.Infrastructure.Customizations
{
	public interface ILayoutRepository
	{
		Task<string> GetFooterUrlStringAsync(string subdomain, CancellationToken cancellationToken);
		Task<string> GetHeaderUrlStringAsync(string subdomain, CancellationToken cancellationToken);
	}

	public class LayoutRepository : ILayoutRepository
	{
		public Task<string> GetFooterUrlStringAsync(string subdomain, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<string> GetHeaderUrlStringAsync(string subdomain, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
