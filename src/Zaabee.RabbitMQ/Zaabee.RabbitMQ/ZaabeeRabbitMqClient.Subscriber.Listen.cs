namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void Listen<T>(
        Func<Action<T?>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, false);
        var queueParam = GetQueueParam(queue, false, SubscribeType.Listen);
        Subscribe(exchangeParam, queueParam, resolve, false, prefetchCount);
    }

    public void Listen<T>(
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var topic = GetTypeName(typeof(T));
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, false);
        var queueParam = GetQueueParam(queue, false, SubscribeType.Listen);
        Subscribe(exchangeParam, queueParam, resolve, false, prefetchCount);
    }

    public void Listen<T>(
        string topic,
        Func<Action<T?>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, false);
        var queueParam = GetQueueParam(queue, false, SubscribeType.Listen);
        Subscribe(exchangeParam, queueParam, resolve, false, prefetchCount);
    }

    public void Listen<T>(
        string topic,
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, false);
        var queueParam = GetQueueParam(queue, false, SubscribeType.Listen);
        Subscribe(exchangeParam, queueParam, resolve, false, prefetchCount);
    }

    public void Listen(
        string topic,
        Func<Action<byte[]>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, false);
        var queueParam = GetQueueParam(queue, false, SubscribeType.Listen);
        Subscribe(exchangeParam, queueParam, resolve, false, prefetchCount);
    }

    public void Listen(
        string topic,
        Func<Func<byte[], Task>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var queue = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";
        var exchangeParam = GetExchangeParam(topic, false);
        var queueParam = GetQueueParam(queue, false, SubscribeType.Listen);
        Subscribe(exchangeParam, queueParam, resolve, false, prefetchCount);
    }
}