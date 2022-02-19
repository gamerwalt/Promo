using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Core;
using Promo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Membership.API.Controllers.v1
{
	[ApiController]
	[Route("v1")]
	public class WatchTypesController : ControllerBase
	{
		private readonly ILogger<WatchTypesController> _logger;

		public WatchTypesController(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<WatchTypesController>();
		}

		[HttpGet("watchtypes")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<WatchTypeCode>))]
		public async Task<ActionResult<IEnumerable<WatchTypeCode>>> GetWatchTypes()
		{
			var watchTypeCodes = Enum.GetValues(typeof(WatchTypeCode))
				.Cast<WatchTypeCode>()
				.Select(c => c.ToString());

			return new OkObjectResult(new ApiResponse(watchTypeCodes, 200));
		}
	}
}
