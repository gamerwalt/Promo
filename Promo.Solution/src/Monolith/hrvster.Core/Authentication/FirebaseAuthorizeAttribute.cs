using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		private readonly ISessionManager _sessionManager;
		private readonly IFirebaseAuthManager _firebaseAuthManager;

		public FirebaseAuthorizeFilter(
			HrvsterUser hrvsterUser, 
			FirebaseAuth firebaseAuth,
			ILoggerFactory loggerFactory,
			ISessionManager sessionManager,
			IFirebaseAuthManager firebaseAuthManager)
		{
			User = hrvsterUser;
			_firebaseAuth = firebaseAuth;
			_logger = loggerFactory.CreateLogger<FirebaseAuthorizeFilter>();
			_sessionManager = sessionManager;
			_firebaseAuthManager = firebaseAuthManager;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			_logger.LogInformation("Attempting to load {path}", context.HttpContext.Request.Path);

			var token = string.Empty;

			if(_sessionManager.ContainsKey(SessionConstants.UserToken))
			{
				token = _sessionManager.GetValue(SessionConstants.UserToken);
			}

			if (string.IsNullOrWhiteSpace(token))
			{
				_logger.LogWarning("Request forbidden");
				context.Result = new ForbidResult();

				var displayNameValue = context.HttpContext.Request.Headers[SessionConstants.UserDisplayName].ToList();
				User.DisplayName = displayNameValue.FirstOrDefault();
			}
			else
			{
				_logger.LogWarning("Request passed. Contains token");
				try
				{
					if(_firebaseAuthManager.VerifyToken(token))
					{
						User.UserUid = Guid.Parse(_sessionManager.GetValue(SessionConstants.UserUid));
						User.DisplayName = _sessionManager.GetValue(SessionConstants.UserDisplayName);
						User.Token = _sessionManager.GetValue(SessionConstants.UserToken);
						User.EmailAddress = _sessionManager.GetValue(SessionConstants.UserEmail);
					}
					else
					{
						context.Result = new ForbidResult();
					}
				}
				catch (Exception ex)
				{
					context.Result = new ForbidResult();
				}
			}
		}
	}
}
