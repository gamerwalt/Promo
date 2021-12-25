using System;

namespace Promo.Core.Exceptions
{
    public class ServiceAuthenticateException : Exception
    {
        public ServiceAuthenticateException() : base("Unable to authenticate service.") { }
    }
}
