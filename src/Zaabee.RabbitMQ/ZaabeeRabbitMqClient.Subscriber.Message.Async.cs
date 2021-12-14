namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public async Task ReceiveMessageAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var messageName = GetTypeName(typeof(T));
        await SubscribeMessageAsync(messageName, messageName, resolve, prefetchCount);
    }

    public async Task ReceiveMessageAsync<T>(Func<Func<T, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var messageName = GetTypeName(typeof(T));
        await SubscribeMessageAsync(messageName, messageName, resolve, prefetchCount);
    }

    public async Task SubscribeMessageAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var messageName = GetTypeName(typeof(T));
        await SubscribeMessageAsync(messageName, resolve, prefetchCount);
    }

    public async Task SubscribeMessageAsync<T>(Func<Func<T, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var messageName = GetTypeName(typeof(T));
        await SubscribeMessageAsync(messageName, resolve, prefetchCount);
    }

    public async Task SubscribeMessageAsync<T>(string exchange, Func<Action<T>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        await SubscribeMessageAsync(exchange, queue, resolve, prefetchCount);
    }

    public async Task SubscribeMessageAsync<T>(string exchange, Func<Func<T, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        await SubscribeMessageAsync(exchange, queue, resolve, prefetchCount);
    }

    public async Task SubscribeMessageAsync<T>(string exchange, string queue, Func<Action<T>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var exchangeParam = new ExchangeParam { Exchange = exchange, Durable = false };
        var queueParam = new QueueParam { Queue = queue, Durable = false };
        var channel = GetReceiverAsyncChannel(exchangeParam, queueParam, prefetchCount);

        await ConsumeMessageAsync(channel, resolve, queueParam.Queue);
    }

    public async Task SubscribeMessageAsync<T>(string exchange, string queue, Func<Func<T, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var exchangeParam = new ExchangeParam { Exchange = exchange, Durable = false };
        var queueParam = new QueueParam { Queue = queue, Durable = false };
        var channel = GetReceiverAsyncChannel(exchangeParam, queueParam, prefetchCount);

        await ConsumeMessageAsync(channel, resolve, queueParam.Queue);
    }

    public async Task ListenMessageAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var exchangeName = GetTypeName(typeof(T));
        var queueName = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";

        var exchangeParam = new ExchangeParam { Exchange = exchangeName, Durable = false };
        var queueParam = new QueueParam { Queue = queueName, Durable = false, Exclusive = true, AutoDelete = true };
        var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

        await ConsumeMessageAsync(channel, resolve, queueParam.Queue);
    }

    public async Task ListenMessageAsync<T>(Func<Func<T, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var exchangeName = GetTypeName(typeof(T));
        var queueName = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";

        var exchangeParam = new ExchangeParam { Exchange = exchangeName, Durable = false };
        var queueParam = new QueueParam { Queue = queueName, Durable = false, Exclusive = true, AutoDelete = true };
        var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

        await ConsumeMessageAsync(channel, resolve, queueParam.Queue);
    }
}