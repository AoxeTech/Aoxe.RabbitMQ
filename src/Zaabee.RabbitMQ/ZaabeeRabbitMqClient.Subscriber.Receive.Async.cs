namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public async Task ReceiveMessageAsync<T>(
        Func<Action<T?>> resolve,
        bool persistence = false,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(topic, MessageType.Message, SubscribeType.Receive);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ReceiveMessageAsync<T>(
        Func<Func<T?, Task>> resolve,
        bool persistence = false,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(topic, MessageType.Message, SubscribeType.Receive);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ReceiveMessageAsync<T>(
        string topic,
        Func<Action<T?>> resolve,
        bool persistence = false,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(topic, MessageType.Message, SubscribeType.Receive);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ReceiveMessageAsync<T>(
        string topic,
        Func<Func<T?, Task>> resolve,
        bool persistence = false,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(topic, MessageType.Message, SubscribeType.Receive);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ReceiveMessageAsync(
        string topic,
        Func<Action<byte[]>> resolve,
        bool persistence = false,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(topic, MessageType.Message, SubscribeType.Receive);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }

    public async Task ReceiveMessageAsync(
        string topic,
        Func<Func<byte[], Task>> resolve,
        bool persistence = false,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(topic, MessageType.Message, SubscribeType.Receive);
        await SubscribeAsync(exchangeParam, queueParam, resolve, MessageType.Message, prefetchCount);
    }
}