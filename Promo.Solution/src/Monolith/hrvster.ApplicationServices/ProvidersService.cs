using hrvster.Infrastructure.Core;
using hrvster.Models.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices
{
	public interface IProvidersService
	{
		Task<IEnumerable<WatchTypeProvider>> GetWathcTypeProvidersAsync();
	}

	public class ProvidersService : IProvidersService
	{
		private readonly IWatchTypeProvidersRepository _watchTypeProvidersRepository;

		public ProvidersService(
			IWatchTypeProvidersRepository watchTypeProvidersRepository)
		{
			_watchTypeProvidersRepository = watchTypeProvidersRepository;
		}

		public async Task<IEnumerable<WatchTypeProvider>> GetWathcTypeProvidersAsync()
			=> await _watchTypeProvidersRepository.GetWatchTypeProviders();
	}
}
