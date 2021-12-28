using MediatR;
using Microsoft.AspNetCore.Mvc;
using Promo.Rules.Core.RequestHandlers;
using Promo.Rules.Infrastructure.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Rules.API.Controllers
{
    [ApiController]
    [Route("v1")]
    public class RulesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RulesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("rules")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Rule>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<Rule>>> AddRule([FromBody] IEnumerable<Promo.Core.Models.Rule> rules)
        {
            var rulesAdded = await _mediator.Send(new AddCampaignRules(rules));

            return new OkObjectResult(rulesAdded);
        }
    }
}
