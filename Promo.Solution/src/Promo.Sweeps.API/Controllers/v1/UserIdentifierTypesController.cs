using MediatR;
using Microsoft.AspNetCore.Mvc;
using Promo.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Sweeps.API.Controllers.v1
{
    [ApiController]
    [Route("v1/sweeps/useridentifiertypes")]
    public class UserIdentifierTypesController : Controller
    {
        private readonly IMediator _mediator;

        public UserIdentifierTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<UserIdentifierType>))]
        public async Task<ActionResult<IEnumerable<UserIdentifierType>>> Get()
        {
            var userIdentifierTypes = Enum.GetValues(typeof(UserIdentifierType))
                .Cast<UserIdentifierType>().Select(c => c.ToString());
            return Ok(userIdentifierTypes);
        }
    }
}
