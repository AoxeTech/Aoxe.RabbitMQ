namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public async Task ReceiveEventAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var eventName = GetTypeName(typeof(T));
        await SubscribeEventAsync(eventName, eventName, resolve, prefetchCount);
    }

    public async Task ReceiveEventAsync<T>(Func<Func<T, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var eventName = GetTypeName(typeof(T));
        await SubscribeEventAsync(eventName, eventName, resolve, prefetchCount);
    }

    public async Task SubscribeEventAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var eventName = GetTypeName(typeof(T));
        await SubscribeEventAsync(eventName, resolve, prefetchCount);
    }

    public async Task SubscribeEventAsync<T>(Func<Func<T, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var eventName = GetTypeName(typeof(T));
        await SubscribeEventAsync(eventName, resolve, prefetchCount);
    }

    public async Task SubscribeEventAsync<T>(string exchange, Func<Action<T>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        await SubscribeEventAsync(exchange, queue, resolve, prefetchCount);
    }

    public async Task SubscribeEventAsync<T>(string exchange, Func<Func<T, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        await SubscribeEventAsync(exchange, queue, resolve, prefetchCount);
    }

    public async Task SubscribeEventAsync<T>(string exchange, string queue, Func<Action<T>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var exchangeParam = new ExchangeParam { Exchange = exchange };
        var queueParam = new QueueParam { Queue = queue };
        var channel = GetReceiverAsyncChannel(exchangeParam, queueParam, prefetchCount);

        await ConsumeEventAsync(channel, resolve, queueParam.Queue);
    }

    public async Task SubscribeEventAsync<T>(string exchange, string queue, Func<Func<T, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var exchangeParam = new ExchangeParam { Exchange = exchange };
        var queueParam = new QueueParam { Queue = queue };
        var channel = GetReceiverAsyncChannel(exchangeParam, queueParam, prefetchCount);

        await ConsumeEventAsync(channel, resolve, queueParam.Queue);
    }
}