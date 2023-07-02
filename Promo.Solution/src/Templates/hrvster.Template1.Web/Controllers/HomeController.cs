using hrvster.Template1.Web.Models;
using hrvster.Templates.Core.Common;
using hrvster.Templates.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace hrvster.Template1.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ILayoutService _headerFooterService;

		public HomeController(
			ILoggerFactory loggerFactory, 
			ILayoutService headerFooterService)
		{
			_logger = loggerFactory.CreateLogger<HomeController>();
			_headerFooterService = headerFooterService;
		}

		public async Task<IActionResult> Index(CancellationToken cancellationToken)
		{
			HttpContext.Items.TryGetValue(Constants.ContextItemsSubdomain, out var subdomain);
			var subdomainName = subdomain.ToString();

			ViewBag.TestData = subdomainName;

			_logger.LogInformation("Loading the HOme Controller index action.");

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}