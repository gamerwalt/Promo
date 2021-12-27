using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Rules.API.Controllers
{
    [ApiController]
    [Route("v1/ruletypes")]
    public class RuleTypesController : ControllerBase
    {
        private readonly ILogger<RuleTypesController> _logger;

        public RuleTypesController(ILogger<RuleTypesController> logger)
        {
            _logger = logger;
        }
    }
}
