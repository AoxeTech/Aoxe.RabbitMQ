namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void SubscribeEvent<T>(
        Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        SubscribeMessage(resolve, true, prefetchCount, retry, dlx);

    public void SubscribeEvent<T>(
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        SubscribeMessage(resolve, true, prefetchCount, retry, dlx);

    public void SubscribeEvent<T>(
        string topic,
        Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        SubscribeMessage(topic, resolve, true, prefetchCount, retry, dlx);

    public void SubscribeEvent<T>(
        string topic,
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        SubscribeMessage(topic, resolve, true, prefetchCount, retry, dlx);

    public void SubscribeEvent<T>(
        string topic,
        Func<Action<byte[]>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        SubscribeMessage<T>(topic, resolve, true, prefetchCount, retry, dlx);

    public void SubscribeEvent<T>(
        string topic,
        Func<Func<byte[], Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        SubscribeMessage<T>(topic, resolve, true, prefetchCount, retry, dlx);
}