using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promo.Rules.Core.RequestHandlers;
using Promo.Rules.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Tweetinvi.Models.Entities;

namespace Promo.Rules.API.Controllers
{
    [ApiController]
    [Route("v1")]
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<Rule>>> AddRule([FromBody] IEnumerable<Infrastructure.Models.Rule> rules)
        {
            var rulesAdded = await _mediator.Send(new AddCampaignRules(rules));

            return new OkObjectResult(rulesAdded);
        }

        [HttpGet("rules/hashtags")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Rule>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<Rule>>> GetRulesByHashtags([FromBody] List<string> hashTags)
        {
            var rules = await _mediator.Send(new GetRulesForHashTags(hashTags));

            return new OkObjectResult(rules);
        }

        [HttpGet("rule/{ruleUid:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Rule))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Rule>> GetRule([FromRoute] Guid ruleUid)
        {
            var rule = await _mediator.Send(new GetRule(ruleUid));

            return new OkObjectResult(rule);
        }

        [HttpGet("rules/{watchTypeCode}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Rule>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<Rule>>> GetRules([FromRoute] string watchTypeCode)
        {
            var unaddedRules = await _mediator.Send(new GetRules(watchTypeCode));

            return new OkObjectResult(unaddedRules);
        }

        [HttpPost("rule/{ruleUid:Guid}/added")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Rule))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Rule>> UpdateRuleAsAded([FromRoute] Guid ruleUid)
        {
            var rule = await _mediator.Send(new UpdateRuleAsAdded(ruleUid));
            
            return new OkObjectResult(rule);
        }
    }
}
