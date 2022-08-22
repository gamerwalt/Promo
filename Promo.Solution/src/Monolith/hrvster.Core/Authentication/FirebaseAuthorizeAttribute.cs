using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Text;

namespace hrvster.Core.Authentication
{
	public class FirebaseAuthorizeAttribute : TypeFilterAttribute
	{
		public FirebaseAuthorizeAttribute() : base(typeof(FirebaseAuthorizeFilter)) { }
	}

	public class FirebaseAuthorizeFilter : IAuthorizationFilter
	{
		public HrvsterUser User { get; set; }
		public FirebaseAuthorizeFilter(HrvsterUser hrvsterUser) 
		{
			User = hrvsterUser;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var token = context.HttpContext.Session.GetString(SessionConstants.UserToken);

			if (string.IsNullOrWhiteSpace(token))
			{
				context.Result = new ForbidResult();
			}
			else
			{
				User.UserUid = Guid.Parse(context.HttpContext.Session.GetString(SessionConstants.UserUid));
				User.DisplayName = context.HttpContext.Session.GetString(SessionConstants.UserDisplayName);
				User.Token = context.HttpContext.Session.GetString(SessionConstants.UserToken);
				User.EmailAddress = context.HttpContext.Session.GetString(SessionConstants.UserEmail);
			}
		}
	}
}
