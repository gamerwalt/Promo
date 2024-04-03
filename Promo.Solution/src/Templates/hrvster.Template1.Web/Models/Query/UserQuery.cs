using Microsoft.AspNetCore.WebUtilities;

namespace hrvster.Template1.Web.Models.Query
{
	public class UserQuery
	{
		public string UserId { get; set; }
		public string UserName { get; set; }

		public static UserQuery Parse(string queryString)
		{
			var queryDictionary = QueryHelpers.ParseNullableQuery(queryString);

			return new UserQuery
			{
				UserId = queryDictionary["userId"],
				UserName = queryDictionary["userName"]
			};
		}
	}
}
