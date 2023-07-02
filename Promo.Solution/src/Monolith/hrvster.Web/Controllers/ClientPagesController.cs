using AutoMapper;
using hrvster.ApplicationServices.Accounts;
using hrvster.Core.Authentication;
using hrvster.Models;
using hrvster.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

		private HrvsterUser _hrvsterUser;

		public ClientPagesController(
			ILoggerFactory loggerFactory,
			ICompanyImagesService services,
			IMapper mapper,
			ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<ClientPagesController>();
			_services = services;
			_mapper = mapper;
			_sessionManager = sessionManager;
		}

		public async Task<IActionResult> Index()
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var clientImages = await _services.GetClientImagesAsync(_hrvsterUser.UserUid);

			var model = new ViewClientPages
			{
				Images = _mapper.Map<ClientImages>(clientImages),
			};

			return View(model);
		}
	}
}
