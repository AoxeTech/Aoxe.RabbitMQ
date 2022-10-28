namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public async Task SubscribeEventAsync<T>(
        Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        await SubscribeMessageAsync(resolve, true, prefetchCount, retry, dlx);

    public async Task SubscribeEventAsync<T>(
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        await SubscribeMessageAsync(resolve, true, prefetchCount, retry, dlx);

    public async Task SubscribeEventAsync<T>(
        string topic,
        Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        await SubscribeMessageAsync(topic, resolve, true, prefetchCount, retry, dlx);

    public async Task SubscribeEventAsync<T>(
        string topic,
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        await SubscribeMessageAsync(topic, resolve, true, prefetchCount, retry, dlx);

    public async Task SubscribeEventAsync<T>(
        string topic,
        Func<Action<byte[]>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        await SubscribeMessageAsync<T>(topic, resolve, true, prefetchCount, retry, dlx);

    public async Task SubscribeEventAsync<T>(
        string topic,
        Func<Func<byte[], Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 3,
        bool dlx = true) =>
        await SubscribeMessageAsync<T>(topic, resolve, true, prefetchCount, retry, dlx);
}