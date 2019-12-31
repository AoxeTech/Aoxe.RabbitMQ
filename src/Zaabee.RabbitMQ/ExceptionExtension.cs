using System;

namespace Zaabee.RabbitMQ
{
    public static class ExceptionExtension
    {
        public static Exception GetInmostException(this Exception ex)
        {
            if (ex.InnerException is null) return ex;
            var innerEx = ex.InnerException;
            while (innerEx.InnerException != null)
                innerEx = innerEx.InnerException;
            return innerEx;
        }
    }
}