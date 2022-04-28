namespace Zaabee.RabbitMQ.Internal;

internal static class ExceptionExtension
{
    public static Exception GetInmostException(this Exception ex)
    {
        if (ex.InnerException is null) return ex;
        var innerEx = ex.InnerException;
        while (innerEx.InnerException is not null)
            innerEx = innerEx.InnerException;
        return innerEx;
    }
}