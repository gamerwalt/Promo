using AutoMapper;
using hrvster.Infrastructure.Core;
using hrvster.Models.Core;
using hrvster.Models.WebModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices
{
	public interface IProvidersService
	{
		Task<IEnumerable<WatchType>> GetWatchTypesAsync();
		Task<IEnumerable<WatchTypeProvider>> GetWathcTypeProvidersAsync();
	}

	public class ProvidersService : IProvidersService
	{
		private readonly IWatchTypeProvidersRepository _watchTypeProvidersRepository;
		private readonly IWatchTypesRepository _watchTypesRepository;
		private readonly IMapper _mapper;

		public ProvidersService(
			IWatchTypeProvidersRepository watchTypeProvidersRepository,
			IWatchTypesRepository watchTypesRepository,
			IMapper mapper)
		{
			_watchTypeProvidersRepository = watchTypeProvidersRepository;
			_watchTypesRepository = watchTypesRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<WatchType>> GetWatchTypesAsync()
		{
			var watchTypes = await _watchTypesRepository.GetWatchTypesAsync();

			return _mapper.Map<IEnumerable<WatchType>>(watchTypes);
		}

		public async Task<IEnumerable<WatchTypeProvider>> GetWathcTypeProvidersAsync()
			=> await _watchTypeProvidersRepository.GetWatchTypeProviders();
	}
}
