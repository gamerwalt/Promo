using System.Text.Json;

namespace hrvster.Models
{
	public class ApiResponse
	{
		public object data { get; private set; }
		public int statusCode { get; private set; }

		public string message { get; set; }
		public string stackTrace { get; set; }

		public ApiResponse(object _data, int _statusCode)
		{
			data = _data;
			statusCode = _statusCode;
		}

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
