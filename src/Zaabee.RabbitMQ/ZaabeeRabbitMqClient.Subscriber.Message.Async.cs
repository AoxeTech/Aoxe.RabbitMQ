namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public async Task ReceiveMessageAsync<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(topic, MessageType.Message, SubscribeType.Receive);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ReceiveMessageAsync<T>(Func<Func<T?, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(topic, MessageType.Message, SubscribeType.Receive);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task SubscribeMessageAsync<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Subscribe);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task SubscribeMessageAsync<T>(Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Subscribe);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task SubscribeMessageAsync<T>(string topic, Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Subscribe);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task SubscribeMessageAsync<T>(string topic, Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = GetQueueName(resolve);
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Subscribe);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ListenMessageAsync<T>(Func<Action<T?>> resolve, ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Listen);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ListenMessageAsync<T>(Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Listen);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ListenMessageAsync<T>(string topic, Func<Action<T?>> resolve, ushort prefetchCount = 10)
    {
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Listen);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ListenMessageAsync<T>(string topic, Func<Func<T?, Task>> resolve, ushort prefetchCount = 10)
    {
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Listen);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }
}