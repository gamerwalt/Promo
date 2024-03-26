using hrvster.ApplicationServices.Files;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Company;
using hrvster.Models;
using hrvster.Models.Company;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Accounts
{
	public interface ICompanyImagesService
	{
		Task<ClientImage> GetClientImagesAsync(Guid userUid);
		Task UpdateClientImagesAsync(
			Guid rowUid,
			HrvsterUser hrvsterUser,
			IFormFile logoLocation,
			IFormFile headerLocation,
			IFormFile footerLocation,
			string previousLogoLocation,
			string previousHeaderLocation,
			string previousFooterLocation,
			CancellationToken cancellationToken);
		Task AddClientImagesAsync(
			HrvsterUser hrvsterUser,
			IFormFile logoLocation,
			IFormFile headerLocation,
			IFormFile footerLocation,
			CancellationToken cancellationToken);
		Task<bool> BelongsToUserAsync(Guid userUid, ClientImage clientImages);
	}

	public class CompanyImagesService : ICompanyImagesService
	{
		private readonly ICompanyImagesRepository _repository;
		private readonly IFileUploaderService _fileUploaderService;
		private readonly IAccountCompanyRepository _accountCompanyRepository;
		private readonly IAccountsRepository _accountRepository;
		private readonly ILogger<CompanyImagesService> _logger;

		public CompanyImagesService(
			ILoggerFactory loggerFactory,
			ICompanyImagesRepository repository,
			IFileUploaderService fileUploaderService,
			IAccountCompanyRepository accountCompanyRepository,
			IAccountsRepository accountsRepository)
		{
			_repository = repository;
			_fileUploaderService = fileUploaderService;
			_accountCompanyRepository = accountCompanyRepository;
			_accountRepository = accountsRepository;
			_logger = loggerFactory.CreateLogger<CompanyImagesService>();
		}

		public async Task AddClientImagesAsync(
			HrvsterUser hrvsterUser,
			IFormFile logoLocation,
			IFormFile headerLocation,
			IFormFile footerLocation,
			CancellationToken cancellationToken)
		{
			var accountCompanies = await _accountCompanyRepository.GetAccountCompaniesAsync(hrvsterUser.UserUid);
			var accountCompany = accountCompanies.FirstOrDefault();

			var logoStorageFileLocation = await _fileUploaderService.UploadToServiceAsync(logoLocation, hrvsterUser.Token, accountCompany.Subdomain);
			var headerStorageFileLocation = await _fileUploaderService.UploadToServiceAsync(headerLocation, hrvsterUser.Token, accountCompany.Subdomain);
			var footerStorageFileLocation = await _fileUploaderService.UploadToServiceAsync(footerLocation, hrvsterUser.Token, accountCompany.Subdomain);

			//await Task.WhenAll(logoStorageFileLocationTask, headerStorageFileLocationTask, footerStorageFileLocationTask);

			//var logoStorageFileLocation = await logoStorageFileLocationTask;
			//var headerStorageFileLocation = await headerStorageFileLocationTask;
			//var footerStorageFileLocation = await footerStorageFileLocationTask;

			var companyImages = new ClientImage
			{
				LogoLocation = logoStorageFileLocation,
				HeaderLocation = headerStorageFileLocation,
				FooterLocation = footerStorageFileLocation
			};

			await _repository.AddCompanyImagesAsync(accountCompany.Id, companyImages, cancellationToken);
		}

		public async Task<bool> BelongsToUserAsync(Guid userUid, ClientImage clientImages)
		{
			if(clientImages == null)
			{
				return false;
			}

			var account = await _accountRepository.GetAsync(userUid);

			if(account == null)
			{
				_logger.LogWarning("Unable to load the account with uid {userUid}", userUid);

				return false;
			}

			var accountCompanies = await _accountCompanyRepository.GetAccountCompaniesAsync(account.AccountUid);
			var accountCompany = accountCompanies.FirstOrDefault();

			if(accountCompanies == null || accountCompany == null)
			{
				_logger.LogWarning("Unable to load the account companies or company for the account with uid {userUid}", userUid);

				return false;
			}

			if(clientImages.Company.Id != accountCompany.Id)
			{
				_logger.LogWarning("Account company does not match the company images the user is trying to match. AccountCompanyId : {accountCompanyId}, AccountUid: {accountUid}, ClientImageUid: {clientImagesUid}",
					accountCompany.Id,
					userUid,
					clientImages.Uid);

				return false;
			}

			return true;
		}

		public async Task<ClientImage> GetClientImagesAsync(Guid userUid)
			=> await _repository.GetCompanyImages(userUid);

		public async Task UpdateClientImagesAsync(
			Guid rowUid,
			HrvsterUser hrvsterUser,
			IFormFile logoLocation,
			IFormFile headerLocation,
			IFormFile footerLocation,
			string previousLogoLocation,
			string previousHeaderLocation,
			string previousFooterLocation,
			CancellationToken cancellationToken)
		{
			var accountCompanies = await _accountCompanyRepository.GetAccountCompaniesAsync(hrvsterUser.UserUid);
			var accountCompany = accountCompanies.FirstOrDefault();

			var logoStorageFileLocation = string.Empty;
			var headerStorageFileLocation = string.Empty;
			var footerStorageFileLocation = string.Empty;

			if (logoLocation != null)
			{
				logoStorageFileLocation = await _fileUploaderService.UploadToServiceAsync(logoLocation, hrvsterUser.Token, accountCompany.Subdomain);
			}

			if(headerLocation != null)
			{
				headerStorageFileLocation = await _fileUploaderService.UploadToServiceAsync(headerLocation, hrvsterUser.Token, accountCompany.Subdomain);
			}

			if(footerLocation != null)
			{
				footerStorageFileLocation = await _fileUploaderService.UploadToServiceAsync(footerLocation, hrvsterUser.Token, accountCompany.Subdomain);
			}

			var companyImages = new ClientImage
			{
				LogoLocation = string.IsNullOrWhiteSpace(logoStorageFileLocation) ? previousLogoLocation : logoStorageFileLocation,
				HeaderLocation = string.IsNullOrWhiteSpace(headerStorageFileLocation) ? previousHeaderLocation : headerStorageFileLocation,
				FooterLocation = string.IsNullOrWhiteSpace(footerStorageFileLocation) ? previousFooterLocation : footerStorageFileLocation
			};

			await _repository.UpdateCompanyImagesAsync(rowUid, companyImages, cancellationToken);
		}
	}
}
