using hrvster.ApplicationServices.Files;
using hrvster.Infrastructure.Company;
using hrvster.Models;
using hrvster.Models.Company;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
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
	}

	public class CompanyImagesService : ICompanyImagesService
	{
		private readonly ICompanyImagesRepository _repository;
		private readonly IFileUploaderService _fileUploaderService;
		private readonly IMediator _mediator;

		public CompanyImagesService(
			ICompanyImagesRepository repository,
			IFileUploaderService fileUploaderService,
			IMediator mediator)
		{
			_repository = repository;
			_mediator = mediator;
			_fileUploaderService = fileUploaderService;
		}

		public async Task AddClientImagesAsync(
			HrvsterUser hrvsterUser,
			IFormFile logoLocation,
			IFormFile headerLocation,
			IFormFile footerLocation,
			CancellationToken cancellationToken)
		{
			var logoStorageFileLocationTask = _fileUploaderService.UploadToServiceAsync(logoLocation, hrvsterUser.Token, "Logo");
			var headerStorageFileLocationTask = _fileUploaderService.UploadToServiceAsync(headerLocation, hrvsterUser.Token, "Header");
			var footerStorageFileLocationTask = _fileUploaderService.UploadToServiceAsync(footerLocation, hrvsterUser.Token, "Footer");

			await Task.WhenAll(logoStorageFileLocationTask, headerStorageFileLocationTask, footerStorageFileLocationTask);

			var logoStorageFileLocation = await logoStorageFileLocationTask;
			var headerStorageFileLocation = await headerStorageFileLocationTask;
			var footerStorageFileLocation = await footerStorageFileLocationTask;

			var companyImages = new ClientImage
			{
				LogoLocation = logoStorageFileLocation,
				HeaderLocation = headerStorageFileLocation,
				FooterLocation = footerStorageFileLocation
			};

			await _repository.AddCompanyImagesAsync(hrvsterUser.UserUid, companyImages, cancellationToken);
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
			var logoStorageFileLocationTask = _fileUploaderService.UploadToServiceAsync(logoLocation, hrvsterUser.Token, "Logo");
			var headerStorageFileLocationTask = _fileUploaderService.UploadToServiceAsync(headerLocation, hrvsterUser.Token, "Header");
			var footerStorageFileLocationTask = _fileUploaderService.UploadToServiceAsync(footerLocation, hrvsterUser.Token, "Footer");

			await Task.WhenAll(logoStorageFileLocationTask, headerStorageFileLocationTask, footerStorageFileLocationTask);

			var logoStorageFileLocation = await logoStorageFileLocationTask;
			var headerStorageFileLocation = await headerStorageFileLocationTask;
			var footerStorageFileLocation = await footerStorageFileLocationTask;

			var companyImages = new ClientImage
			{
				LogoLocation = logoStorageFileLocation,
				HeaderLocation = headerStorageFileLocation,
				FooterLocation = footerStorageFileLocation
			};

			await _repository.UpdateCompanyImagesAsync(rowUid, companyImages, cancellationToken);
		}
	}
}
