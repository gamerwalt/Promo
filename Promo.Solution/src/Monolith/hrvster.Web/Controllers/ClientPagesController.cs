using AutoMapper;
using hrvster.ApplicationServices.Accounts;
using hrvster.Core.Authentication;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class ClientPagesController : Controller
	{
		private readonly ILogger<ClientPagesController> _logger;
		private readonly ICompanyImagesService _services;
		private readonly IMapper _mapper;
		private readonly ISessionManager _sessionManager;
		private readonly IToastNotification _notificationService;

		private HrvsterUser _hrvsterUser;

		public ClientPagesController(
			ILoggerFactory loggerFactory,
			ICompanyImagesService services,
			IMapper mapper,
			ISessionManager sessionManager,
			IToastNotification notificationService)
		{
			_logger = loggerFactory.CreateLogger<ClientPagesController>();
			_services = services;
			_mapper = mapper;
			_sessionManager = sessionManager;
			_notificationService = notificationService;
		}

		public async Task<IActionResult> Index()
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var clientImages = await _services.GetClientImagesAsync(_hrvsterUser.UserUid);

			var model = new ViewClientPages
			{
				Uid = clientImages.Uid,
				Images = _mapper.Map<ClientImages>(clientImages),
			};

			return View(model);
		}

		[HttpGet("update/{uid:Guid}")]
		public async Task<IActionResult> Update(Guid uid)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var clientImages = await _services.GetClientImagesAsync(_hrvsterUser.UserUid);

			var model = new UpdateClientPages
			{
				Uid = clientImages.Uid,
				PreviousFooterLocation = clientImages.FooterLocation,
				PreviousHeaderLocation = clientImages.HeaderLocation,
				PreviousLogoLocation = clientImages.LogoLocation
			};

			return View(model);
		}

		[HttpPost("update/{uid:Guid}")]
		public async Task<IActionResult> Update(Guid uid, UpdateClientPages form, CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			if (ModelState.IsValid)
			{
				await _services.UpdateClientImagesAsync(uid, _hrvsterUser, form.LogoLocation, form.HeaderLocation, form.FooterLocation, form.PreviousLogoLocation, form.PreviousHeaderLocation, form.PreviousFooterLocation, cancellationToken);

				_notificationService.AddSuccessToastMessage("Images updated successfully.");
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return RedirectToAction(nameof(Update), new { uid });
		}

		[HttpGet("create")]
		public async Task<IActionResult> Create()
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var model = new CreateClientPages();

			return View(model);
		}

		[HttpPost("create")]
		public async Task<IActionResult> Create(CreateClientPages form, CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			if(ModelState.IsValid)
			{
				await _services.AddClientImagesAsync(
					_hrvsterUser, 
					form.LogoLocation, 
					form.HeaderLocation, 
					form.FooterLocation, 
					cancellationToken);

				_notificationService.AddSuccessToastMessage("Images created successfully.");
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return RedirectToAction(nameof(Create));
		}
	}
}
