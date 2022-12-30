using hrvster.Infrastructure;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace hrvster.ApplicationServices.Payments
{
	public interface IPaymentService
	{

	}

	public class PaymentService : IPaymentService
	{
		private readonly ILogger<PaymentService> _logger;
		private readonly IPaymentServiceRepository _repository;
		private readonly Func<PaymentClientEnum, IPaymentAdaptor> _paymentAdaptorResolver;
		private readonly IPaymentAdaptor _paymentAdaptor;

		public PaymentService(
			ILoggerFactory loggerFactory,
			IPaymentServiceRepository repository,
			Func<PaymentClientEnum, IPaymentAdaptor> paymentAdaptorResolver)
		{
			_logger = loggerFactory.CreateLogger<PaymentService>();
			_repository = repository;
			_paymentAdaptorResolver = paymentAdaptorResolver;

			var paymentServiceModels = _repository.GetPaymentServices();
			var paymentServiceModel = paymentServiceModels.FirstOrDefault(p => p.EnabledIndicator);
			if (paymentServiceModel != null)
			{
				_paymentAdaptor = _paymentAdaptorResolver(Enum.Parse<PaymentClientEnum>(paymentServiceModel.PaymentServiceName));
			}
		}
	}
}
