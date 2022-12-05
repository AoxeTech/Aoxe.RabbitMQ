namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public async Task ListenMessageAsync<T>(
        Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Listen);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ListenMessageAsync<T>(
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Listen);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ListenMessageAsync<T>(
        string topic,
        Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Listen);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ListenMessageAsync<T>(
        string topic,
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Listen);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ListenMessageAsync(
        string topic,
        Func<Action<byte[]>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Listen);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ListenMessageAsync(
        string topic,
        Func<Func<byte[], Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(queue, MessageType.Message, SubscribeType.Listen);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }
}