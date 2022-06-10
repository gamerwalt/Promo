using MySqlConnector;
using System;

namespace hrvster.Infrastructure.Extensions
{
	public static class MySqlDataReaderExtensions
	{
		public static string GetStringValue(this MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if (string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}

			return reader.GetString(fieldName);
		}

		public static long GetLongValue(this MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if (string.IsNullOrWhiteSpace(value))
			{
				return 0;
			}

			return reader.GetInt64(fieldName);
		}

		public static DateTime? GetNullableDateTimeValue(this MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			return reader.GetDateTime(fieldName);
		}
	}
}
