using System;

namespace hrvster.Models.Core
{
	public class ServiceOperation<TModel> where TModel : class
	{
		public ServiceOperation() { }
		public TModel Model { get; set; }

		public string Message { get; set; }
		public Exception Exception { get; set; }
		public DateTime ExceptionDate { get; set; }
		public string ActionName { get; set; }
	}
}
