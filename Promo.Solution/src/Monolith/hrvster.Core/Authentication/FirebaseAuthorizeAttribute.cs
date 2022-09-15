using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
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
		private readonly FirebaseAuth _firebaseAuth;
		private readonly ILogger<FirebaseAuthorizeFilter> _logger;

		public FirebaseAuthorizeFilter(
			HrvsterUser hrvsterUser, 
			FirebaseAuth firebaseAuth,
			ILoggerFactory loggerFactory)
		{
			User = hrvsterUser;
			_firebaseAuth = firebaseAuth;
			_logger = loggerFactory.CreateLogger<FirebaseAuthorizeFilter>();
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var token = context.HttpContext.Session.GetString(SessionConstants.UserToken);

			if (string.IsNullOrWhiteSpace(token))
				context.Result = new ForbidResult();
			else
			{
				try
				{
					var firebaseVerificationTokenResult = _firebaseAuth.VerifyIdTokenAsync(token).Result;

					User.UserUid = Guid.Parse(context.HttpContext.Session.GetString(SessionConstants.UserUid));
					User.DisplayName = context.HttpContext.Session.GetString(SessionConstants.UserDisplayName);
					User.Token = context.HttpContext.Session.GetString(SessionConstants.UserToken);
					User.EmailAddress = context.HttpContext.Session.GetString(SessionConstants.UserEmail);
				}
				catch (Exception ex)
				{
					context.Result = new ForbidResult();
				}
			}
		}
	}
}
