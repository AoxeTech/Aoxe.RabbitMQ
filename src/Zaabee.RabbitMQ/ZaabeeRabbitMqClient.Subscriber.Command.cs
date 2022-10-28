namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void ReceiveCommand<T>(
        Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        ReceiveMessage(resolve, true, prefetchCount, retry, dlx);

    public void ReceiveCommand<T>(
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        ReceiveMessage(resolve, true, prefetchCount, retry, dlx);

    public void ReceiveCommand<T>(
        string topic,
        Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        ReceiveMessage(topic, resolve, true, prefetchCount, retry, dlx);

    public void ReceiveCommand<T>(
        string topic,
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        ReceiveMessage(topic, resolve, true, prefetchCount, retry, dlx);

    public void ReceiveCommand<T>(
        string topic,
        Func<Action<byte[]>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        ReceiveMessage<T>(topic, resolve, true, prefetchCount, retry, dlx);

    public void ReceiveCommand<T>(
        string topic,
        Func<Func<byte[], Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        ReceiveMessage<T>(topic, resolve, true, prefetchCount, retry, dlx);
}