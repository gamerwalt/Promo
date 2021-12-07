using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Web.API.Controllers
{
    [ApiController]
    [Route("v1")]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(ILogger<AccountsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("accounts")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Account>))]
        public async Task<ActionResult<IEnumerable<Account>>> Get()
        {
            return new OkObjectResult(default(List<Account>));
        }
    }
}
