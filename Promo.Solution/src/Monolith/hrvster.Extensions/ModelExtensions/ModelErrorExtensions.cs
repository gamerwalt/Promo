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
			foreach (var error in errors)
			{
				if(string.IsNullOrWhiteSpace(messageReplace))
				{
					sb.Append("<br/>");
					sb.Append("<br/>");
					sb.Append(error.FirstOrDefault()?.ErrorMessage);
				}
				else
				{
					sb.Append("<br/>");
					sb.Append("<br/>");
					sb.Append(error.FirstOrDefault()?.ErrorMessage?.Replace(messageReplace, replaceWith));
				}
			}

			return sb.ToString();
		}
	}
}
