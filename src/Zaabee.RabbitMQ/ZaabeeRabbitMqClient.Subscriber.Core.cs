namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    private const string RetryCount = "retry-count";

    private void Consume<T>(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam,
        Func<Action<T?>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry)
    {
        var retryExchangeParam = consumeRetry > 0
            ? CreateExchangeParam(queueParam.Queue, true, ExchangeRole.Retry)
            : null;
        var channel = GetConsumerChannel(
            exchangeParam,
            queueParam,
            retryExchangeParam,
            dlxExchangeParam,
            dlxQueueParam,
            prefetchCount);
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (_, ea) =>
        {
            var consumerChannel = consumer.Model;
            try
            {
                var body = ea.Body;
                var msg = _serializer.FromBytes<T>(body.ToArray());
                resolve.Invoke()(msg);
                consumerChannel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                var retryCount = GetRetryCount(ea.BasicProperties);
                IncreaseRetryCount(ea.BasicProperties);
                if (retryCount < consumeRetry && retryExchangeParam is not null)
                {
                    consumerChannel.BasicPublish(retryExchangeParam.Exchange, DefaultRoutingKey, ea.BasicProperties, ea.Body);
                    consumerChannel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    consumerChannel.BasicNack(ea.DeliveryTag, false, false);
                }
            }
        };
        channel.BasicConsume(queue: queueParam.Queue, autoAck: false, consumer: consumer);
    }

    private void Consume(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam,
        Func<Action<byte[]>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry)
    {
        var retryExchangeParam = consumeRetry > 0
            ? CreateExchangeParam(queueParam.Queue, true, ExchangeRole.Retry)
            : null;
        var channel = GetConsumerChannel(
            exchangeParam,
            queueParam,
            retryExchangeParam,
            dlxExchangeParam,
            dlxQueueParam,
            prefetchCount);
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (_, ea) =>
        {
            var consumerChannel = consumer.Model;
            try
            {
                resolve.Invoke()(ea.Body.ToArray());
                consumerChannel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                var retryCount = GetRetryCount(ea.BasicProperties);
                IncreaseRetryCount(ea.BasicProperties);
                if (retryCount < consumeRetry && retryExchangeParam is not null)
                {
                    consumerChannel.BasicPublish(retryExchangeParam.Exchange, DefaultRoutingKey, ea.BasicProperties, ea.Body);
                    consumerChannel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    consumerChannel.BasicNack(ea.DeliveryTag, false, false);
                }
            }
        };
        channel.BasicConsume(queue: queueParam.Queue, autoAck: false, consumer: consumer);
    }

    private static int GetRetryCount(IBasicProperties properties)
    {
        properties.Headers ??= new Dictionary<string, object>();
        if (properties.Headers.TryGetValue(RetryCount, out var retryCount))
            return (int)retryCount;
        retryCount = 0;
        properties.Headers.Add(RetryCount, retryCount);
        return (int)retryCount;
    }

    private static void IncreaseRetryCount(IBasicProperties properties)
    {
        properties.Headers ??= new Dictionary<string, object>();
        if (!properties.Headers.TryGetValue(RetryCount, out var retryCount))
            properties.Headers.Add(RetryCount, 0);
        var count = (int)(retryCount ?? 0);
        properties.Headers[RetryCount] = ++count;
    }
}