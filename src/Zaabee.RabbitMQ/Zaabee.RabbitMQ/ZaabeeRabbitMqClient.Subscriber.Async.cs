namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    private readonly ConcurrentDictionary<string, IModel> _subscriberAsyncChannelDic = new();

    private ValueTask ListenAsync<T>(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount)
    {
        var channel = GetConsumerAsyncChannel(
            exchangeParam,
            queueParam,
            prefetchCount: prefetchCount);
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body;
                var msg = _serializer.FromBytes<T>(body.ToArray());
                await resolve.Invoke()(msg);
            }
            finally
            {
                channel.BasicAck(ea.DeliveryTag, false);
                await Task.Yield();
            }
        };
        channel.BasicConsume(queue: queueParam.Queue, autoAck: false, consumer: consumer);
        return default;
    }

    private ValueTask ConsumeAsync<T>(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam,
        Func<Func<T?, Task>> resolve,
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

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body;
                var msg = _serializer.FromBytes<T>(body.ToArray());
                await resolve.Invoke()(msg);
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                var retryCount = GetRetryCount(ea.BasicProperties);
                channel.BasicNack(ea.DeliveryTag, false, retryCount < consumeRetry);
            }
            finally
            {
                await Task.Yield();
            }
        };
        channel.BasicConsume(queue: queueParam.Queue, autoAck: false, consumer: consumer);
        return default;
    }
}