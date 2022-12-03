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
		void SetKeyValue(string keyName, string value);
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

		public void SetKeyValue(string keyName, string value)
		{
			_httpContextAccessor.HttpContext.Session.SetString(keyName, value);
		}
	}
}
