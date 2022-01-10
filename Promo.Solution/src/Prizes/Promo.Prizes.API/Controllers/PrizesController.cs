using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promo.Prizes.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrizesController : ControllerBase
    {
        private readonly ILogger<PrizesController> _logger;

        public PrizesController(ILogger<PrizesController> logger)
        {
            _logger = logger;
        }
    }
}
