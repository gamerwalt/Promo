using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[ApiController]
	public class FacebookHookController : Controller
	{
		private readonly ILogger<FacebookHookController> _logger;

		public FacebookHookController(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<FacebookHookController>();
		}

		[HttpPost]
		[Route("/facebookhook/feedhook")]
		public async Task<IActionResult> Feedhook([FromBody] dynamic dataReceived)
		{
			if(dataReceived?.entry != null)
			{
				var entries = dataReceived.entry;

				foreach(var entry in entries)
				{
					var id = entry.Id as string;
					var time = entry.Time as string;

					_logger.LogInformation("Id received is {id}", id);
				}
			}

			return new OkObjectResult(new { message = "Thank you." });
		}
	}
}
