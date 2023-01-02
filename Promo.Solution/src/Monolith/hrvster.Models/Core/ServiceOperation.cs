using System;

namespace hrvster.Models.Core
{
	public class ServiceOperation<TModel> where TModel : class
	{
		public ServiceOperation(TModel model, string message) 
		{ 
			Model = model;
			Message = message;
		}

		public TModel Model { get; private set; }
		public string Message { get; private set; }
		public bool HasError { get; private set; }
		public DateTime ExceptionDate { get; private set; }
		public string ActionName { get; private set; }

		public static ServiceOperation<TModel> CreateFailedOperation(TModel model, string message, string action)
		{
			var data = new ServiceOperation<TModel>(model, message)
			{
				ExceptionDate = DateTime.Now,
				HasError = true,
				ActionName = action
			};

			return data;
		}

		public static ServiceOperation<TModel> CreateSuccessOperation(TModel model, string message)
		{
			var data = new ServiceOperation<TModel>(model, message)
			{
				HasError = false
			};

			return data;
		}
	}
}
