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
			return new OkObjectResult(new { message = "Home" });
		}
	}
}