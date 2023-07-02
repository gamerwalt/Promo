using hrvster.Infrastructure.Company;
using hrvster.Models.Company;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Accounts
{
	public interface ICompanyImagesService
	{
		Task<ClientImage> GetClientImagesAsync(Guid userUid);
	}

	public class CompanyImagesService : ICompanyImagesService
	{
		private readonly ICompanyImagesRepository _repository;

		public CompanyImagesService(ICompanyImagesRepository repository)
		{
			_repository = repository;
		}

		public async Task<ClientImage> GetClientImagesAsync(Guid userUid)
			=> await _repository.GetCompanyImages(userUid);
	}
}
