using AutoMapper;
using hrvster.ApplicationServices.Accounts;
using hrvster.Core.Authentication;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Models.Company;
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

		[Route("clientpages/myimages")]
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var clientImages = await _services.GetClientImagesAsync(_hrvsterUser.UserUid);

			if(clientImages == null)
			{
				_notificationService.AddInfoToastMessage("You need to create your client images.");

				return RedirectToAction("create");
			}

			var model = new ViewClientPages
			{
				Uid = clientImages == null ? Guid.Empty : clientImages.Uid,
				Images = _mapper.Map<ClientImages>(clientImages),
			};

			return View(model);
		}

		[HttpGet("clientpages/update/{uid:Guid}")]
		public async Task<IActionResult> Update(Guid uid)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var clientImages = await _services.GetClientImagesAsync(_hrvsterUser.UserUid);

			if (!await _services.BelongsToUserAsync(_hrvsterUser.UserUid, clientImages))
			{
				_logger.LogWarning("The company image uid {uid} does not belong to the user {userUid}", uid, _hrvsterUser.UserUid);

				_notificationService.AddWarningToastMessage("There has been an issue.");

				return RedirectToAction("Index");
			}

			_logger.LogInformation("Client images pulled and includes Logo: {logoLocation}", clientImages.LogoLocation);

			var model = new UpdateClientPages
			{
				Uid = clientImages.Uid,
				PreviousFooterLocation = clientImages.FooterLocation,
				PreviousHeaderLocation = clientImages.HeaderLocation,
				PreviousLogoLocation = clientImages.LogoLocation
			};

			return View(model);
		}

		[HttpPost("clientpages/update/{uid:Guid}")]
		public async Task<IActionResult> Update(Guid uid, UpdateClientPages form, CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();
			var clientImages = await _services.GetClientImagesAsync(_hrvsterUser.UserUid);

			if (!await _services.BelongsToUserAsync(_hrvsterUser.UserUid, clientImages))
			{
				_logger.LogWarning("The company image uid {uid} does not belong to the user {userUid}", uid, _hrvsterUser.UserUid);

				_notificationService.AddWarningToastMessage("There has been an issue.");

				return RedirectToAction("Index");
			}

			if(form.HeaderLocation == null && form.LogoLocation == null && form.FooterLocation == null)
			{
				_notificationService.AddInfoToastMessage("No images were updated.");

				return RedirectToAction("update", new { uid });
			}

			if (ModelState.IsValid)
			{
				await _services.UpdateClientImagesAsync(uid, _hrvsterUser, form.LogoLocation, form.HeaderLocation, form.FooterLocation, form.PreviousLogoLocation, form.PreviousHeaderLocation, form.PreviousFooterLocation, cancellationToken);

				_notificationService.AddSuccessToastMessage("Images updated successfully.");

				return RedirectToAction("update", new { uid = uid });
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return RedirectToAction("update", new { uid });
		}

		[HttpGet("clientpages/create")]
		public async Task<IActionResult> Create()
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var clientImages = await _services.GetClientImagesAsync(_hrvsterUser.UserUid);

			if (clientImages != null)
			{
				return RedirectToAction("Update", new { uid = clientImages.Uid });
			}

			var model = new CreateClientPages();

			return View(model);
		}

		[HttpPost("clientpages/create")]
		public async Task<IActionResult> Create(CreateClientPages form, CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			if (ModelState.IsValid)
			{
				await _services.AddClientImagesAsync(
					_hrvsterUser, 
					form.LogoLocation, 
					form.HeaderLocation, 
					form.FooterLocation, 
					cancellationToken);

				_notificationService.AddSuccessToastMessage("Images created successfully.");

				return RedirectToAction("Index");
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return RedirectToAction(nameof(Create));
		}
	}
}
