using hrvster.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;

namespace hrvster.Core.Authentication
{
	public interface ISessionManager
	{
		bool ContainsKey(string keyName);
		string GetValue(string keyName);
		void RemoveKeyValue(string userToken);
		void SetKeyValue(string keyName, string value);
		HrvsterUser SetUserInformation();
	}

	public class SessionManager : ISessionManager
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public SessionManager(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public bool ContainsKey(string keyName)
		{
			return _httpContextAccessor.HttpContext.Session.Keys.Contains(keyName);
		}

		public string GetValue(string keyName)
		{
			return _httpContextAccessor.HttpContext.Session.GetString(keyName);
		}

		public void RemoveKeyValue(string keyName)
		{
			if (ContainsKey(keyName))
			{
				_httpContextAccessor.HttpContext.Session.Remove(keyName);
			}
		}

		public void SetKeyValue(string keyName, string value)
		{
			_httpContextAccessor.HttpContext.Session.SetString(keyName, value);
		}

		public HrvsterUser SetUserInformation()
		{
			if (!ContainsKey(SessionConstants.UserUid))
				return null;

			var userUid = GetValue(SessionConstants.UserUid);

			return new HrvsterUser
			{
				UserUid = Guid.Parse(userUid),
				DisplayName = GetValue(SessionConstants.UserDisplayName),
				Token = GetValue(SessionConstants.UserToken),
				EmailAddress = GetValue(SessionConstants.UserEmail),
				Locale = GetValue(SessionConstants.Locale),
				WindowsTimeZone = GetValue(SessionConstants.WindowsLocale),
				IsDaylight = bool.Parse(GetValue(SessionConstants.IsDaylight)),
			};
		}
	}
}
