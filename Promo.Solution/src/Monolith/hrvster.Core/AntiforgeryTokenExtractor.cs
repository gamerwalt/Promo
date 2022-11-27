using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace hrvster.Core
{
	public static class AntiforgeryTokenExtractor
	{
		public static string AntiForgeryFieldName { get; } = "AntiForgeryTokenField";
		public static string AntiForgeryCookieName { get; } = "AntiForgeryTokenCookie";

		private static string ExtractAntiForgeryToken(string htmlBody)
		{
			var requestVerificationTokenMatch =
				Regex.Match(htmlBody, $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");
			if (requestVerificationTokenMatch.Success)
				return requestVerificationTokenMatch.Groups[1].Captures[0].Value;
			throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' not found in HTML", nameof(htmlBody));
		}

		private static string ExtractAntiForgeryCookieValueFrom(HttpResponseMessage response)
		{
			var antiForgeryCookie = response.Headers.GetValues("Set-Cookie")
				.FirstOrDefault(x => x.Contains(AntiForgeryCookieName));
			if (antiForgeryCookie is null)
				throw new ArgumentException($"Cookie '{AntiForgeryCookieName}' not found in HTTP response", nameof(response));
			var antiForgeryCookieValue = SetCookieHeaderValue.Parse(antiForgeryCookie).Value.ToString();
			return antiForgeryCookieValue;
		}

		public static string ExtractAntiforgeryToken(HttpResponseMessage response)
		{
			var token = string.Empty;

			Task.Run(async () => token = ExtractAntiForgeryToken(await response.Content.ReadAsStringAsync())).Wait();

			return token;
		}

		public static string ExtractAntiforgeryCookieValue(HttpResponseMessage response)
			=> ExtractAntiForgeryCookieValueFrom(response);
	}
}
