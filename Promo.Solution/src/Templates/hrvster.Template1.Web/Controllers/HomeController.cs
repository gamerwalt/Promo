using Microsoft.AspNetCore.Mvc;

namespace hrvster.Template1.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<HomeController>();
		}

		public async Task<IActionResult> Index(CancellationToken cancellationToken)
		{
			//HttpContext.Items.TryGetValue(Constants.ContextItemsSubdomain, out var subdomain);
			//var subdomainName = subdomain.ToString();

			//ViewBag.TestData = subdomainName;

			//_logger.LogInformation("Loading the HOme Controller index action.");

			//return View();
			return new OkObjectResult(new { message = "Home" });
		}
	}
}