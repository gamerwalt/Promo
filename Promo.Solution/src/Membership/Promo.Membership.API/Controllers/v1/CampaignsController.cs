using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Promo.Clients.External;

namespace Promo.Membership.API.Controllers.v1
{
	[ApiController]
	[Route("v1")]
	public class CampaignsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public CampaignsController(
			IIdentityClient identityClient,
			IMediator mediator)
		{
			_identityClient = identityClient;
			_mediator = mediator;
		}


	}
}
