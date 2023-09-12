namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    private readonly ConcurrentDictionary<string, IModel> _subscriberChannelDic = new();
    private const string XDeath = "x-death";
    private const string XDeathCount = "count";

    private void Listen<T>(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        Func<Action<T?>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var channel = GetConsumerChannel(exchangeParam, queueParam, prefetchCount: prefetchCount);
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += OnConsumerOnReceived;
        channel.BasicConsume(queue: queueParam.Queue, autoAck: false, consumer: consumer);
        return;

        void OnConsumerOnReceived(object model, BasicDeliverEventArgs ea)
        {
            try
            {
                var body = ea.Body;
                var msg = _serializer.FromBytes<T>(body.ToArray());
                resolve.Invoke()(msg);
            }
            finally
            {
                channel.BasicAck(ea.DeliveryTag, false);
            }
        }
    }

    private void Consume<T>(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam,
        Func<Action<T?>> resolve,
        int consumeRetry = Consts.DefaultConsumeRetry,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var channel = GetConsumerChannel(
            exchangeParam,
            queueParam,
            dlxExchangeParam,
            dlxQueueParam,
            prefetchCount);
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += OnConsumerOnReceived;
        channel.BasicConsume(queue: queueParam.Queue, autoAck: false, consumer: consumer);
        return;

        void OnConsumerOnReceived(object model, BasicDeliverEventArgs ea)
        {
            try
            {
                var body = ea.Body;
                var msg = _serializer.FromBytes<T>(body.ToArray());
                resolve.Invoke()(msg);
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                var retryCount = GetRetryCount(ea.BasicProperties);
                channel.BasicNack(ea.DeliveryTag, false, retryCount < consumeRetry);
            }
        }
    }

    private static long? GetRetryCount(IBasicProperties properties)
    {
        if (!properties.Headers.TryGetValue(XDeath, out var header)) return null;
        var deathProperties = (List<object>)header;
        var lastRetry = (Dictionary<string, object>)deathProperties[0];
        var count = lastRetry[XDeathCount];
        return (long)count;
    }
}