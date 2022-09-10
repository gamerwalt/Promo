using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hrvster.Extensions.ModelExtensions
{
	public static class ModelErrorExtensions
	{
		public static string BuildMessageFromErrors(
			this ModelStateDictionary ModelState,
			string messageReplace = null, 
			string replaceWith = null)
		{
			var errors = ModelState.Select(c => c.Value.Errors)
				.Where(c => c.Count > 0)
				.ToList();

			var sb = new StringBuilder();
			sb.Append("<ol>");
			foreach (var error in errors)
			{
				if(string.IsNullOrWhiteSpace(messageReplace))
				{

					sb.Append($"<li>{error.FirstOrDefault()?.ErrorMessage}</li>");
				}
				else
				{
					sb.Append($"<li>{error.FirstOrDefault()?.ErrorMessage?.Replace(messageReplace, replaceWith)}</li>");
				}
			}

			sb.Append("</ol>");

			return sb.ToString();
		}
	}
}
