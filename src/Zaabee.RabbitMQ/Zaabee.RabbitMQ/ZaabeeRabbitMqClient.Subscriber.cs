namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    private const string XDeath = "x-death";
    private const string XDeathCount = "count";

    private void Consume<T>(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam,
        Func<Action<T?>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry)
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

    private void Consume(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam,
        Func<Action<byte[]>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry)
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
                resolve.Invoke()(ea.Body.ToArray());
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                var retryCount = GetRetryCount(ea.BasicProperties);
                channel.BasicNack(ea.DeliveryTag, false, retryCount < consumeRetry);
            }
        }
    }

    private void Consume<T>(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam,
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry)
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
    }

    private void Consume(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam,
        Func<Func<byte[], Task>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry)
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
                await resolve.Invoke()(ea.Body.ToArray());
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